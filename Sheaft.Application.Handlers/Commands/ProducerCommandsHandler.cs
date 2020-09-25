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
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class ProducerCommandsHandler : ResultsHandler,
        IRequestHandler<RegisterProducerCommand, Result<Guid>>,
        IRequestHandler<UpdateProducerCommand, Result<bool>>,
        IRequestHandler<CheckProducerConfigurationCommand, Result<bool>>,
        IRequestHandler<CheckProducerDocumentsCreatedCommand, Result<bool>>,
        IRequestHandler<CheckProducerDocumentsReviewedCommand, Result<bool>>,
        IRequestHandler<CheckProducerDocumentsValidatedCommand, Result<bool>>
    {
        private readonly IImageService _imageService;
        private readonly RoleOptions _roleOptions;
        private readonly IDistributedCache _cache;

        public ProducerCommandsHandler(
            IDistributedCache cache,
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IImageService imageService,
            ILogger<ProducerCommandsHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _imageService = imageService;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(RegisterProducerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var producer = await _context.FindByIdAsync<Producer>(request.Id, token);
                    if (producer != null)
                        return Conflict<Guid>(MessageKind.Register_User_AlreadyExists);

                    var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                    var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                    var address = request.Address != null ?
                        new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City,
                        request.Address.Country, department, request.Address.Longitude, request.Address.Latitude)
                        : null;

                    producer = new Producer(Guid.NewGuid(), request.Name, request.FirstName, request.LastName, request.Email,
                        address, request.OpenForNewBusiness, request.Phone, request.Description);

                    if (request.Tags != null && request.Tags.Any())
                    {
                        var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                        producer.SetTags(tags);
                    }

                    var resultImage = await _imageService.HandleUserImageAsync(producer.Id, request.Picture, token);
                    if (!resultImage.Success)
                        return Failed<Guid>(resultImage.Exception);

                    producer.SetPicture(resultImage.Data);

                    await _context.AddAsync(producer, token);
                    await _context.SaveChangesAsync(token);

                    var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Store.Id };
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
                        UserId = request.RequestUser.Id,
                        Owner = request.Legals.Owner
                    }, token);

                    if (!result.Success)
                        return result;

                    await transaction.CommitAsync(token);
                    await _cache.RemoveAsync(producer.Id.ToString("N"));

                    if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                    {
                        await _mediatr.Post(new CreateSponsoringCommand(request.RequestUser) { Code = request.SponsoringCode, UserId = producer.Id }, token);
                    }

                    return Created(request.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(UpdateProducerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
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

                var resultImage = await _imageService.HandleUserImageAsync(producer.Id, request.Picture, token);
                if (!resultImage.Success)
                    return Failed<bool>(resultImage.Exception);

                producer.SetPicture(resultImage.Data);

                if (request.Tags != null)
                {
                    var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                    producer.SetTags(tags);
                }

                _context.Update(producer);
                var result = await _context.SaveChangesAsync(token);

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

                await _cache.RemoveAsync(producer.Id.ToString("N"));
                return Ok(result > 0);
            });
        }

        public async Task<Result<bool>> Handle(CheckProducerConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var business = await _mediatr.Process(new CheckBusinessLegalConfigurationCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Process(new CheckWalletPaymentsConfigurationCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                var declaration = await _mediatr.Process(new CheckDeclarationConfigurationCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!declaration.Success)
                    return Failed<bool>(declaration.Exception);

                return Ok(true);
            });
        }

        public Task<Result<bool>> Handle(CheckProducerDocumentsCreatedCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(CheckProducerDocumentsReviewedCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(CheckProducerDocumentsValidatedCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}