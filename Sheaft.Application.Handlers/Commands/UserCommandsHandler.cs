using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using System.IO;
using OfficeOpenXml;
using Sheaft.Domain.Enums;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Sheaft.Exceptions;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using IdentityModel.Client;
using System.Collections.Generic;

namespace Sheaft.Application.Handlers
{
    public class UserCommandsHandler : ResultsHandler,
            IRequestHandler<QueueExportUserDataCommand, Result<Guid>>,
            IRequestHandler<ExportUserDataCommand, Result<bool>>,
            IRequestHandler<GenerateUserCodeCommand, Result<string>>,
            IRequestHandler<CreateUserPointsCommand, Result<bool>>,
            IRequestHandler<ChangeUserRolesCommand, Result<bool>>,
            IRequestHandler<UpdateUserPictureCommand, Result<bool>>,
            IRequestHandler<DeleteUserCommand, Result<bool>>,
            IRequestHandler<RemoveUserDataCommand, Result<string>>
    {
        private readonly IIdentifierService _identifierService;
        private readonly IBlobService _blobService;
        private readonly IImageService _imageService;
        private readonly ScoringOptions _scoringOptions;
        private readonly StorageOptions _storageOptions;
        private readonly HttpClient _httpClient;
        private readonly RoleOptions _roleOptions;
        private readonly AuthOptions _authOptions;
        private readonly IDistributedCache _cache;

        public UserCommandsHandler(
            IOptionsSnapshot<AuthOptions> authOptions,
            IOptionsSnapshot<ScoringOptions> scoringOptions,
            IOptionsSnapshot<StorageOptions> storageOptions,
            IMediator mediatr,
            IHttpClientFactory httpClientFactory,
            IIdentifierService identifierService,
            IAppDbContext context,
            IQueueService queueService,
            IBlobService blobService,
            IImageService imageService,
            ILogger<UserCommandsHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IDistributedCache cache)
            : base(mediatr, context, queueService, logger)
        {
            _imageService = imageService;
            _roleOptions = roleOptions.Value;
            _authOptions = authOptions.Value;
            _scoringOptions = scoringOptions.Value;
            _storageOptions = storageOptions.Value;
            _identifierService = identifierService;
            _blobService = blobService;

            _httpClient = httpClientFactory.CreateClient("identityServer");
            _httpClient.BaseAddress = new Uri(_authOptions.Url);
            _httpClient.SetToken(_authOptions.Scheme, _authOptions.ApiKey);
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(ChangeUserRolesCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.UserId, token);

                var roles = new List<Guid>();

                if (request.Roles.Contains(_roleOptions.Producer.Value))
                {
                    roles.Add(_roleOptions.Producer.Id);
                }

                if (request.Roles.Contains(_roleOptions.Store.Value))
                {
                    roles.Add(_roleOptions.Owner.Id);
                    roles.Add(_roleOptions.Store.Id);
                }

                if (request.Roles.Contains(_roleOptions.Consumer.Value))
                {
                    roles.Add(_roleOptions.Owner.Id);
                    roles.Add(_roleOptions.Consumer.Id);
                }

                var oidcUser = new IdentityUserInput(request.UserId, entity.Email, entity.Name, entity.FirstName, entity.LastName, roles)
                {
                    Phone = entity.Phone,
                    Picture = entity.Picture
                };

                var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile, new StringContent(JsonConvert.SerializeObject(oidcUser), Encoding.UTF8, "application/json"), token);
                if (!oidcResult.IsSuccessStatusCode)
                    return Failed<bool>(new BadRequestException(MessageKind.Oidc_UpdateProfile_Error, await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

                _context.Update(entity);
                await _cache.RemoveAsync(entity.Id.ToString("N"));

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<string>> Handle(GenerateUserCodeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.Id, token);

                if (!string.IsNullOrWhiteSpace(entity.SponsorshipCode))
                    return Ok(entity.SponsorshipCode);

                var result = await _identifierService.GetNextSponsoringCode(token);
                if (!result.Success)
                    return Failed<string>(result.Exception);

                entity.SetSponsoringCode(result.Data);
                _context.Update(entity);

                await _context.SaveChangesAsync(token);
                return Created(entity.SponsorshipCode);
            });
        }
        public async Task<Result<string>> Handle(RemoveUserDataCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.Id, token);
                await _blobService.CleanUserStorageAsync(request.Id, token);

