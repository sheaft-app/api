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
    public class BusinessCommandsHandler : ResultsHandler,
        IRequestHandler<RegisterProducerCommand, Result<Guid>>,
        IRequestHandler<RegisterStoreCommand, Result<Guid>>,
        IRequestHandler<UpdateProducerCommand, Result<bool>>,
        IRequestHandler<UpdateStoreCommand, Result<bool>>
    {
        private readonly IImageService _imageService;
        private readonly RoleOptions _roleOptions;
        private readonly HttpClient _httpClient;
        private readonly AuthOptions _authOptions;
        private readonly IDistributedCache _cache;

        public BusinessCommandsHandler(
            IOptionsSnapshot<AuthOptions> authOptions,
            IDistributedCache cache,
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IImageService imageService,
            IHttpClientFactory httpClientFactory,
            ILogger<BusinessCommandsHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _authOptions = authOptions.Value;
            _roleOptions = roleOptions.Value;
            _imageService = imageService;
            _cache = cache;

            _httpClient = httpClientFactory.CreateClient("companies");
            _httpClient.BaseAddress = new Uri(_authOptions.Url);
            _httpClient.SetToken(_authOptions.Scheme, _authOptions.ApiKey);
        }

        public async Task<Result<Guid>> Handle(RegisterProducerCommand request, CancellationToken token)
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
                        new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City,
                        request.Address.Country, department, request.Address.Longitude, request.Address.Latitude)
                        : null;

                    var producerResult = await CreateProducerAsync(request, address, token);
                    if (!producerResult.Success)
                        return Failed<Guid>(producerResult.Exception);

                    var producer = producerResult.Data;
                    await _context.AddAsync(producer, token);
                    await _context.SaveChangesAsync(token);

                    var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Store.Id };
                    var oidcResult = await UpdateOidcUserAsync(producer, roles, token);
                    if (!oidcResult.Success)
                        return Failed<Guid>(new BadRequestException(MessageKind.Oidc_Register_Error, oidcResult.Exception?.Message));

                    var result = await CreateBusinessLegalsAsync(request.Legals, request.RequestUser, token);
                    if (!result.Success)
                        return result;

                    await RegisterSponsorAsync(request.SponsoringCode, user, request.RequestUser, token);
                    await transaction.CommitAsync(token);

                    await _cache.RemoveAsync(user.Id.ToString("N"));
                    return Created(request.Id);
                }
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

                    var storeResult = await CreateStoreAsync(request, address, token);
                    if (!storeResult.Success)
                        return Failed<Guid>(storeResult.Exception);

                    var store = storeResult.Data;

                    await _context.AddAsync(storeResult, token);
                    await _context.SaveChangesAsync(token);

                    var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Store.Id };
                    var oidcResult = await UpdateOidcUserAsync(store, roles, token);
                    if (!oidcResult.Success)
                        return Failed<Guid>(new BadRequestException(MessageKind.Oidc_Register_Error, oidcResult.Exception?.Message));

                    var result = await CreateBusinessLegalsAsync(request.Legals, request.RequestUser, token);
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

        public async Task<Result<bool>> Handle(UpdateProducerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Producer>(request.Id, token);

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

                _context.Update(entity);
                var result = await _context.SaveChangesAsync(token);

                var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Producer.Id };
                var oidcResult = await UpdateOidcUserAsync(entity, roles, token);
                if (!oidcResult.Success)
                    return Failed<bool>(new BadRequestException(MessageKind.Oidc_UpdateProfile_Error, oidcResult.Exception?.Message));

                await _cache.RemoveAsync(entity.Id.ToString("N"));
                return Ok(result > 0);
            });
        }

        private async Task<Result<Guid>> CreateBusinessLegalsAsync(BusinessLegalInput legals, RequestUser requestUser, CancellationToken token)
        {
            return await _mediatr.Process(new CreateBusinessLegalCommand(requestUser)
            {
                Address = legals.Address,
                Email = legals.Email,
                Siret = legals.Siret,
                Kind = legals.Kind,
                VatIdentifier = legals.VatIdentifier,
                UserId = requestUser.Id,
                Owner = legals.Owner
            }, token);
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

        private async Task<Result<Store>> CreateStoreAsync(RegisterStoreCommand request, UserAddress address, CancellationToken token)
        {
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
                return Failed<Store>(resultImage.Exception);

            store.SetPicture(resultImage.Data);

            return Ok(store);
        }

        private async Task<Result<Producer>> CreateProducerAsync(RegisterProducerCommand request, UserAddress address, CancellationToken token)
        {
            var producer = new Producer(Guid.NewGuid(), request.Name, request.FirstName, request.LastName, request.Email,
                address, request.OpenForNewBusiness, request.Phone, request.Description);

            if (request.Tags != null && request.Tags.Any())
            {
                var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                producer.SetTags(tags);
            }

            var resultImage = await _imageService.HandleUserImageAsync(producer.Id, request.Picture, token);
            if (!resultImage.Success)
                return Failed<Producer>(resultImage.Exception);

            producer.SetPicture(resultImage.Data);

            return Ok(producer);
        }
    }
}