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

namespace Sheaft.Application.Handlers
{
    public class StoreCommandsHandler : ResultsHandler,
        IRequestHandler<CheckStoreConfigurationCommand, Result<bool>>,
        IRequestHandler<RegisterStoreCommand, Result<Guid>>,
        IRequestHandler<UpdateStoreCommand, Result<bool>>
    {
        private readonly RoleOptions _roleOptions;

        public StoreCommandsHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<StoreCommandsHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckStoreConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var business = await _mediatr.Process(new CheckBusinessLegalConfigurationCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Process(new CheckWalletPaymentsConfigurationCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<Guid>> Handle(RegisterStoreCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var store = await _context.FindByIdAsync<Store>(request.RequestUser.Id, token);
                    if (store != null)
                        return Conflict<Guid>(MessageKind.Register_User_AlreadyExists);

                    var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                    var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                    var address = request.Address != null ?
                        new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country,
                            department, request.Address.Longitude, request.Address.Latitude)
                        : null;

                    var openingHours = new List<TimeSlotHour>();
                    if (request.OpeningHours != null)
                    {
                        foreach (var oh in request.OpeningHours)
                        {
                            openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                        }
                    }

                    store = new Store(request.RequestUser.Id, request.Name, request.FirstName, request.LastName, request.Email,
                        address, openingHours, request.OpenForNewBusiness, request.Phone, request.Description);

                    if (request.Tags != null && request.Tags.Any())
                    {
                        var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                        store.SetTags(tags);
                    }

                    await _context.AddAsync(store, token);
                    await _context.SaveChangesAsync(token);

                    var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
                    {
                        UserId = store.Id,
                        Picture = request.Picture,
                        SkipAuthUpdate = true
                    }, token);

                    if (!resultImage.Success)
                        return Failed<Guid>(resultImage.Exception);

                    var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Store.Id };
                    var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
                    {
                        Email = store.Email,
                        FirstName = store.FirstName,
                        LastName = store.LastName,
                        Name = store.Name,
                        Phone = store.Phone,
                        Picture = store.Picture,
                        Roles = roles,
                        UserId = store.Id
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
                        UserId = store.Id,
                        Owner = request.Legals.Owner
                    }, token);

                    if (!result.Success)
                        return result;

                    await transaction.CommitAsync(token);

                    if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                    {
                        _mediatr.Post(new CreateSponsoringCommand(request.RequestUser) { Code = request.SponsoringCode, UserId = store.Id });
                    }

                    return Created(store.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(UpdateStoreCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var store = await _context.GetByIdAsync<Store>(request.Id, token);

                store.SetName(request.Name);
                store.SetFirstname(request.FirstName);
                store.SetLastname(request.LastName);
                store.SetEmail(request.Email);
                store.SetProfileKind(request.Kind);
                store.SetPhone(request.Phone);
                store.SetDescription(request.Description);
                store.SetOpenForNewBusiness(request.OpenForNewBusiness);

                var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                store.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country, department, request.Address.Longitude, request.Address.Latitude);

                if (request.Tags != null)
                {
                    var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                    store.SetTags(tags);
                }

                if (request.OpeningHours != null)
                {
                    var openingHours = new List<TimeSlotHour>();
                    foreach (var oh in request.OpeningHours)
                    {
                        openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                    }

                    store.SetOpeningHours(openingHours);
                }

                await _context.SaveChangesAsync(token);

                var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
                {
                    UserId = store.Id,
                    Picture = request.Picture,
                    SkipAuthUpdate = true
                }, token);

                if (!resultImage.Success)
                    return Failed<bool>(resultImage.Exception);

                var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Store.Id };
                var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
                {
                    Email = store.Email,
                    FirstName = store.FirstName,
                    LastName = store.LastName,
                    Name = store.Name,
                    Phone = store.Phone,
                    Picture = store.Picture,
                    Roles = roles,
                    UserId = store.Id
                }, token);

                if (!authResult.Success)
                    return authResult;

                return Ok(true);
            });
        }
    }
}