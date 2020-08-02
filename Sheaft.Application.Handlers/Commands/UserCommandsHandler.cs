using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using Sheaft.Models.Inputs;
using Sheaft.Core.Security;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{
    public class UserCommandsHandler : CommandsHandler,
        IRequestHandler<RegisterOwnerCommand, CommandResult<Guid>>,
        IRequestHandler<RegisterConsumerCommand, CommandResult<Guid>>,
        IRequestHandler<UpdateUserCommand, CommandResult<bool>>,
        IRequestHandler<UpdateUserPictureCommand, CommandResult<bool>>,
        IRequestHandler<DeleteUsersCommand, CommandResult<bool>>,
        IRequestHandler<DeleteUserCommand, CommandResult<bool>>,
        IRequestHandler<RemoveUserDataCommand, CommandResult<string>>,
        IRequestHandler<GenerateUserSponsoringCodeCommand, CommandResult<string>>,
        IRequestHandler<CreateUserPointsCommand, CommandResult<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediatr;
        private readonly IIdentifierService _identifierService;
        private readonly IQueueService _queuesService;
        private readonly IBlobService _blobsService;
        private readonly AuthOptions _authOptions;
        private readonly ScoringOptions _scoringOptions;
        private readonly HttpClient _httpClient;

        public UserCommandsHandler(
            IOptionsSnapshot<AuthOptions> authOptions,
            IOptionsSnapshot<ScoringOptions> scoringOptions,
            IMediator mediatr,
            IHttpClientFactory httpClientFactory,
            IIdentifierService identifierService,
            IAppDbContext context,
            IQueueService queuesService,
            IBlobService blobsService,
            ILogger<UserCommandsHandler> logger) : base(logger)
        {
            _authOptions = authOptions.Value;
            _scoringOptions = scoringOptions.Value;
            _context = context;
            _mediatr = mediatr;
            _identifierService = identifierService;
            _queuesService = queuesService;
            _blobsService = blobsService;

            _httpClient = httpClientFactory.CreateClient("identityServer");
            _httpClient.BaseAddress = new Uri(_authOptions.Url);
            _httpClient.SetToken(_authOptions.Scheme, _authOptions.ApiKey);
        }

        public async Task<CommandResult<Guid>> Handle(RegisterOwnerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.FindByIdAsync<User>(request.Id, token);
                if (entity != null)
                    return ConflictResult<Guid>(MessageKind.RegisterOwner_User_AlreadyExists);

                if (request.Roles.Contains(RoleIds.PRODUCER) && request.Roles.Contains(RoleIds.STORE))
                    return BadRequestResult<Guid>(MessageKind.RegisterOwner_User_CannotBe_ProducerAndStoreRoles);

                var roles = request.Roles.ToList();
                if (!request.Roles.Contains(RoleIds.OWNER))
                {
                    roles.Add(RoleIds.OWNER);
                }

                var oidcUser = new IdentityUserInput(request.Id, request.Email, request.FirstName, request.LastName, roles.Select(Guid.Parse))
                {
                    Phone = request.Phone,
                    Picture = request.Picture
                };

                var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile, new StringContent(JsonConvert.SerializeObject(oidcUser), Encoding.UTF8, "application/json"), token);
                if (!oidcResult.IsSuccessStatusCode)
                    return CommandFailed<Guid>(new BadRequestException(MessageKind.RegisterOwner_Oidc_Register_Error, await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

                var company = await _context.GetByIdAsync<Company>(request.CompanyId, token);

                var user = new User(request.Id, UserKind.Owner, request.Email, request.FirstName, request.LastName, request.Phone, company);
                user.SetPicture(request.Picture);

                await _context.AddAsync(user, token);
                await _context.SaveChangesAsync(token);

                if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                {
                    var sponsor = await _context.FindSingleAsync<User>(u => u.Code == request.SponsoringCode, token);
                    if (sponsor == null)
                        return NotFoundResult<Guid>(MessageKind.RegisterOwner_UserWithSponsorCode_NotFound);

                    await _context.AddAsync(new Sponsoring(sponsor, user), token);
                    await _context.SaveChangesAsync(token);

                    await _queuesService.ProcessEventAsync(UserSponsoredEvent.QUEUE_NAME, new UserSponsoredEvent(request.RequestUser) { SponsorId = sponsor.Id, SponsoredId = user.Id }, token);
                    await _queuesService.ProcessCommandAsync(CreateUserPointsCommand.QUEUE_NAME, new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.Sponsoring, UserId = sponsor.Id }, token);
                }

                return CreatedResult(user.Id);
            });
        }

        public async Task<CommandResult<Guid>> Handle(RegisterConsumerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.FindByIdAsync<User>(request.Id, token);
                if (entity != null)
                    return ConflictResult<Guid>(MessageKind.RegisterConsumer_User_AlreadyExists);

                var oidcUser = new IdentityUserInput(request.Id, request.Email, request.FirstName, request.LastName, new List<Guid> { Guid.Parse(RoleIds.CONSUMER) }) 
                { 
                    Phone = request.Phone, 
                    Picture = request.Picture 
                };

                var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile, new StringContent(JsonConvert.SerializeObject(oidcUser), Encoding.UTF8, "application/json"), token);
                if (!oidcResult.IsSuccessStatusCode)
                    return CommandFailed<Guid>(new BadRequestException(MessageKind.RegisterConsumer_Oidc_Register_Error, await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

                var user = new User(request.Id, UserKind.Consumer, request.Email, request.FirstName, request.LastName, request.Phone);
                user.SetPicture(request.Picture);
                user.SetAnonymous(request.Anonymous);

                var dept = await _context.GetByIdAsync<Department>(request.DepartmentId, token);
                user.SetDepartment(dept);

                await _context.AddAsync(user, token);
                await _context.SaveChangesAsync(token);

                if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                {
                    var sponsor = await _context.FindSingleAsync<User>(u => u.Code == request.SponsoringCode, token);
                    if (sponsor == null)
                        return NotFoundResult<Guid>(MessageKind.RegisterConsumer_UserWithSponsorCode_NotFound);

                    await _context.AddAsync(new Sponsoring(sponsor, user), token);
                    await _context.SaveChangesAsync(token);

                    await _queuesService.ProcessEventAsync(UserSponsoredEvent.QUEUE_NAME, new UserSponsoredEvent(request.RequestUser) { SponsorId = sponsor.Id, SponsoredId = user.Id }, token);
                    await _queuesService.ProcessCommandAsync(CreateUserPointsCommand.QUEUE_NAME, new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.Sponsoring, UserId = sponsor.Id }, token);
                }

                return CreatedResult(user.Id);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteUsersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var id in request.Ids)
                    {
                        var result = await _mediatr.Send(new DeleteUserCommand(request.RequestUser) { Id = id, Reason = request.Reason }, token);
                        if (!result.Success)
                            return CommandFailed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return OkResult(true);
                }
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateUserCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.Id, token);

                entity.SetEmail(request.Email);
                entity.SetPhone(request.Phone);
                entity.SetFirstname(request.FirstName);
                entity.SetLastname(request.LastName);
                entity.SetAnonymous(request.Anonymous);

                if (request.DepartmentId.HasValue && request.DepartmentId != entity.Department?.Id)
                {
                    var dept = await _context.GetByIdAsync<Department>(request.DepartmentId.Value, token);
                    entity.SetDepartment(dept);
                }

                var oidcUser = new IdentityUserInput(request.Id, request.Email, request.FirstName, request.LastName)
                {
                    Phone = request.Phone,
                    Picture = request.Picture
                };

                var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile, new StringContent(JsonConvert.SerializeObject(oidcUser), Encoding.UTF8, "application/json"), token);
                if (!oidcResult.IsSuccessStatusCode)
                    return CommandFailed<bool>(new BadRequestException(MessageKind.UpdateUser_Oidc_UpdateProfile_Error, await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

                var entry = _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<string>> Handle(GenerateUserSponsoringCodeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.Id, token);

                if (!string.IsNullOrWhiteSpace(entity.Code))
                    return OkResult(entity.Code);

                var result = await _identifierService.GetNextSponsoringCode(token);
                if (!result.Success)
                    return CommandFailed<string>(result.Exception);

                entity.SetSponsoringCode(result.Result);
                _context.Update(entity);

                await _context.SaveChangesAsync(token);
                return CreatedResult(entity.Code);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteUserCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var entity = await _context.GetByIdAsync<User>(request.Id, token);

                    var hasActiveOrders = await _context.AnyAsync<PurchaseOrder>(o => o.Sender.Id == entity.Id && (int)o.Status < 6, token);
                    if (hasActiveOrders)
                        return ValidationResult<bool>(MessageKind.DeleteUser_CannotBeDeleted_HasActiveOrders);

                    if (entity.UserType == UserKind.Owner && entity.Company != null && !entity.Company.RemovedOn.HasValue)
                    {
                        var hasOrders = await _context.AnyAsync<PurchaseOrder>(o => o.Vendor.Id == entity.Company.Id && (int)o.Status < 6, token);
                        if (hasOrders)
                            return ValidationResult<bool>(MessageKind.DeleteUser_CannotBeDeleted_CompanyHasActiveOrders);

                        var result = await _mediatr.Send(new DeleteCompanyCommand(request.RequestUser) { Id = entity.Company.Id, Reason = request.Reason });
                        if (!result.Success)
                            return CommandFailed<bool>(result.Exception);
                    }

                    var oidcResult = await _httpClient.DeleteAsync(string.Format(_authOptions.Actions.Delete, entity.Id), token);
                    if (!oidcResult.IsSuccessStatusCode)
                        return CommandFailed<bool>(new BadRequestException(MessageKind.DeleteUser_Oidc_DeleteProfile_Error, await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

                    var email = entity.Email;

                    entity.CloseAccount(request.Reason);
                    _context.Update(entity);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    await _queuesService.ProcessCommandAsync(RemoveUserDataCommand.QUEUE_NAME, new RemoveUserDataCommand(request.RequestUser) { Id = request.Id, Email = email }, token);
                    return OkResult(true);
                }
            });
        }

        public async Task<CommandResult<string>> Handle(RemoveUserDataCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.Id, token);
                await _blobsService.CleanUserStorageAsync(request.Id, token);

                return OkResult(request.Email);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateUserPictureCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.Id, token);

                var base64Data = Regex.Match(request.Picture, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                var bytes = Convert.FromBase64String(base64Data);

                using (Image image = Image.Load(bytes))
                {
                    using (var blobStream = new MemoryStream())
                    {
                        image.Clone(context => context.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(64, 64)
                        })).Save(blobStream, new JpegEncoder { Quality = 100 });

                        var compImage = await _blobsService.UploadUserPictureAsync(entity.Id, blobStream, token);
                        if (!compImage.Success)
                            return CommandFailed<bool>(compImage.Exception);

                        entity.SetPicture(compImage.Result);
                    }
                }

                var oidcUser = new UpdatePictureInput { Id = request.Id, Picture = entity.Picture };

                var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Picture, new StringContent(JsonConvert.SerializeObject(oidcUser), Encoding.UTF8, "application/json"), token);
                if (!oidcResult.IsSuccessStatusCode)
                    return CommandFailed<bool>(new BadRequestException(MessageKind.UpdateUser_Oidc_UpdatePicture_Error, await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(CreateUserPointsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (!_scoringOptions.Points.TryGetValue(request.Kind.ToString("G"), out int quantity))
                    return BadRequestResult<bool>(MessageKind.UserPoints_Scoring_Matching_ActionPoints_NotFound);

                var user = await _context.GetByIdAsync<User>(request.UserId, token);
                user.AddPoints(request.Kind, quantity, request.CreatedOn);
                await _context.SaveChangesAsync(token);

                await _queuesService.ProcessEventAsync(UserPointsCreatedEvent.QUEUE_NAME, new UserPointsCreatedEvent(request.RequestUser) { UserId = user.Id, Kind = request.Kind, Points = quantity, CreatedOn = request.CreatedOn }, token);

                return OkResult(true);
            });
        }
    }
}