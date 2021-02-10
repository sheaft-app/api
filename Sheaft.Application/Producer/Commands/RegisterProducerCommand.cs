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
using Sheaft.Application.Auth.Commands;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Options;
using Sheaft.Application.Legal.Commands;
using Sheaft.Application.Picture.Commands;
using Sheaft.Application.Sponsor.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Producer;

namespace Sheaft.Application.Producer.Commands
{
    public class RegisterProducerCommand : Command<Guid>
    {
        [JsonConstructor]
        public RegisterProducerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string SponsoringCode { get; set; }
        public BusinessLegalInput Legals { get; set; }
        public FullAddressInput Address { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public bool NotSubjectToVat { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
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
            var producer = await _context.FindByIdAsync<Domain.Producer>(request.ProducerId, token);
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
                address, request.OpenForNewBusiness, request.Phone, request.Description);
            producer.SetNotSubjectToVat(request.NotSubjectToVat);

            if (request.Tags != null && request.Tags.Any())
            {
                var tags = await _context.FindAsync<Domain.Tag>(t => request.Tags.Contains(t.Id), token);
                producer.SetTags(tags);
            }

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                await _context.AddAsync(producer, token);
                await _context.SaveChangesAsync(token);

                var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
                {
                    UserId = producer.Id,
                    Picture = request.Picture,
                    SkipAuthUpdate = true
                }, token);

                if (!resultImage.Succeeded)
                    return Failure<Guid>(resultImage.Exception);

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
                    return Failure<Guid>(authResult.Exception);

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