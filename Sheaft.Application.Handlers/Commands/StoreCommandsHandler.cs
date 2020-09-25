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
using Sheaft.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Options;
using Sheaft.Exceptions;
using System.Net.Http;
using Sheaft.Application.Models;
using Newtonsoft.Json;
using System.Text;
using Sheaft.Application.Events;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{

    public class StoreCommandsHandler : ResultsHandler,
        IRequestHandler<CheckStoreConfigurationCommand, Result<bool>>,
        IRequestHandler<RegisterStoreCommand, Result<Guid>>,
        IRequestHandler<UpdateStoreCommand, Result<bool>>
    {
        private readonly IImageService _imageService;
        private readonly RoleOptions _roleOptions;
        private readonly HttpClient _httpClient;
        private readonly AuthOptions _authOptions;
        private readonly IDistributedCache _cache;

        public StoreCommandsHandler(
            IOptionsSnapshot<AuthOptions> authOptions,
            IDistributedCache cache,
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IImageService imageService,
            IHttpClientFactory httpClientFactory,
            ILogger<StoreCommandsHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _authOptions = authOptions.Value;
            _roleOptions = roleOptions.Value;
            _imageService = imageService;
            _cache = cache;

            _httpClient = httpClientFactory.CreateClient("stores");
            _httpClient.BaseAddress = new Uri(_authOptions.Url);
            _httpClient.SetToken(_authOptions.Scheme, _authOptions.ApiKey);
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
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var user = await _context.FindByIdAsync<User>(request.Id, token);
                    if (user != null)
                        return Conflict<Guid>(MessageKind.Register_User_AlreadyExists);

                    var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                    var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                    var address = request.Address != null ?
                        new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country,
                            department, request.Address.Longitude, request.Address.Latitude)
                        : null;

                    var openingHours = new List<TimeSlotHour>();
                    if (request.OpeningHours == null)
                    {
                        foreach (var oh in request.OpeningHours)
                        {
                            openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                        }
                    }

                    var store = new Store(Guid.NewGuid(), request.Name, request.FirstName, request.LastName, request.Email,
                        address, openingHours, request.OpenForNewBusiness, request.Phone, request.Description);

                    if (request.Tags != null && request.Tags.Any())
                    {
                        var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                        store.SetTags(tags);
                    }

                    var resultImage = await _imageService.HandleUserImageAsync(store.Id, request.Picture, token);
                    if (!resultImage.Success)
                        return Failed<Guid>(resultImage.Exception);

                    store.SetPicture(resultImage.Data);

                    await _context.AddAsync(store, token);
                    await _context.SaveChangesAsync(token);

                    var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Store.Id };
                    var oidcResult = await UpdateOidcUserAsync(store, roles, token);
                    if (!oidcResult.Success)
                        return Failed<Guid>(new BadRequestException(MessageKind.Oidc_Register_Error, oidcResult.Exception?.Message));

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

                    await RegisterSponsorAsync(request.SponsoringCode, user, request.RequestUser, token);
                    await transaction.CommitAsync(token);

                    await _cache.RemoveAsync(user.Id.ToString("N"));
                    return Created(request.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(UpdateStoreCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Store>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetFirstname(request.FirstName);
                entity.SetLastname(request.LastName);
                entity.SetEmail(request.Email);
                entity.SetProfileKind(request.Kind);
                entity.SetPhone(request.Phone);
                entity.SetDescription(request.Description);
                entity.SetOpenForNewBusiness(request.OpenForNewBusiness);

                var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                entity.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country, department, request.Address.Longitude, request.Address.Latitude);

                var resultImage = await _imageService.HandleUserImageAsync(entity.Id, request.Picture, token);
                if (!resultImage.Success)
                    return Failed<bool>(resultImage.Exception);

                entity.SetPicture(resultImage.Data);

                if (request.Tags != null)
                {
                    var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                    entity.SetTags(tags);
                }

                if (request.OpeningHours != null)
                {
                    var openingHours = new List<TimeSlotHour>();
                    foreach (var oh in request.OpeningHours)
                    {
                        openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                    }

                    entity.SetOpeningHours(openingHours);
                }

                _context.Update(entity);
                var result = await _context.SaveChangesAsync(token);

                var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Store.Id };
                var oidcResult = await UpdateOidcUserAsync(entity, roles, token);
                if (!oidcResult.Success)
                    return Failed<bool>(new BadRequestException(MessageKind.Oidc_UpdateProfile_Error, oidcResult.Exception?.Message));

                await _cache.RemoveAsync(entity.Id.ToString("N"));
                return Ok(result > 0);
            });
        }

        private async Task<Result<bool>> UpdateOidcUserAsync(Business entity, List<Guid> roles, CancellationToken token)
        {
            var oidcUser = new IdentityUserInput(entity.Id, entity.Email, entity.Name, entity.FirstName, entity.LastName, roles)
            {
                Phone = entity.Phone,
                Picture = entity.Picture
            };

            var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile,
                new StringContent(JsonConvert.SerializeObject(oidcUser), Encoding.UTF8, "application/json"), token);

            if (!oidcResult.IsSuccessStatusCode)
                return Failed<bool>(new Exception(await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

            return Ok(true);
        }

        private async Task RegisterSponsorAsync(string code, User user, RequestUser requestUser, CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                var sponsor = await _context.FindSingleAsync<User>(u => u.SponsorshipCode == code, token);
                if (sponsor == null)
                    throw new NotFoundException(MessageKind.Register_User_SponsorCode_NotFound);

                await _context.AddAsync(new Sponsoring(sponsor, user), token);
                await _context.SaveChangesAsync(token);

                await _mediatr.Post(new UserSponsoredEvent(requestUser)
                {
                    SponsorId = sponsor.Id,
                    SponsoredId = user.Id
                }, token);

                await _mediatr.Post(new CreateUserPointsCommand(requestUser)
                {
                    CreatedOn = DateTimeOffset.UtcNow,
                    Kind = PointKind.Sponsoring,
                    UserId = sponsor.Id
                }, token);
            }
        }
    }
}