using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Job.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Transactions;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Transactions.Commands
{
    public class ExportUserTransactionsCommand : Command
    {
        [JsonConstructor]
        public ExportUserTransactionsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
    }

    public class ExportUserTransactionsCommandHandler : CommandsHandler,
        IRequestHandler<ExportUserTransactionsCommand, Result>
    {
        private readonly IBlobService _blobService;

        public ExportUserTransactionsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<ExportUserTransactionsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result> Handle(ExportUserTransactionsCommand request, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(job.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            try
            {
                var startResult = await _mediatr.Process(new StartJobCommand(request.RequestUser) {JobId = job.Id}, token);
                if (!startResult.Succeeded)
                    throw startResult.Exception;

                _mediatr.Post(new UserTransactionsExportProcessingEvent(job.Id));

                using (var stream = new MemoryStream())
                {
                    using (var package = new ExcelPackage(stream))
                        await CreateExcelUserTransactionsFileAsync(package, job.User, request.From, request.To, token);
                    
                    var response = await _blobService.UploadUserTransactionsFileAsync(job.User.Id, job.Id,
                        $"Virements_{request.From:dd-MM-yyyy}_{request.To:dd-MM-yyyy}.xlsx", stream.ToArray(), token);
                    if (!response.Succeeded)
                        throw response.Exception;

                    _mediatr.Post(new UserTransactionsExportSucceededEvent(job.Id));

                    _logger.LogInformation($"Transactions for user {job.User.Id} successfully exported");
                    return await _mediatr.Process(
                        new CompleteJobCommand(request.RequestUser) {JobId = job.Id, FileUrl = response.Data}, token);
                }
            }
            catch (Exception e)
            {
                _mediatr.Post(new UserTransactionsExportFailedEvent(job.Id));
                return await _mediatr.Process(new FailJobCommand(request.RequestUser) {JobId = job.Id, Reason = e.Message},
                    token);
            }
        }

        private async Task CreateExcelUserTransactionsFileAsync(ExcelPackage package, Domain.User user, DateTimeOffset from, DateTimeOffset to, CancellationToken token)
        {
            package = await CreatePayoutsSheetAsync(package, user, from, to, token);
            package = await CreateWithholdingsSheetAsync(package, user, from, to, token);
            package.Save();
        }

        private async Task<ExcelPackage> CreatePayoutsSheetAsync(ExcelPackage package, Domain.User user, DateTimeOffset from, DateTimeOffset to, CancellationToken token)
        {
            var payoutsWorksheet = package.Workbook.Worksheets.Add("Virements");

            payoutsWorksheet.Cells[1, 1, 1, 8].Value = $"Vos virements du {from:dd-MM-yyyy} au {to:dd-MM-yyyy}";
            payoutsWorksheet.Cells[1, 1, 1, 8].Merge = true;

            payoutsWorksheet.Cells[2, 1, 2, 2].Value = "Information virements";
            payoutsWorksheet.Cells[2, 1, 2, 2].Merge = true;

            payoutsWorksheet.Cells[2, 3, 2, 8].Value = "Commandes relatives";
            payoutsWorksheet.Cells[2, 3, 2, 8].Merge = true;

            payoutsWorksheet.Cells[3, 1].Value = "Date d'éxécution";
            payoutsWorksheet.Cells[3, 2].Value = "Montant du virement (en €)";
            payoutsWorksheet.Cells[3, 3].Value = "Numéro commande";
            payoutsWorksheet.Cells[3, 4].Value = "Nom client";
            payoutsWorksheet.Cells[3, 5].Value = "Email client";
            payoutsWorksheet.Cells[3, 6].Value = "Date de livraison";
            payoutsWorksheet.Cells[3, 7].Value = "Total HT (en €)";
            payoutsWorksheet.Cells[3, 8].Value = "Total TTC (en €)";

            var payouts = await _context.FindAsync<Domain.Payout>(o => 
                o.Author.Id == user.Id 
                && o.Status == TransactionStatus.Succeeded
                && o.ExecutedOn.HasValue && o.ExecutedOn.Value >= from
                && o.ExecutedOn.HasValue && o.ExecutedOn.Value <= to, token);
            var i = 4;
            
            foreach (var payout in payouts)
            {
                payoutsWorksheet.Cells[i, 1].Value = payout.ExecutedOn.Value.ToString("dd/MM/yyyy");
                payoutsWorksheet.Cells[i, 2].Value = payout.Debited;
                payoutsWorksheet.Cells[i, 1, i + payout.Transfers.Count - 1, 1].Merge = true;
                payoutsWorksheet.Cells[i, 2, i + payout.Transfers.Count - 1, 2].Merge = true;
                
                foreach (var transfer in payout.Transfers)
                {
                    payoutsWorksheet.Cells[i, 3].Value = transfer.PurchaseOrder.Reference;
                    payoutsWorksheet.Cells[i, 4].Value = transfer.PurchaseOrder.Sender.Name;
                    payoutsWorksheet.Cells[i, 5].Value = transfer.PurchaseOrder.Sender.Email;
                    payoutsWorksheet.Cells[i, 6].Value = transfer.PurchaseOrder.ExpectedDelivery.ExpectedDeliveryDate;
                    payoutsWorksheet.Cells[i, 7].Value = transfer.PurchaseOrder.TotalWholeSalePrice;
                    payoutsWorksheet.Cells[i, 8].Value = transfer.PurchaseOrder.TotalOnSalePrice;
                    i++;
                }
            }

            return package;
        }

        private async Task<ExcelPackage> CreateWithholdingsSheetAsync(ExcelPackage package, Domain.User user, DateTimeOffset from, DateTimeOffset to, CancellationToken token)
        {
            var withholdingsWorksheet = package.Workbook.Worksheets.Add("Paiements");

            withholdingsWorksheet.Cells[1, 1, 1, 5].Value = $"Vos paiements du {from:dd-MM-yyyy} au {to:dd-MM-yyyy}";
            withholdingsWorksheet.Cells[1, 1, 1, 5].Merge = true;
            
            withholdingsWorksheet.Cells[2, 1].Value = "Nom du destinataire";
            withholdingsWorksheet.Cells[2, 2].Value = "Email du destinataire";
            withholdingsWorksheet.Cells[2, 3].Value = "Date d'éxécution";
            withholdingsWorksheet.Cells[2, 4].Value = "Montant TTC";
            withholdingsWorksheet.Cells[2, 5].Value = "Raison";

            var withholdings = await _context.FindAsync<Domain.Withholding>(o => o.Author.Id == user.Id 
                && o.Status == TransactionStatus.Succeeded
                && o.ExecutedOn.HasValue && o.ExecutedOn.Value >= from
                && o.ExecutedOn.HasValue && o.ExecutedOn.Value <= to, token);
            var i = 3;
            
            foreach (var withholding in withholdings)
            {
                withholdingsWorksheet.Cells[i, 1].Value = withholding.DebitedWallet.User.Name;
                withholdingsWorksheet.Cells[i, 2].Value = withholding.DebitedWallet.User.Email;
                withholdingsWorksheet.Cells[i, 3].Value = withholding.ExecutedOn.Value.ToString("dd/MM/yyyy");
                withholdingsWorksheet.Cells[i, 4].Value = withholding.Debited;
                withholdingsWorksheet.Cells[i, 5].Value = "Validation de vos documents de paiements";
                i++;
            }

            return package;
        }
    }
}