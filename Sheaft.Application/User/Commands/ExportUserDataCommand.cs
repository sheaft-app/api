using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OfficeOpenXml;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Application.Job.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Events.User;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.User.Commands
{
    public class ExportUserDataCommand : Command
    {
        [JsonConstructor]
        public ExportUserDataCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }

    public class ExportUserDataCommandHandler : CommandsHandler,
        IRequestHandler<ExportUserDataCommand, Result>
    {
        private readonly IBlobService _blobService;

        public ExportUserDataCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<ExportUserDataCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result> Handle(ExportUserDataCommand request, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(job.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            try
            {
                var startResult = await _mediatr.Process(new StartJobCommand(request.RequestUser) {JobId = job.Id}, token);
                if (!startResult.Succeeded)
                    throw startResult.Exception;

                _mediatr.Post(new UserDataExportProcessingEvent(job.Id));

                using (var stream = new MemoryStream())
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        await CreateExcelUserDataFileAsync(package, job.User, token);
                    }

                    var response = await _blobService.UploadRgpdFileAsync(job.User.Id, job.Id,
                        $"RGPD_{job.CreatedOn:dd-MM-yyyy}.xlsx", stream.ToArray(), token);
                    if (!response.Succeeded)
                        throw response.Exception;

                    _mediatr.Post(new UserDataExportSucceededEvent(job.Id));

                    _logger.LogInformation($"RGPD data for user {job.User.Id} successfully exported");
                    return await _mediatr.Process(
                        new CompleteJobCommand(request.RequestUser) {JobId = job.Id, FileUrl = response.Data}, token);
                }
            }
            catch (Exception e)
            {
                _mediatr.Post(new UserDataExportFailedEvent(job.Id));
                return await _mediatr.Process(new FailJobCommand(request.RequestUser) {JobId = job.Id, Reason = e.Message},
                    token);
            }
        }

        private async Task CreateExcelUserDataFileAsync(ExcelPackage package, Domain.User user, CancellationToken token)
        {
            package = ProcessUserProfileData(package, user);
            package = await ProcessUserOrdersDataAsync(package, user, token);
            package = await ProcessUserRatingsDataAsync(package, user, token);

            package.Save();
        }

        private ExcelPackage ProcessUserProfileData(ExcelPackage package, Domain.User user)
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
            profileWorksheet.Cells[8, 2].Value =
                (user.UpdatedOn.HasValue ? user.UpdatedOn.Value : user.CreatedOn).ToString("dd/MM/yyyy HH:mm");
            profileWorksheet.Cells[9, 1].Value = "Points";
            profileWorksheet.Cells[9, 2].Value = user.TotalPoints;

            return package;
        }

        private async Task<ExcelPackage> ProcessUserOrdersDataAsync(ExcelPackage package, Domain.User user,
            CancellationToken token)
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

            var orders = await _context.FindAsync<Domain.PurchaseOrder>(o => o.Sender.Id == user.Id, token);
            var i = 4;
            foreach (var order in orders)
            {
                foreach (var product in order.Products)
                {
                    ordersWorksheet.Cells[i, 1].Value = order.Reference;
                    ordersWorksheet.Cells[i, 2].Value = order.Vendor.Name;
                    ordersWorksheet.Cells[i, 3].Value = order.Status.ToString("G");
                    ordersWorksheet.Cells[i, 4].Value = order.CreatedOn.ToString("dd/MM/yyyy");
                    ordersWorksheet.Cells[i, 5].Value =
                        order.ExpectedDelivery.ExpectedDeliveryDate.ToString("dd/MM/yyyy");
                    ordersWorksheet.Cells[i, 6].Value = order.TotalOnSalePrice;

                    ordersWorksheet.Cells[i, 7].Value = product.Name;
                    ordersWorksheet.Cells[i, 8].Value = product.Quantity;
                    ordersWorksheet.Cells[i, 9].Value = product.TotalOnSalePrice;
                    i++;
                }
            }

            return package;
        }

        private async Task<ExcelPackage> ProcessUserRatingsDataAsync(ExcelPackage package, Domain.User user,
            CancellationToken token)
        {
            var ratingsWorksheet = package.Workbook.Worksheets.Add("Avis");

            ratingsWorksheet.Cells[1, 1, 1, 5].Value = "Vos avis";
            ratingsWorksheet.Cells[1, 1, 1, 5].Merge = true;

            ratingsWorksheet.Cells[2, 1].Value = "Nom du produit";
            ratingsWorksheet.Cells[2, 2].Value = "Producteur";
            ratingsWorksheet.Cells[2, 3].Value = "Date de création";
            ratingsWorksheet.Cells[2, 4].Value = "Note";
            ratingsWorksheet.Cells[2, 5].Value = "Commentaire";

            var productsRated =
                await _context.FindAsync<Domain.Product>(o => o.Ratings.Any(r => r.User.Id == user.Id), token);
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