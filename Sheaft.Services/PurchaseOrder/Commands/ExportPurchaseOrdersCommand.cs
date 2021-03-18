using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Services.Job.Commands;

namespace Sheaft.Services.PurchaseOrder.Commands
{
    public class ExportPurchaseOrdersCommand : Command
    {
        [JsonConstructor]
        public ExportPurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
    }

    public class ExportPurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<ExportPurchaseOrdersCommand, Result>
    {
        private readonly IBlobService _blobService;

        public ExportPurchaseOrdersCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<ExportPurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result> Handle(ExportPurchaseOrdersCommand request, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(job.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            try
            {
                var startResult = await _mediatr.Process(new StartJobCommand(request.RequestUser) {JobId = job.Id}, token);
                if (!startResult.Succeeded)
                    throw startResult.Exception;

                _mediatr.Post(new PurchaseOrdersExportProcessingEvent(job.Id));

                using (var stream = new MemoryStream())
                {
                    using (var package = new ExcelPackage(stream))
                        await CreateExcelPurchaseOrdersFileAsync(package, job.User, request.From, request.To, token);
                    
                    var response = await _blobService.UploadUserPurchaseOrdersFileAsync(job.User.Id, job.Id,
                        $"Commandes{request.From:dd-MM-yyyy}_{request.To:dd-MM-yyyy}.xlsx", stream.ToArray(), token);
                    if (!response.Succeeded)
                        throw response.Exception;

                    _mediatr.Post(new PurchaseOrdersExportSucceededEvent(job.Id));

                    _logger.LogInformation($"PurchaseOrders for user {job.User.Id} successfully exported");
                    return await _mediatr.Process(
                        new CompleteJobCommand(request.RequestUser) {JobId = job.Id, FileUrl = response.Data}, token);
                }
            }
            catch (Exception e)
            {
                _mediatr.Post(new PurchaseOrdersExportFailedEvent(job.Id));
                return await _mediatr.Process(new FailJobCommand(request.RequestUser) {JobId = job.Id, Reason = e.Message},
                    token);
            }
        }

        private async Task CreateExcelPurchaseOrdersFileAsync(ExcelPackage package, Domain.User user, DateTimeOffset from, DateTimeOffset to, CancellationToken token)
        {
            var purchaseOrdersWorksheet = package.Workbook.Worksheets.Add("Commandes");

            purchaseOrdersWorksheet.Cells[1, 1, 1, 20].Value = $"Vos commandes du {from:dd-MM-yyyy} au {to:dd-MM-yyyy}";
            purchaseOrdersWorksheet.Cells[1, 1, 1, 20].Merge = true;

            purchaseOrdersWorksheet.Cells[2, 1, 2, 12].Value = "Information des commandes";
            purchaseOrdersWorksheet.Cells[2, 1, 2, 12].Merge = true;

            purchaseOrdersWorksheet.Cells[2, 13, 2, 20].Value = "Détails des produits";
            purchaseOrdersWorksheet.Cells[2, 13, 2, 20].Merge = true;

            purchaseOrdersWorksheet.Cells[3, 1].Value = "Date de commande";
            purchaseOrdersWorksheet.Cells[3, 2].Value = "Nom client";
            purchaseOrdersWorksheet.Cells[3, 3].Value = "Email";
            purchaseOrdersWorksheet.Cells[3, 4].Value = "Téléphone";
            purchaseOrdersWorksheet.Cells[3, 5].Value = "Adresse";
            purchaseOrdersWorksheet.Cells[3, 6].Value = "Référence commande";
            purchaseOrdersWorksheet.Cells[3, 7].Value = "Total commande HT";
            purchaseOrdersWorksheet.Cells[3, 8].Value = "Total commande TVA";
            purchaseOrdersWorksheet.Cells[3, 9].Value = "Total commande TTC";
            purchaseOrdersWorksheet.Cells[3, 10].Value = "Date de livraison";
            purchaseOrdersWorksheet.Cells[3, 11].Value = "Référence du produit";
            purchaseOrdersWorksheet.Cells[3, 12].Value = "Nom du produit";
            purchaseOrdersWorksheet.Cells[3, 13].Value = "Quantité";
            purchaseOrdersWorksheet.Cells[3, 14].Value = "Prix unitaire (HT)";
            purchaseOrdersWorksheet.Cells[3, 15].Value = "Prix unitaire (TTC)";
            purchaseOrdersWorksheet.Cells[3, 16].Value = "TVA";
            purchaseOrdersWorksheet.Cells[3, 17].Value = "Total produit HT";
            purchaseOrdersWorksheet.Cells[3, 18].Value = "Total produit TTC";

            var purchaseOrders = await _context.FindAsync<Domain.PurchaseOrder>(o => 
                o.Vendor.Id == user.Id 
                && o.CreatedOn >= from
                && o.CreatedOn <= to, token);
            var i = 4;
            
            foreach (var purchaseOrder in purchaseOrders)
            {
                purchaseOrdersWorksheet.Cells[i, 1].Value = purchaseOrder.CreatedOn.ToString("dd/MM/yyyy");
                purchaseOrdersWorksheet.Cells[i, 2].Value = purchaseOrder.Sender.Name;
                purchaseOrdersWorksheet.Cells[i, 3].Value = purchaseOrder.Sender.Email;
                purchaseOrdersWorksheet.Cells[i, 4].Value = purchaseOrder.Sender.Phone;
                purchaseOrdersWorksheet.Cells[i, 5].Value = purchaseOrder.Sender.Address;
                purchaseOrdersWorksheet.Cells[i, 6].Value = purchaseOrder.Reference;
                purchaseOrdersWorksheet.Cells[i, 7].Value = purchaseOrder.TotalWholeSalePrice;
                purchaseOrdersWorksheet.Cells[i, 8].Value = purchaseOrder.TotalVatPrice;
                purchaseOrdersWorksheet.Cells[i, 9].Value = purchaseOrder.TotalOnSalePrice;
                purchaseOrdersWorksheet.Cells[i, 10].Value = purchaseOrder.DeliveredOn.HasValue
                    ? "En attente"
                    : purchaseOrder.DeliveredOn.Value.ToString("dd/MM/yyyy");
                
                purchaseOrdersWorksheet.Cells[i, 1, i + purchaseOrder.Products.Count - 1, 1].Merge = true;
                purchaseOrdersWorksheet.Cells[i, 2, i + purchaseOrder.Products.Count - 1, 2].Merge = true;
                
                foreach (var product in purchaseOrder.Products)
                {
                    purchaseOrdersWorksheet.Cells[i, 11].Value = product.Reference;
                    purchaseOrdersWorksheet.Cells[i, 12].Value = product.Name;
                    purchaseOrdersWorksheet.Cells[i, 13].Value = product.Quantity;
                    purchaseOrdersWorksheet.Cells[i, 14].Value = product.UnitWholeSalePrice;
                    purchaseOrdersWorksheet.Cells[i, 15].Value = product.UnitOnSalePrice;
                    purchaseOrdersWorksheet.Cells[i, 16].Value = product.Vat;
                    purchaseOrdersWorksheet.Cells[i, 17].Value = product.TotalWholeSalePrice;
                    purchaseOrdersWorksheet.Cells[i, 18].Value = product.TotalOnSalePrice;
                    i++;
                }
            }

            package.Save();
        }
    }
}