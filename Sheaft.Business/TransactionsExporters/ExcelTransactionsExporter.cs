using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Models;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Business.TransactionsExporters
{
    public class ExcelTransactionsExporter : SheaftService, ITransactionsFileExporter
    {
        public ExcelTransactionsExporter(ILogger<ExcelTransactionsExporter> logger) : base(logger)
        {
        }

        public async Task<Result<ResourceExportDto>> ExportAsync(RequestUser user, DateTimeOffset from,
            DateTimeOffset to, IQueryable<Transaction> transactionsQuery, CancellationToken token)
        {
            using (var stream = new MemoryStream())
            {
                using (var package = new ExcelPackage(stream))
                {
                    var payouts = await transactionsQuery.OfType<Payout>().ToListAsync(token);
                    var withholdings = await transactionsQuery.OfType<Withholding>().ToListAsync(token);
                    
                    var data = CreateExcelUserTransactionsFileAsync(package, user, from, to, payouts, withholdings,
                        token);

                    return Success(new ResourceExportDto
                        {Data = data, Extension = "xlsx", MimeType = "application/ms-excel"});
                }
            }
        }

        private byte[] CreateExcelUserTransactionsFileAsync(ExcelPackage package, RequestUser user, DateTimeOffset from,
            DateTimeOffset to, IEnumerable<Payout> payouts, IEnumerable<Withholding> withholdings,
            CancellationToken token)
        {
            InitializePayoutsSheet(package, user, from, to, payouts, token);
            InitializeWithholdingsSheet(package, user, from, to, withholdings, token);

            return package.GetAsByteArray();
        }

        private void InitializePayoutsSheet(ExcelPackage package, RequestUser user, DateTimeOffset from,
            DateTimeOffset to, IEnumerable<Payout> payouts, CancellationToken token)
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
                    payoutsWorksheet.Cells[i, 4].Value = transfer.PurchaseOrder.SenderInfo.Name;
                    payoutsWorksheet.Cells[i, 5].Value = transfer.PurchaseOrder.SenderInfo.Email;
                    payoutsWorksheet.Cells[i, 6].Value = transfer.PurchaseOrder.ExpectedDelivery.ExpectedDeliveryDate;
                    payoutsWorksheet.Cells[i, 7].Value = transfer.PurchaseOrder.TotalWholeSalePrice;
                    payoutsWorksheet.Cells[i, 8].Value = transfer.PurchaseOrder.TotalOnSalePrice;
                    i++;
                }
            }
        }

        private void InitializeWithholdingsSheet(ExcelPackage package, RequestUser user, DateTimeOffset from,
            DateTimeOffset to, IEnumerable<Withholding> withholdings, CancellationToken token)
        {
            var withholdingsWorksheet = package.Workbook.Worksheets.Add("Paiements");

            withholdingsWorksheet.Cells[1, 1, 1, 5].Value = $"Vos paiements du {from:dd-MM-yyyy} au {to:dd-MM-yyyy}";
            withholdingsWorksheet.Cells[1, 1, 1, 5].Merge = true;

            withholdingsWorksheet.Cells[2, 1].Value = "Nom du destinataire";
            withholdingsWorksheet.Cells[2, 2].Value = "Email du destinataire";
            withholdingsWorksheet.Cells[2, 3].Value = "Date d'éxécution";
            withholdingsWorksheet.Cells[2, 4].Value = "Montant TTC";
            withholdingsWorksheet.Cells[2, 5].Value = "Raison";

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
        }
    }
}