                return Ok(request.Email);
            });
        }

        public async Task<Result<bool>> Handle(UpdateUserPictureCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.Id, token);

                var resultImage = await _imageService.HandleUserImageAsync(request.Id, request.Picture, token);
                if (!resultImage.Success)
                    return Failed<bool>(resultImage.Exception);

                entity.SetPicture(resultImage.Data);

                var oidcUser = new IdentityPictureInput(request.Id, entity.Picture);
                var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Picture, new StringContent(JsonConvert.SerializeObject(oidcUser), Encoding.UTF8, "application/json"), token);
                if (!oidcResult.IsSuccessStatusCode)
                    return BadRequest<bool>(MessageKind.Oidc_UpdatePicture_Error, await oidcResult.Content.ReadAsStringAsync());

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(CreateUserPointsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (!_scoringOptions.Points.TryGetValue(request.Kind.ToString("G"), out int quantity))
                    return BadRequest<bool>(MessageKind.UserPoints_Scoring_Matching_ActionPoints_NotFound);

                var user = await _context.GetByIdAsync<User>(request.UserId, token);
                var point = new Points(user, request.Kind, quantity, request.CreatedOn);

                user.SetTotalPoints(user.TotalPoints + point.Quantity);
                _context.Update(user);

                await _context.AddAsync(point, token);
                await _context.SaveChangesAsync(token);

                await _queueService.ProcessEventAsync(UserPointsCreatedEvent.QUEUE_NAME, new UserPointsCreatedEvent(request.RequestUser) { UserId = user.Id, Kind = request.Kind, Points = quantity, CreatedOn = request.CreatedOn }, token);

                return Ok(true);
            });
        }

        public async Task<Result<Guid>> Handle(QueueExportUserDataCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var sender = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);

                var entity = new Job(Guid.NewGuid(), JobKind.ExportUserData, $"Export RGPD", sender, ExportUserDataCommand.QUEUE_NAME);
                entity.SetCommand(new ExportUserDataCommand(request.RequestUser) { Id = entity.Id });

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                await _queueService.InsertJobToProcessAsync(entity, token);
                _logger.LogInformation($"User RGPD data export successfully initiated by {request.RequestUser.Id}");

                return Ok(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(ExportUserDataCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var job = await _context.GetByIdAsync<Job>(request.Id, token);
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);

                try
                {
                    var startResult = await _mediatr.Send(new StartJobCommand(request.RequestUser) { Id = job.Id });
                    if (!startResult.Success)
                        throw startResult.Exception;

                    await _queueService.ProcessEventAsync(ExportUserDataProcessingEvent.QUEUE_NAME, new ExportUserDataProcessingEvent(request.RequestUser) { Id = request.Id, JobId = job.Id }, token);

                    using (var stream = new MemoryStream())
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            await CreateExcelUserDataFileAsync(package, user, token);
                        }

                        var response = await _blobService.UploadRgpdFileAsync(request.RequestUser.Id, job.Id, $"RGPD_{job.CreatedOn:dd-MM-yyyy}.xlsx", stream, token);
                        if (!response.Success)
                            throw response.Exception;

                        await _queueService.ProcessEventAsync(ExportUserDataSucceededEvent.QUEUE_NAME, new ExportUserDataSucceededEvent(request.RequestUser) { Id = job.Id, JobId = job.Id }, token);

                        _logger.LogInformation($"RGPD data for user {request.RequestUser.Id} successfully exported");
                        return await _mediatr.Send(new CompleteJobCommand(request.RequestUser) { Id = job.Id, FileUrl = response.Data });
                    }
                }
                catch (Exception e)
                {
                    await _queueService.ProcessEventAsync(ExportUserDataFailedEvent.QUEUE_NAME, new ExportUserDataFailedEvent(request.RequestUser) { Id = request.Id, JobId = job.Id }, token);
                    return await _mediatr.Send(new FailJobCommand(request.RequestUser) { Id = job.Id, Reason = e.Message }, token);
                }

            });
        }

        public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var entity = await _context.GetByIdAsync<User>(request.Id, token);

                    var hasActiveOrders = await _context.AnyAsync<PurchaseOrder>(o => (o.Vendor.Id == entity.Id || o.Sender.Id == entity.Id) && (int)o.Status < 6, token);
                    if (hasActiveOrders)
                        return ValidationError<bool>(MessageKind.Consumer_CannotBeDeleted_HasActiveOrders);

                    var oidcResult = await _httpClient.DeleteAsync(string.Format(_authOptions.Actions.Delete, entity.Id.ToString("N")), token);
                    if (!oidcResult.IsSuccessStatusCode)
                        return Failed<bool>(new BadRequestException(MessageKind.Oidc_DeleteProfile_Error, await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

                    var email = entity.Email;

                    entity.Close(request.Reason);
                    _context.Update(entity);

                    var result = await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    await _cache.RemoveAsync(entity.Id.ToString("N"));
                    await _queueService.ProcessCommandAsync(RemoveUserDataCommand.QUEUE_NAME, new RemoveUserDataCommand(request.RequestUser) { Id = request.Id, Email = email }, token);

                    return Ok(result > 0);
                }
            });
        }

        private async Task CreateExcelUserDataFileAsync(ExcelPackage package, User user, CancellationToken token)
        {
            package = ProcessUserProfileData(package, user);
            package = await ProcessUserOrdersDataAsync(package, user, token);
            package = await ProcessUserRatingsDataAsync(package, user, token);

            package.Save();
        }

        private ExcelPackage ProcessUserProfileData(ExcelPackage package, User user)
        {
            var profileWorksheet = package.Workbook.Worksheets.Add("Profil");

            profileWorksheet.Cells[1, 1, 1, 2].Value = "Vos données";
            profileWorksheet.Cells[1, 1, 1, 2].Merge = true;

            profileWorksheet.Cells[2, 1].Value = "Nom";
            profileWorksheet.Cells[2, 2].Value = user.LastName;
            profileWorksheet.Cells[3, 1].Value = "Prénom";
            profileWorksheet.Cells[3, 2].Value = user.FirstName;
            profileWorksheet.Cells[4, 1].Value = "Adresse e-mail";
            profileWorksheet.Cells[4, 2].Value = user.Email;
            profileWorksheet.Cells[5, 1].Value = "Numéro de mobile";
            profileWorksheet.Cells[5, 2].Value = user.Phone;
            profileWorksheet.Cells[6, 1].Value = "Image de profil";
            profileWorksheet.Cells[6, 2].Value = user.Picture;
            profileWorksheet.Cells[7, 1].Value = "Date de création du compte";
            profileWorksheet.Cells[7, 2].Value = user.CreatedOn.ToString("dd/MM/yyyy HH:mm");
            profileWorksheet.Cells[8, 1].Value = "Date de dernière mise à jour";
            profileWorksheet.Cells[8, 2].Value = (user.UpdatedOn.HasValue ? user.UpdatedOn.Value : user.CreatedOn).ToString("dd/MM/yyyy HH:mm");
            profileWorksheet.Cells[9, 1].Value = "Points";
            profileWorksheet.Cells[9, 2].Value = user.TotalPoints;

            return package;
        }

        private async Task<ExcelPackage> ProcessUserOrdersDataAsync(ExcelPackage package, User user, CancellationToken token)
        {
            var ordersWorksheet = package.Workbook.Worksheets.Add("Commandes");

            ordersWorksheet.Cells[1, 1, 1, 9].Value = "Vos commandes";
            ordersWorksheet.Cells[1, 1, 1, 9].Merge = true;

            ordersWorksheet.Cells[2, 1, 2, 6].Value = "Information commande";
            ordersWorksheet.Cells[2, 1, 2, 6].Merge = true;

            ordersWorksheet.Cells[2, 7, 2, 9].Value = "Détails produits";
            ordersWorksheet.Cells[2, 7, 2, 9].Merge = true;

            ordersWorksheet.Cells[3, 1].Value = "Numéro de commande";
            ordersWorksheet.Cells[3, 2].Value = "Destinataire";
            ordersWorksheet.Cells[3, 3].Value = "Statut";
            ordersWorksheet.Cells[3, 4].Value = "Date de création";
            ordersWorksheet.Cells[3, 5].Value = "Date de livraison";
            ordersWorksheet.Cells[3, 6].Value = "Total TTC";
            ordersWorksheet.Cells[3, 7].Value = "Nom du produit";
            ordersWorksheet.Cells[3, 8].Value = "Quantité";
            ordersWorksheet.Cells[3, 9].Value = "Prix TTC";

            var orders = await _context.FindAsync<PurchaseOrder>(o => o.Sender.Id == user.Id, token);
            var i = 4;
            foreach (var order in orders)
            {
                foreach (var product in order.Products)
                {
                    ordersWorksheet.Cells[i, 1].Value = order.Reference;
                    ordersWorksheet.Cells[i, 2].Value = order.Vendor.Name;
                    ordersWorksheet.Cells[i, 3].Value = order.Status.ToString("G");
                    ordersWorksheet.Cells[i, 4].Value = order.CreatedOn.ToString("dd/MM/yyyy");
                    ordersWorksheet.Cells[i, 5].Value = order.ExpectedDelivery.ExpectedDeliveryDate.ToString("dd/MM/yyyy");
                    ordersWorksheet.Cells[i, 6].Value = order.TotalOnSalePrice;

                    ordersWorksheet.Cells[i, 7].Value = product.Name;
                    ordersWorksheet.Cells[i, 8].Value = product.Quantity;
                    ordersWorksheet.Cells[i, 9].Value = product.TotalOnSalePrice;
                    i++;
                }
            }

            return package;
        }

        private async Task<ExcelPackage> ProcessUserRatingsDataAsync(ExcelPackage package, User user, CancellationToken token)
        {
            var ratingsWorksheet = package.Workbook.Worksheets.Add("Avis");

            ratingsWorksheet.Cells[1, 1, 1, 5].Value = "Vos avis";
            ratingsWorksheet.Cells[1, 1, 1, 5].Merge = true;

            ratingsWorksheet.Cells[2, 1].Value = "Nom du produit";
            ratingsWorksheet.Cells[2, 2].Value = "Producteur";
            ratingsWorksheet.Cells[2, 3].Value = "Date de création";
            ratingsWorksheet.Cells[2, 4].Value = "Note";
            ratingsWorksheet.Cells[2, 5].Value = "Commentaire";

            var productsRated = await _context.FindAsync<Product>(o => o.Ratings.Any(r => r.User.Id == user.Id), token);
            var i = 3;
            foreach (var product in productsRated)
            {
                foreach (var rating in product.Ratings.Where(r => r.User.Id == user.Id))
                {
                    ratingsWorksheet.Cells[i, 1].Value = product.Name;
                    ratingsWorksheet.Cells[i, 2].Value = product.Producer.Name;
                    ratingsWorksheet.Cells[i, 3].Value = rating.CreatedOn.ToString("G");
                    ratingsWorksheet.Cells[i, 4].Value = rating.Value;
                    ratingsWorksheet.Cells[i, 5].Value = rating.Comment;

                    i++;
                }
            }

            return package;
        }
    }
}