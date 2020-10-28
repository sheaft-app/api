using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Options;
using Sheaft.Exceptions;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Handlers
{
    public class ProducerCommandsHandler : ResultsHandler,
        IRequestHandler<RegisterProducerCommand, Result<Guid>>,
        IRequestHandler<UpdateProducerCommand, Result<bool>>,
        IRequestHandler<CheckProducerConfigurationCommand, Result<bool>>,
        IRequestHandler<EnsureProducerDocumentsValidatedCommand, Result<bool>>,
        IRequestHandler<UpdateProducerTagsCommand, Result<bool>>
    {
        private readonly RoleOptions _roleOptions;

        public ProducerCommandsHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<ProducerCommandsHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
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
                        Email = request.Legals.Email,
                        Siret = request.Legals.Siret,
                        Kind = request.Legals.Kind,
                        VatIdentifier = request.Legals.VatIdentifier,
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

                    return Created(producer.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(UpdateProducerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.Id, token);

                producer.SetName(request.Name);
                producer.SetFirstname(request.FirstName);
                producer.SetLastname(request.LastName);
                producer.SetEmail(request.Email);
                producer.SetProfileKind(request.Kind);
                producer.SetPhone(request.Phone);
                producer.SetDescription(request.Description);
                producer.SetOpenForNewBusiness(request.OpenForNewBusiness);

                var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                producer.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country, department, request.Address.Longitude, request.Address.Latitude);
                                
                if (request.Tags != null)
                {
                    var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                    producer.SetTags(tags);
                }

                await _context.SaveChangesAsync(token);

                var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
                {
                    UserId = producer.Id,
                    Picture = request.Picture,
                    SkipAuthUpdate = true
                }, token);

                if (!resultImage.Success)
                    return Failed<bool>(resultImage.Exception);

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
                    return authResult;

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckProducerConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var business = await _mediatr.Process(new CheckBusinessLegalConfigurationCommand(request.RequestUser) { UserId = request.ProducerId }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Process(new CheckWalletPaymentsConfigurationCommand(request.RequestUser) { UserId = request.ProducerId }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(EnsureProducerDocumentsValidatedCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producerLegal = await _context.GetSingleAsync<BusinessLegal>(l => l.User.Id == request.ProducerId, token);
                if (!producerLegal.Documents.Any() || producerLegal.Documents.Any(d => d.Status != DocumentStatus.Validated))
                    return BadRequest<bool>(MessageKind.Producer_Documents_NotValidated);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(UpdateProducerTagsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.ProducerId, token);

                var productTags = await _context.Products.Where(p => p.Producer.Id == producer.Id).SelectMany(p => p.Tags).Select(p => p.Tag).Distinct().ToListAsync(token);
                producer.SetTags(productTags);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}