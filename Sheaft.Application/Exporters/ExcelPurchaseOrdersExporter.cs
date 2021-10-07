using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Sheaft.Application.Interfaces.Exporters;
using Sheaft.Application.Models;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Exporters
{
    public class ExcelPurchaseOrdersExporter : SheaftService, IPurchaseOrdersFileExporter
    {
        public ExcelPurchaseOrdersExporter(ILogger<ExcelPurchaseOrdersExporter> logger) : base(logger)
        {
        }
        
        public async Task<Result<ResourceExportDto>> ExportAsync(RequestUser requestUser, DateTimeOffset from, DateTimeOffset to, IQueryable<PurchaseOrder> purchaseOrdersQuery, CancellationToken token)
        {
            using (var stream = new MemoryStream())
            {
                using (var package = new ExcelPackage(stream))
                {
                    var purchaseOrders = await purchaseOrdersQuery.ToListAsync(token);
                    var data = CreateExcelPurchaseOrdersFile(package, requestUser, from, to, purchaseOrders, token);
                    
                    return Success(new ResourceExportDto
                        {Data = data, Extension = "xlsx", MimeType = "application/ms-excel"});
                }
            }
        }

        private byte[] CreateExcelPurchaseOrdersFile(ExcelPackage package, RequestUser user, DateTimeOffset from, DateTimeOffset to, IEnumerable<PurchaseOrder> purchaseOrders, CancellationToken token)
        {
            var purchaseOrdersWorksheet = package.Workbook.Worksheets.Add("Commandes");

            WriteHeaders(from, to, purchaseOrdersWorksheet);
            WriteSubHeaders(purchaseOrdersWorksheet);

            var i = 4;
            foreach (var purchaseOrder in purchaseOrders)
            {
                WritePurchaseOrderInfo(purchaseOrdersWorksheet, purchaseOrder, i);
                foreach (var product in purchaseOrder.Products)
                {
                    WriteProductInfo(purchaseOrdersWorksheet, product, i);
                    i++;
                }
            }

            return package.GetAsByteArray();
        }

        private static void WriteProductInfo(ExcelWorksheet purchaseOrdersWorksheet, PurchaseOrderProduct product, int i)
        {
            purchaseOrdersWorksheet.Cells[i, 10].Value = product.Reference;
            purchaseOrdersWorksheet.Cells[i, 11].Value = product.Name;
            purchaseOrdersWorksheet.Cells[i, 12].Value = product.Quantity;
            purchaseOrdersWorksheet.Cells[i, 13].Value = product.UnitWholeSalePrice;
            purchaseOrdersWorksheet.Cells[i, 14].Value = product.UnitWholeSalePrice * (1+product.Vat / 100);
            purchaseOrdersWorksheet.Cells[i, 15].Value = product.Vat;
            purchaseOrdersWorksheet.Cells[i, 16].Value = product.TotalWholeSalePrice;
            purchaseOrdersWorksheet.Cells[i, 17].Value = product.TotalOnSalePrice;
        }

        private static void WritePurchaseOrderInfo(ExcelWorksheet purchaseOrdersWorksheet, PurchaseOrder purchaseOrder, int i)
        {
            purchaseOrdersWorksheet.Cells[i, 1].Value = purchaseOrder.CreatedOn.ToString("dd/MM/yyyy");
            purchaseOrdersWorksheet.Cells[i, 2].Value = purchaseOrder.SenderInfo.Name;
            purchaseOrdersWorksheet.Cells[i, 3].Value = purchaseOrder.SenderInfo.Email;
            purchaseOrdersWorksheet.Cells[i, 4].Value = purchaseOrder.SenderInfo.Phone;
            purchaseOrdersWorksheet.Cells[i, 5].Value = purchaseOrder.SenderInfo.Address;
            purchaseOrdersWorksheet.Cells[i, 6].Value = purchaseOrder.Reference;
            purchaseOrdersWorksheet.Cells[i, 7].Value = purchaseOrder.TotalWholeSalePrice;
            purchaseOrdersWorksheet.Cells[i, 8].Value = purchaseOrder.TotalVatPrice;
            purchaseOrdersWorksheet.Cells[i, 9].Value = purchaseOrder.TotalOnSalePrice;

            purchaseOrdersWorksheet.Cells[i, 1, i + purchaseOrder.Products.Count - 1, 1].Merge = true;
            purchaseOrdersWorksheet.Cells[i, 2, i + purchaseOrder.Products.Count - 1, 2].Merge = true;
        }

        private static void WriteSubHeaders(ExcelWorksheet purchaseOrdersWorksheet)
        {
            purchaseOrdersWorksheet.Cells[3, 1].Value = "Date de commande";
            purchaseOrdersWorksheet.Cells[3, 2].Value = "Nom client";
            purchaseOrdersWorksheet.Cells[3, 3].Value = "Email";
            purchaseOrdersWorksheet.Cells[3, 4].Value = "Téléphone";
            purchaseOrdersWorksheet.Cells[3, 5].Value = "Adresse";
            purchaseOrdersWorksheet.Cells[3, 6].Value = "Référence commande";
            purchaseOrdersWorksheet.Cells[3, 7].Value = "Total commande HT";
            purchaseOrdersWorksheet.Cells[3, 8].Value = "Total commande TVA";
            purchaseOrdersWorksheet.Cells[3, 9].Value = "Total commande TTC";
            purchaseOrdersWorksheet.Cells[3, 10].Value = "Référence du produit";
            purchaseOrdersWorksheet.Cells[3, 11].Value = "Nom du produit";
            purchaseOrdersWorksheet.Cells[3, 12].Value = "Quantité";
            purchaseOrdersWorksheet.Cells[3, 13].Value = "Prix unitaire (HT)";
            purchaseOrdersWorksheet.Cells[3, 14].Value = "Prix unitaire (TTC)";
            purchaseOrdersWorksheet.Cells[3, 15].Value = "TVA";
            purchaseOrdersWorksheet.Cells[3, 16].Value = "Total produit HT";
            purchaseOrdersWorksheet.Cells[3, 17].Value = "Total produit TTC";
        }

        private static void WriteHeaders(DateTimeOffset from, DateTimeOffset to, ExcelWorksheet purchaseOrdersWorksheet)
        {
            purchaseOrdersWorksheet.Cells[1, 1, 1, 17].Value = $"Vos commandes du {from:dd-MM-yyyy} au {to:dd-MM-yyyy}";
            purchaseOrdersWorksheet.Cells[1, 1, 1, 17].Merge = true;

            purchaseOrdersWorksheet.Cells[2, 1, 2, 9].Value = "Information des commandes";
            purchaseOrdersWorksheet.Cells[2, 1, 2, 9].Merge = true;

            purchaseOrdersWorksheet.Cells[2, 10, 2, 17].Value = "Détails des produits";
            purchaseOrdersWorksheet.Cells[2, 10, 2, 17].Merge = true;
        }
    }
}