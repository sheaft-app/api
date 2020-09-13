using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using MediatR;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Core;
using Sheaft.Exceptions;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Services.Interop;
using Sheaft.Models.Inputs;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class ConsumerCommandsHandler : ResultsHandler,
        IRequestHandler<RegisterConsumerCommand, Result<Guid>>,
        IRequestHandler<UpdateConsumerCommand, Result<bool>>,
        IRequestHandler<SetConsumerLegalsCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IQueueService _queueService;
        private readonly IImageService _imageService;
        private readonly HttpClient _httpClient;
        private readonly RoleOptions _roleOptions;
        private readonly AuthOptions _authOptions;
        private readonly IDistributedCache _cache;

        public ConsumerCommandsHandler(
            IOptionsSnapshot<AuthOptions> authOptions,
            IHttpClientFactory httpClientFactory,
            IAppDbContext context,
            IQueueService queueService,
            IImageService imageService,
            ILogger<ConsumerCommandsHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IDistributedCache cache) : base(logger)
        {
            _imageService = imageService;
            _roleOptions = roleOptions.Value;
            _authOptions = authOptions.Value;
            _context = context;
            _queueService = queueService;

            _httpClient = httpClientFactory.CreateClient("identityServer");
            _httpClient.BaseAddress = new Uri(_authOptions.Url);
            _httpClient.SetToken(_authOptions.Scheme, _authOptions.ApiKey);
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(RegisterConsumerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.FindSingleAsync<Consumer>(r => r.Id == request.Id || r.Email == request.Email, token);
                if (entity != null)
                    return Conflict<Guid>(MessageKind.Register_User_AlreadyExists);

                var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                var address = request.Address != null ?
                    new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country,
                        department, request.Address.Longitude, request.Address.Latitude)
                    : null;

                entity = new Consumer(request.Id, request.Email, request.FirstName, request.LastName, address, request.Phone);
                entity.SetAnonymous(request.Anonymous);

                var resultImage = await _imageService.HandleUserImageAsync(request.Id, request.Picture, token);
                if (!resultImage.Success)
                    return Failed<Guid>(resultImage.Exception);

                entity.SetPicture(resultImage.Data);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                var oidcResult = await UpdateOidcUserAsync(entity, new List<Guid> { _roleOptions.Consumer.Id }, token);
                if (!oidcResult.Success)
                    return Failed<Guid>(new BadRequestException(MessageKind.Oidc_Register_Error, oidcResult.Exception?.Message));

                if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                {
                    var sponsor = await _context.FindSingleAsync<User>(u => u.SponsorshipCode == request.SponsoringCode, token);
                    if (sponsor == null)
                        return NotFound<Guid>(MessageKind.Register_UserWithSponsorCode_NotFound);

                    await _context.AddAsync(new Sponsoring(sponsor, entity), token);
                    await _context.SaveChangesAsync(token);

                    await _queueService.ProcessEventAsync(UserSponsoredEvent.QUEUE_NAME, new UserSponsoredEvent(request.RequestUser) { SponsorId = sponsor.Id, SponsoredId = entity.Id }, token);
                    await _queueService.ProcessCommandAsync(CreateUserPointsCommand.QUEUE_NAME, new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.Sponsoring, UserId = sponsor.Id }, token);
                }

                await _cache.RemoveAsync(entity.Id.ToString("N"));
                return Created(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdateConsumerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Consumer>(request.Id, token);

                entity.SetEmail(request.Email);
                entity.SetPhone(request.Phone);
                entity.SetFirstname(request.FirstName);
                entity.SetLastname(request.LastName);
                entity.SetAnonymous(request.Anonymous);

                var resultImage = await _imageService.HandleUserImageAsync(request.Id, request.Picture, token);
                if (!resultImage.Success)
                    return Failed<bool>(resultImage.Exception);

                entity.SetPicture(resultImage.Data);

                var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                var address = request.Address != null ?
                    new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country,
                        department, request.Address.Longitude, request.Address.Latitude)
                    : null;

                entity.SetAddress(address);

                var oidcResult = await UpdateOidcUserAsync(entity, new List<Guid> { _roleOptions.Consumer.Id }, token);
                if (!oidcResult.Success)
                    return Failed<bool>(new BadRequestException(MessageKind.Oidc_UpdateProfile_Error, oidcResult.Exception?.Message));

                _context.Update(entity);
                var result = await _context.SaveChangesAsync(token);

                await _cache.RemoveAsync(entity.Id.ToString("N"));
                return Ok(result > 0);
            });
        }

        public async Task<Result<bool>> Handle(SetConsumerLegalsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Consumer>(request.Id, token);
                
                _context.Update(entity);

                var success = await _context.SaveChangesAsync(token) > 0;
                return Ok(success);
            });
        }

        private async Task<Result<bool>> UpdateOidcUserAsync(Consumer entity, List<Guid> roles, CancellationToken token)
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
    }
}