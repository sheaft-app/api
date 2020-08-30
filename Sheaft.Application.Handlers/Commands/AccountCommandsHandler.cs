using Sheaft.Application.Commands;
using Sheaft.Infrastructure.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using System.IO;
using OfficeOpenXml;
using Sheaft.Interop.Enums;
using System.Linq;
using Sheaft.Services.Interop;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{
    public class AccountCommandsHandler : CommandsHandler,
        IRequestHandler<QueueExportAccountDataCommand, CommandResult<Guid>>,
        IRequestHandler<ExportAccountDataCommand, CommandResult<bool>>
    {
        private readonly IMediator _mediatr;
        private readonly IAppDbContext _context;
        private readonly IQueueService _queuesService;
        private readonly IBlobService _blobsService;

        public AccountCommandsHandler(
            IMediator mediatr,
            IAppDbContext context,
            IQueueService queuesService,
            IBlobService blobsService,
            ILogger<AccountCommandsHandler> logger) : base(logger)
        {
            _blobsService = blobsService;
            _queuesService = queuesService;
            _mediatr = mediatr;
            _context = context;
        }

        public async Task<CommandResult<Guid>> Handle(QueueExportAccountDataCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var sender = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);

                var entity = new Job(Guid.NewGuid(), JobKind.ExportAccountData, $"Export RGPD", sender, ExportAccountDataCommand.QUEUE_NAME);
                entity.SetCommand(new ExportAccountDataCommand(request.RequestUser) { Id = entity.Id });

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                await _queuesService.InsertJobToProcessAsync(entity, token);
                Logger.LogInformation($"Account RGPD data export successfully initiated by {request.RequestUser.Id}");

                return Ok(entity.Id);
            });
        }

        public async Task<CommandResult<bool>> Handle(ExportAccountDataCommand request, CancellationToken token)
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

                    await _queuesService.ProcessEventAsync(AccountExportDataProcessingEvent.QUEUE_NAME, new AccountExportDataProcessingEvent(request.RequestUser) { Id = request.Id, JobId = job.Id }, token);

                    using (var stream = new MemoryStream())
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            await CreateExcelAccountDataFileAsync(package, user, token);
                        }

                        var response = await _blobsService.UploadRgpdFileAsync(request.RequestUser.Id, job.Id, $"RGPD_{job.CreatedOn:dd-MM-yyyy}.xlsx", stream, token);
                        if (!response.Success)
                            throw response.Exception;

                        await _queuesService.ProcessEventAsync(AccountExportDataSucceededEvent.QUEUE_NAME, new AccountExportDataSucceededEvent(request.RequestUser) { Id = job.Id, JobId = job.Id }, token);
                        
                        Logger.LogInformation($"RGPD data for account {request.RequestUser.Id} successfully exported");
                        return await _mediatr.Send(new CompleteJobCommand(request.RequestUser) { Id = job.Id, FileUrl = response.Result });
                    }
                }
                catch (Exception e)
                {
                    await _queuesService.ProcessEventAsync(AccountExportDataFailedEvent.QUEUE_NAME, new AccountExportDataFailedEvent(request.RequestUser) { Id = request.Id, JobId = job.Id }, token);
                    return await _mediatr.Send(new FailJobCommand(request.RequestUser) { Id = job.Id, Reason = e.Message }, token);
                }

            });
        }

        private async Task CreateExcelAccountDataFileAsync(ExcelPackage package, User user, CancellationToken token)
        {
            package = ProcessUserProfileData(package, user);
            package = await ProcessUserOrdersDataAsync(package, user, token);
            package = await ProcessUserRatingsDataAsync(package, user, token);
            package = ProcessUserPointsData(package, user);

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

            if (user.Company != null)
            {
                profileWorksheet.Cells[9, 1].Value = "Société";
                profileWorksheet.Cells[9, 2].Value = user.Company.Name;
            }

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

        private ExcelPackage ProcessUserPointsData(ExcelPackage package, User user)
        {
            var pointsWorksheet = package.Workbook.Worksheets.Add("Points");

            pointsWorksheet.Cells[1, 1, 1, 3].Value = "Vos points";
            pointsWorksheet.Cells[1, 1, 1, 3].Merge = true;

            pointsWorksheet.Cells[2, 1].Value = "Date d'acquisition";
            pointsWorksheet.Cells[2, 2].Value = "Raison";
            pointsWorksheet.Cells[2, 3].Value = "Quantité";

            var i = 3;
            foreach (var userRating in user.Points)
            {
                pointsWorksheet.Cells[i, 1].Value = userRating.CreatedOn.ToString("dd/MM/yyyy HH:mm");
                pointsWorksheet.Cells[i, 2].Value = userRating.Kind;
                pointsWorksheet.Cells[i, 3].Value = userRating.Quantity;

                i++;
            }

            return package;
        }
    }
}