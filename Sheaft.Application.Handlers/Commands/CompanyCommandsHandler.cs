using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using System.Linq;
using System.Collections.Generic;
using Sheaft.Interop.Enums;
using Sheaft.Services.Interop;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Options;
using Sheaft.Exceptions;
using System.Net.Http;
using Sheaft.Models.Inputs;
using Newtonsoft.Json;
using System.Text;
using Sheaft.Application.Events;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class CompanyCommandsHandler : CommandsHandler,
        IRequestHandler<RegisterProducerCommand, Result<Guid>>,
        IRequestHandler<RegisterStoreCommand, Result<Guid>>,
        IRequestHandler<UpdateProducerCommand, Result<bool>>,
        IRequestHandler<UpdateStoreCommand, Result<bool>>,
        IRequestHandler<DeleteCompanyCommand, Result<bool>>,
        IRequestHandler<SetCompanyLegalsCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IQueueService _queueService;
        private readonly IImageService _imageService;
        private readonly RoleOptions _roleOptions;
        private readonly HttpClient _httpClient;
        private readonly AuthOptions _authOptions;
        private readonly IDistributedCache _cache;

        public CompanyCommandsHandler(
            IOptionsSnapshot<AuthOptions> authOptions,
            IDistributedCache cache,
            IAppDbContext context,
            IQueueService queueService,
            IImageService imageService,
            IHttpClientFactory httpClientFactory,
            ILogger<CompanyCommandsHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions) : base(logger)
        {
            _authOptions = authOptions.Value;
            _roleOptions = roleOptions.Value;
            _imageService = imageService;
            _queueService = queueService;
            _context = context;
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
                    var user = await _context.FindByIdAsync<User>(request.Owner.Id, token);
                    if (user != null)
                        return Conflict<Guid>(MessageKind.Register_User_AlreadyExists);

                    var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                    var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                    var address = request.Address != null ?
                        new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City,
                        request.Address.Country, department, request.Address.Longitude, request.Address.Latitude)
                        : null;

                    var producer = await CreateProducerAsync(request, address, token);

                    await _context.AddAsync(producer, token);
                    await _context.SaveChangesAsync(token);

                    var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Producer.Id };
                    var oidcUser = new IdentityUserInput(producer.Id, producer.Email, producer.Name, producer.FirstName, producer.LastName, roles)
                    {
                        Phone = request.Phone,
                        Picture = producer.Picture
                    };

                    var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile,
                        new StringContent(JsonConvert.SerializeObject(oidcUser), Encoding.UTF8, "application/json"), token);

                    if (!oidcResult.IsSuccessStatusCode)
                        return Failed<Guid>(new BadRequestException(MessageKind.Oidc_Register_Error, await oidcResult.Content.ReadAsStringAsync()));

                    await RegisterSponsorAsync(request.Owner.SponsoringCode, user, request.RequestUser, token);
                    await transaction.CommitAsync(token);

                    await _cache.RemoveAsync(user.Id.ToString("N"));
                    return Created(request.Owner.Id);
                }
            });
        }

        public async Task<Result<Guid>> Handle(RegisterStoreCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var user = await _context.FindByIdAsync<User>(request.Owner.Id, token);
                    if (user != null)
                        return Conflict<Guid>(MessageKind.Register_User_AlreadyExists);

                    var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                    var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                    var address = request.Address != null ?
                        new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country,
                            department, request.Address.Longitude, request.Address.Latitude)
                        : null;

                    var store = await CreateStoreAsync(request, address, token);

                    await _context.AddAsync(store, token);
                    await _context.SaveChangesAsync(token);

                    var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Producer.Id };
                    var oidcUser = new IdentityUserInput(store.Id, store.Email, store.Name, store.FirstName, store.LastName, roles)
                    {
                        Phone = request.Phone,
                        Picture = store.Picture
                    };

                    var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile,
                        new StringContent(JsonConvert.SerializeObject(oidcUser), Encoding.UTF8, "application/json"), token);

                    if (!oidcResult.IsSuccessStatusCode)
                        return Failed<Guid>(new BadRequestException(MessageKind.Oidc_Register_Error, await oidcResult.Content.ReadAsStringAsync()));

                    await RegisterSponsorAsync(request.Owner.SponsoringCode, user, request.RequestUser, token);

                    await transaction.CommitAsync(token);

                    await _cache.RemoveAsync(user.Id.ToString("N"));
                    return Created(request.Owner.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(UpdateStoreCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Store>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetEmail(request.Email);
                entity.SetProfileKind(request.Kind);
                entity.SetPhone(request.Phone);
                entity.SetDescription(request.Description);
                entity.SetSiret(request.Siret);
                entity.SetVatIdentifier(request.VatIdentifier);
                entity.SetOpenForNewBusiness(request.OpenForNewBusiness);

                var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                entity.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country, department, request.Address.Longitude, request.Address.Latitude);

                if (request.BillingAddress != null)
                    entity.SetBillingAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                        request.Address.City, request.Address.Country);

                var picture = await _imageService.HandleUserImageAsync(entity.Id, request.Picture, token);
                entity.SetPicture(picture);

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
                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(UpdateProducerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Producer>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetEmail(request.Email);
                entity.SetProfileKind(request.Kind);
                entity.SetPhone(request.Phone);
                entity.SetDescription(request.Description);
                entity.SetSiret(request.Siret);
                entity.SetVatIdentifier(request.VatIdentifier);
                entity.SetOpenForNewBusiness(request.OpenForNewBusiness);

                var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                entity.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country, department, request.Address.Longitude, request.Address.Latitude);
                
                if(request.BillingAddress != null)
                    entity.SetBillingAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                        request.Address.City, request.Address.Country);

                var picture = await _imageService.HandleUserImageAsync(entity.Id, request.Picture, token);
                entity.SetPicture(picture);

                if (request.Tags != null)
                {
                    var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                    entity.SetTags(tags);
                }

                _context.Update(entity);
                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(DeleteCompanyCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Company>(request.Id, token);
                var email = entity.Email;

                entity.Close(request.Reason);
                _context.Update(entity);

                var success = await _context.SaveChangesAsync(token) > 0;

                await _queueService.ProcessCommandAsync(RemoveUserDataCommand.QUEUE_NAME,
                    new RemoveUserDataCommand(request.RequestUser)
                    {
                        Id = request.Id,
                        Email = email
                    }, token);

                return Ok(success);
            });
        }

        public async Task<Result<bool>> Handle(SetCompanyLegalsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Company>(request.Id, token);

                entity.SetLegalAddress(request.LegalAddress.Line1, request.LegalAddress.Line2, request.LegalAddress.Zipcode, request.LegalAddress.City, request.LegalAddress.Country);
                entity.SetBillingAddress(request.BillingAddress.Line1, request.BillingAddress.Line2, request.BillingAddress.Zipcode, request.BillingAddress.City, request.BillingAddress.Country);
                
                entity.SetLegalKind(request.Legal);
                entity.SetBirthdate(request.Birthdate);
                entity.SetNationality(request.Nationality);
                entity.SetCountryOfResidence(request.CountryOfResidence);

                _context.Update(entity);

                //TODO If Changes detected -> revalidation of KYC required

                var success = await _context.SaveChangesAsync(token) > 0;
                return Ok(success);
            });
        }

        private async Task RegisterSponsorAsync(string code, User user, RequestUser requestUser, CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                var sponsor = await _context.FindSingleAsync<User>(u => u.SponsorshipCode == code, token);
                if (sponsor == null)
                    throw new NotFoundException(MessageKind.Register_UserWithSponsorCode_NotFound);

                await _context.AddAsync(new Sponsoring(sponsor, user), token);
                await _context.SaveChangesAsync(token);

                await _queueService.ProcessEventAsync(UserSponsoredEvent.QUEUE_NAME,
                    new UserSponsoredEvent(requestUser)
                    {
                        SponsorId = sponsor.Id,
                        SponsoredId = user.Id
                    }, token);

                await _queueService.ProcessCommandAsync(CreateUserPointsCommand.QUEUE_NAME,
                    new CreateUserPointsCommand(requestUser)
                    {
                        CreatedOn = DateTimeOffset.UtcNow,
                        Kind = PointKind.Sponsoring,
                        UserId = sponsor.Id
                    }, token);
            }
        }

        private async Task<Store> CreateStoreAsync(RegisterStoreCommand request, UserAddress address, CancellationToken token)
        {
            var openingHours = new List<TimeSlotHour>();
            if (request.OpeningHours == null)
            {
                foreach (var oh in request.OpeningHours)
                {
                    openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                }
            }

            var store = new Store(Guid.NewGuid(), request.Name, request.Owner.FirstName, request.Owner.LastName, request.Email,
                request.Siret, request.VatIdentifier, address, openingHours, request.OpenForNewBusiness, request.Phone, request.Description);

            var billingAddress = request.BillingAddress == null ? address
                : new UserAddress(request.BillingAddress.Line1, request.BillingAddress.Line2, request.BillingAddress.Zipcode, request.BillingAddress.City, request.BillingAddress.Country, null);

            store.SetBillingAddress(billingAddress.Line1, billingAddress.Line2, billingAddress.Zipcode,
                billingAddress.City, billingAddress.Country);

            if (request.Tags != null && request.Tags.Any())
            {
                var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                store.SetTags(tags);
            }

            var picture = await _imageService.HandleUserImageAsync(store.Id, request.Picture, token);
            store.SetPicture(picture);

            return store;
        }

        private async Task<Producer> CreateProducerAsync(RegisterProducerCommand request, UserAddress address, CancellationToken token)
        {
            var producer = new Producer(Guid.NewGuid(), request.Name, request.Owner.FirstName, request.Owner.LastName, request.Email,
                request.Siret, request.VatIdentifier, address, request.OpenForNewBusiness, request.Phone, request.Description);

            var billingAddress = request.BillingAddress == null ? address
                : new UserAddress(request.BillingAddress.Line1, request.BillingAddress.Line2, request.BillingAddress.Zipcode, request.BillingAddress.City, request.BillingAddress.Country, null);

            producer.SetBillingAddress(billingAddress.Line1, billingAddress.Line2, billingAddress.Zipcode,
                billingAddress.City, billingAddress.Country);

            if (request.Tags != null && request.Tags.Any())
            {
                var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                producer.SetTags(tags);
            }

            var picture = await _imageService.HandleUserImageAsync(producer.Id, request.Picture, token);
            producer.SetPicture(picture);

            return producer;
        }
    }
}