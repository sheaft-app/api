using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Mediatr.Auth.Commands;
using Sheaft.Mediatr.Legal.Commands;
using Sheaft.Mediatr.Sponsor.Commands;
using Sheaft.Mediatr.User.Commands;
using Sheaft.Options;

namespace Sheaft.Mediatr.Producer.Commands
{
    public class RegisterProducerCommand : Command<Guid>
    {
        protected RegisterProducerCommand()
        {
            
        }
        [JsonConstructor]
        public RegisterProducerCommand(RequestUser requestUser) : base(requestUser)
        {
            ProducerId = RequestUser.Id;
        }

        public Guid ProducerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public string SponsoringCode { get; set; }
        public BusinessLegalInputDto Legals { get; set; }
        public AddressDto Address { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public bool NotSubjectToVat { get; set; }
        public IEnumerable<Guid> Tags { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            ProducerId = RequestUser.Id;
        }
    }

    public class RegisterProducerCommandHandler : CommandsHandler,
        IRequestHandler<RegisterProducerCommand, Result<Guid>>
    {
        private readonly RoleOptions _roleOptions;

        public RegisterProducerCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<RegisterProducerCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result<Guid>> Handle(RegisterProducerCommand request, CancellationToken token)
        {
            var producer = await _context.Producers.SingleOrDefaultAsync(r => r.Id == request.ProducerId || r.Email == request.Email, token);
            if (producer != null)
                return Failure<Guid>(MessageKind.Register_User_AlreadyExists);

            var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
            var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

            var address = request.Address != null
                ? new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City,
                    request.Address.Country, department, request.Address.Longitude, request.Address.Latitude)
                : null;
            
            producer = new Domain.Producer(request.ProducerId, request.Name, request.FirstName, request.LastName,
                request.Email,
                address, request.OpenForNewBusiness, request.Phone);
            producer.SetNotSubjectToVat(request.NotSubjectToVat);

            if (request.Tags != null && request.Tags.Any())
            {
                var tags = await _context.Tags.Where(t => request.Tags.Contains(t.Id)).ToListAsync(token);
                producer.SetTags(tags);
            }

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                await _context.AddAsync(producer, token);
                await _context.SaveChangesAsync(token);

                var resultImage = await _mediatr.Process(new UpdateUserPreviewCommand(request.RequestUser)
                {
                    UserId = producer.Id,
                    Picture = request.Picture,
                    SkipAuthUpdate = true
                }, token);

                if (!resultImage.Succeeded)
                    return Failure<Guid>(resultImage);

                var roles = new List<Guid> {_roleOptions.Owner.Id, _roleOptions.Producer.Id};
                var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
                {
                    Email = producer.Email,
                    FirstName = producer.FirstName,
                    LastName = producer.LastName,
                    Name = producer.Name,
                    Phone = producer.Phone,
                    Picture = producer.Picture,
                    Roles = roles,
                    UserId = producer.Id
                }, token);

                if (!authResult.Succeeded)
                    return Failure<Guid>(authResult);

                var result = await _mediatr.Process(new CreateBusinessLegalCommand(request.RequestUser)
                {
                    Address = request.Legals.Address,
                    Name = request.Legals.Name,
                    Email = request.Legals.Email,
                    Siret = request.Legals.Siret,
                    Kind = request.Legals.Kind,
                    VatIdentifier = request.NotSubjectToVat ? null : request.Legals.VatIdentifier,
                    UserId = producer.Id,
                    Owner = request.Legals.Owner
                }, token);

                if (!result.Succeeded)
                    return result;

                await transaction.CommitAsync(token);

                if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                {
                    _mediatr.Post(new CreateSponsoringCommand(request.RequestUser)
                        {Code = request.SponsoringCode, UserId = producer.Id});
                }

                return Success(producer.Id);
            }
        }
    }
}