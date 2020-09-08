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

namespace Sheaft.Application.Handlers
{
    public class ConsumerCommandsHandler : CommandsHandler,
        IRequestHandler<CreateConsumerCommand, CommandResult<Guid>>,
        IRequestHandler<UpdateConsumerCommand, CommandResult<bool>>,
        IRequestHandler<DeleteConsumerCommand, CommandResult<bool>>
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

        public async Task<CommandResult<Guid>> Handle(CreateConsumerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.FindSingleAsync<Consumer>(r => r.Id == request.Id || r.Email == request.Email, token);
                if (entity != null)
                    return Conflict<Guid>(MessageKind.Register_User_AlreadyExists);

                entity = new Consumer(request.Id, request.Email, request.FirstName, request.LastName, request.Phone);
                entity.SetAnonymous(request.Anonymous);

                var picture = await _imageService.HandleUserImageAsync(request.Id, request.Picture, token);
                entity.SetPicture(picture);

                if (request.DepartmentId.HasValue)
                {
                    var department = await _context.GetSingleAsync<Department>(d => d.Id == request.DepartmentId, token);
                    entity.SetAddress(department);
                }
                else if (request.Address != null && !string.IsNullOrWhiteSpace(request.Address.Zipcode))
                {
                    var departmentCode = Address.GetDepartmentCode(request.Address.Zipcode);
                    var department = await _context.GetSingleAsync<Department>(d => d.Code == departmentCode, token);
                    entity.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, department, request.Address.Longitude, request.Address.Latitude);
                }

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                var oidcUser = new IdentityUserInput(request.Id, entity.Email, entity.Name, entity.FirstName, entity.LastName, new List<Guid> { _roleOptions.Consumer.Id })
                {
                    Phone = request.Phone,
                    Picture = picture
                };

                var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile, new StringContent(JsonConvert.SerializeObject(oidcUser), Encoding.UTF8, "application/json"), token);
                if (!oidcResult.IsSuccessStatusCode)
                    return Failed<Guid>(new BadRequestException(MessageKind.Oidc_Register_Error, await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

                if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                {
                    var sponsor = await _context.FindSingleAsync<User>(u => u.Code == request.SponsoringCode, token);
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


        public async Task<CommandResult<bool>> Handle(UpdateConsumerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Consumer>(request.Id, token);

                entity.SetEmail(request.Email);
                entity.SetPhone(request.Phone);
                entity.SetFirstname(request.FirstName);
                entity.SetLastname(request.LastName);
                entity.SetAnonymous(request.Anonymous);

                var picture = await _imageService.HandleUserImageAsync(request.Id, request.Picture, token);
                entity.SetPicture(picture);

                if (request.DepartmentId.HasValue)
                {
                    var department = await _context.GetSingleAsync<Department>(d => d.Id == request.DepartmentId, token);
                    entity.SetAddress(department);
                }
                else if(request.Address != null && !string.IsNullOrWhiteSpace(request.Address.Zipcode))
                {
                    var departmentCode = Address.GetDepartmentCode(request.Address.Zipcode);
                    var department = await _context.GetSingleAsync<Department>(d => d.Code == departmentCode, token);
                    entity.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, department, request.Address.Longitude, request.Address.Latitude);
                }

                var oidcUser = new IdentityUserInput(request.Id, entity.Email, entity.Name, entity.FirstName, entity.LastName)
                {
                    Phone = request.Phone,
                    Picture = entity.Picture
                };

                var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile, new StringContent(JsonConvert.SerializeObject(oidcUser), Encoding.UTF8, "application/json"), token);
                if (!oidcResult.IsSuccessStatusCode)
                    return BadRequest<bool>(MessageKind.Oidc_UpdateProfile_Error, await oidcResult.Content.ReadAsStringAsync());

                _context.Update(entity);
                await _cache.RemoveAsync(entity.Id.ToString("N"));

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteConsumerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var entity = await _context.GetByIdAsync<Consumer>(request.Id, token);

                    var hasActiveOrders = await _context.AnyAsync<PurchaseOrder>(o => o.Sender.Id == entity.Id && (int)o.Status < 6, token);
                    if (hasActiveOrders)
                        return ValidationError<bool>(MessageKind.Consumer_CannotBeDeleted_HasActiveOrders);

                    var oidcResult = await _httpClient.DeleteAsync(string.Format(_authOptions.Actions.Delete, entity.Id.ToString("N")), token);
                    if (!oidcResult.IsSuccessStatusCode)
                        return Failed<bool>(new BadRequestException(MessageKind.Oidc_DeleteProfile_Error, await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

                    var email = entity.Email;

                    entity.Close(request.Reason);
                    _context.Update(entity);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    await _cache.RemoveAsync(entity.Id.ToString("N"));
                    await _queueService.ProcessCommandAsync(RemoveUserDataCommand.QUEUE_NAME, new RemoveUserDataCommand(request.RequestUser) { Id = request.Id, Email = email }, token);
                    return Ok(true);
                }
            });
        }
    }
}