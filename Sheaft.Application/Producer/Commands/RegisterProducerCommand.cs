using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class RegisterProducerCommand : Command<Guid>
    {
        [JsonConstructor]
        public RegisterProducerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

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
        private readonly IBlobService _blobService;

        public RegisterProducerCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IBlobService blobService,
            ILogger<RegisterProducerCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _blobService = blobService;
        }

        public async Task<Result<Guid>> Handle(RegisterProducerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var producer = await _context.FindByIdAsync<Producer>(request.RequestUser.Id, token);
                    if (producer != null)
                        return Conflict<Guid>(MessageKind.Register_User_AlreadyExists);

                    var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                    var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                    var address = request.Address != null ?
                        new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City,
                        request.Address.Country, department, request.Address.Longitude, request.Address.Latitude)
                        : null;

                    producer = new Producer(request.RequestUser.Id, request.Name, request.FirstName, request.LastName, request.Email,
                        address, request.OpenForNewBusiness, request.Phone, request.Description);
                    producer.SetNotSubjectToVat(request.NotSubjectToVat);

                    if (request.Tags != null && request.Tags.Any())
                    {
                        var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                        producer.SetTags(tags);
                    }

                    await _context.AddAsync(producer, token);
                    await _context.SaveChangesAsync(token);

                    var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
                    {
                        UserId = producer.Id,
                        Picture = request.Picture,
                        SkipAuthUpdate = true
                    }, token);

                    if (!resultImage.Success)
                        return Failed<Guid>(resultImage.Exception);

                    var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Producer.Id };
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

                    if (!authResult.Success)
                        return Failed<Guid>(authResult.Exception);

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

                    if (!result.Success)
                        return result;

                    await transaction.CommitAsync(token);

                    if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                    {
                        _mediatr.Post(new CreateSponsoringCommand(request.RequestUser) { Code = request.SponsoringCode, UserId = producer.Id });
                    }

                    _mediatr.Post(new ProducerRegisteredEvent(request.RequestUser) { ProducerId = producer.Id });

                    return Created(producer.Id);
                }
            });
        }
    }
}
