using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;

namespace Sheaft.Infrastructure.DocumentManagement;

public class PreparationFileGenerator : IPreparationFileGenerator
{
    public Task<Result<byte[]>> Generate(PreparationDocumentData documentData, CancellationToken token)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        using var stream = new MemoryStream();
        using var package = new ExcelPackage(stream);

        var data = CreateExcelPickingFile(package, documentData);
        return Task.FromResult(Result.Success(data));
    }

    private byte[] CreateExcelPickingFile(ExcelPackage package, PreparationDocumentData documentData)
    {
        using var worksheet = package.Workbook.Worksheets.Add("Commandes à préparer");
        
        var currentRow = 1;
        currentRow = AddHeaderRows(worksheet, documentData, currentRow);
        var productStartingRow = currentRow;
        currentRow = AddProductsRows(worksheet, documentData.Clients, documentData.Products, currentRow);
        currentRow = AddFooterRow(worksheet, documentData.Clients, currentRow, productStartingRow);

        return package.GetAsByteArray();
    }

    private int AddHeaderRows(ExcelWorksheet worksheet, PreparationDocumentData documentData, int currentRow)
    {
        currentRow = AddHeaderTableNameRow(worksheet, documentData.Clients, currentRow);
        currentRow = AddHeaderClientsRow(worksheet, documentData.Clients, currentRow);
        currentRow = AddHeaderClientOrdersRow(worksheet, documentData.Clients, currentRow);
        return currentRow;
    }

    private int AddHeaderTableNameRow(ExcelWorksheet worksheet, IEnumerable<ClientOrdersToPrepare> clientOrders, int currentRow)
    {
        var columnsCount = GetTotalColumnCount(clientOrders);
        AddValueToRange(worksheet, currentRow, 1, currentRow, columnsCount, "Vos commandes", true, true, 14, ExcelBorderStyle.Thin,
            ExcelHorizontalAlignment.Center, 100);

        return currentRow + 1;
    }

    private static int AddHeaderClientsRow(ExcelWorksheet worksheet, IEnumerable<ClientOrdersToPrepare> clientOrders, int currentRow)
    {
        AddValueToRange(worksheet, currentRow, 1, currentRow, 1, "Clients", false, true,
            12, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, 25);

        var currentClientColumn = 2;
        foreach (var clientOrder in GetOrderedClients(clientOrders))
        {
            var clientColumnCount = clientOrder.OrdersCount + 1;
            AddValueToRange(worksheet, currentRow, currentClientColumn, currentRow, currentClientColumn + clientColumnCount - 1,
                clientOrder.ClientName, true, true, 12, ExcelBorderStyle.Thin,
                ExcelHorizontalAlignment.Center, 25);

            currentClientColumn += clientColumnCount;
        }

        AddValueToRange(worksheet, currentRow, currentClientColumn, currentRow, currentClientColumn,
            "Total", true, true, 12, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, 25);

        return currentRow + 1;
    }

    private static int AddHeaderClientOrdersRow(ExcelWorksheet worksheet,
        IEnumerable<ClientOrdersToPrepare> clientOrders, int currentRow)
    {
        AddValueToRange(worksheet, currentRow, 1, currentRow, 1, "Produits\\Commandes", false, false,
            10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, 25);

        var currentClientColumn = 2;
        foreach (var clientOrder in GetOrderedClients(clientOrders))
            currentClientColumn += AddHeaderClientOrdersColumns(worksheet, clientOrder, currentRow, currentClientColumn);

        return currentRow + 1;
    }

    private static int AddHeaderClientOrdersColumns(ExcelWorksheet worksheet, ClientOrdersToPrepare clientOrder,
        int currentRow, int currentClientColumn)
    {
        var startingColumn = currentClientColumn;
        foreach (var orderReference in GetOrderedClientOrders(clientOrder))
        {
            AddValueToRange(worksheet, currentRow, currentClientColumn, currentRow, currentClientColumn,
                orderReference.Value, bold: false);

            currentClientColumn++;
        }

        AddValueToRange(worksheet, currentRow, currentClientColumn, currentRow, currentClientColumn,
            "Sous-total");
        
        currentClientColumn++;

        return currentClientColumn - startingColumn;
    }

    private static int AddProductsRows(ExcelWorksheet worksheet,
        IEnumerable<ClientOrdersToPrepare> clientOrders, IEnumerable<ProductToPrepare> productsToPrepare, int currentRow)
    {
        foreach (var productToPrepare in productsToPrepare.OrderBy(p => p.Name.Value))
            currentRow += AddProductRow(worksheet, clientOrders, productToPrepare, currentRow);

        return currentRow;
    }

    private static int AddProductRow(ExcelWorksheet worksheet, IEnumerable<ClientOrdersToPrepare> clientOrders,
        ProductToPrepare productToPrepare, int currentRow)
    {
        var currentColumn = 1;
        
        AddValueToRange(worksheet, currentRow, currentColumn, currentRow, currentColumn,
            productToPrepare.Name.Value, bold: false, horizontalAlignment: ExcelHorizontalAlignment.Left);

        currentColumn++;

        var productClientTotalCells = new List<ExcelRange>();
        foreach (var clientOrder in GetOrderedClients(clientOrders))
        {
            var result =
                AddClientOrderedProductQuantities(worksheet, clientOrder, productToPrepare, currentRow, currentColumn);

            currentColumn += result.ColumnsAdded;
            productClientTotalCells.Add(result.ClientTotalCell);
        }

        var formula = GetProductRowTotalFormula(productClientTotalCells);
        AddFormulaToRange(worksheet, currentRow, currentColumn, currentRow, currentColumn, formula);
        
        return 1;
    }

    private static string GetProductRowTotalFormula(List<ExcelRange> productClientTotalCells)
    {
        var cells = "";
        foreach (var productClientTotalCell in productClientTotalCells)
        {
            if (cells.Length > 0)
                cells += ",";

            cells += $"{productClientTotalCell.Address}";
        }

        return $"SUM({cells})";
    }

    private static (int ColumnsAdded, ExcelRange ClientTotalCell) AddClientOrderedProductQuantities(ExcelWorksheet worksheet, 
        ClientOrdersToPrepare clientOrder, ProductToPrepare productToPrepare, int currentRow, int currentColumn)
    {
        var startingColumn = currentColumn;
        foreach (var orderReference in GetOrderedClientOrders(clientOrder))
        {
            var productQuantityPerOrder =
                productToPrepare.QuantityPerOrder.SingleOrDefault(q => orderReference == q.OrderReference);

            var quantity = productQuantityPerOrder?.Quantity.Value ?? 0;
            AddValueToRange(worksheet, currentRow, currentColumn, currentRow, currentColumn, quantity, bold: false);

            currentColumn++;
        }

        var startCell = worksheet.Cells[currentRow, startingColumn, currentRow, startingColumn];
        var endCell = worksheet.Cells[currentRow, currentColumn - 1, currentRow, currentColumn - 1];

        var totalCell = worksheet.Cells[currentRow, currentColumn, currentRow, currentColumn];
        AddFormulaToRange(worksheet, currentRow, currentColumn, currentRow, currentColumn,
            $"SUM({startCell.Address}:{endCell.Address})");

        currentColumn++;
        return (currentColumn - startingColumn, totalCell);
    }

    private int AddFooterRow(ExcelWorksheet worksheet, IEnumerable<ClientOrdersToPrepare> clientOrders, int currentRow, int productsStartRow)
    {
        AddValueToRange(worksheet, currentRow, 1, currentRow, 1, "Total", false, true,
            10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, 25);

        var ordersColumnCount = GetOrdersColumnCount(clientOrders) + 1;
        for (var i = 1; i < ordersColumnCount; i++)
        {
            var currentColumn = i + 1;
            
            var startCell = worksheet.Cells[productsStartRow, currentColumn, productsStartRow, currentColumn];
            var endCell = worksheet.Cells[currentRow - 1, currentColumn, currentRow - 1, currentColumn];
            
            AddFormulaToRange(worksheet, currentRow, currentColumn, currentRow, currentColumn,
                $"SUM({startCell.Address}:{endCell.Address})");
        }

        var totalColumnNumber = 1 + ordersColumnCount;
        
        var startTotalCell = worksheet.Cells[productsStartRow, totalColumnNumber, productsStartRow, totalColumnNumber];
        var endTotalCell = worksheet.Cells[currentRow - 1, totalColumnNumber, currentRow - 1, totalColumnNumber];
        
        AddFormulaToRange(worksheet, currentRow, totalColumnNumber, currentRow, totalColumnNumber,
            $"SUM({startTotalCell.Address}:{endTotalCell.Address})");

        return currentRow + 1;
    }

    private static List<ClientOrdersToPrepare> GetOrderedClients(IEnumerable<ClientOrdersToPrepare> clientOrders)
    {
        return clientOrders.OrderBy(co => co.ClientName).ToList();
    }

    private static List<OrderReference> GetOrderedClientOrders(ClientOrdersToPrepare clientOrder)
    {
        return clientOrder.Orders.OrderBy(o => o.Value).ToList();
    }

    private int GetTotalColumnCount(IEnumerable<ClientOrdersToPrepare> clientOrders)
    {
        return GetOrdersColumnCount(clientOrders) + 2; //add 2 columns (1 for product name and 1 for row total)
    }

    private static int GetOrdersColumnCount(IEnumerable<ClientOrdersToPrepare> clientOrders)
    {
        return clientOrders.Sum(co => co.OrdersCount + 1);
    }

    private static void AddValueToRange(ExcelWorksheet worksheet, int fromRow, int fromCol, int toRow, int toCol,
        object value,
        bool merge = false, bool bold = true, int fontSize = 10, ExcelBorderStyle style = ExcelBorderStyle.Thin,
        ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center, int autoFitMinWidth = 0)
    {
        using var rng = worksheet.Cells[fromRow, fromCol, toRow, toCol];

        SetRangeValue(rng, value, bold, fontSize, horizontalAlignment);

        MergeRangeIfRequired(merge, rng);
        AutofitIfSpecified(rng, autoFitMinWidth);
        DefineRangeBorders(rng, style);
    }

    private static void AddFormulaToRange(ExcelWorksheet worksheet, int fromRow, int fromCol, int toRow, int toCol,
        string formula,
        bool merge = false, bool bold = true, int fontSize = 10, ExcelBorderStyle style = ExcelBorderStyle.Thin,
        ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center, int autoFitMinWidth = 0)
    {
        using var rng = worksheet.Cells[fromRow, fromCol, toRow, toCol];

        SetRangeFormula(rng, formula, bold, fontSize, horizontalAlignment);

        MergeRangeIfRequired(merge, rng);
        AutofitIfSpecified(rng, autoFitMinWidth);
        DefineRangeBorders(rng, style);
    }

    private static void AddValueToRange(ExcelWorksheet worksheet, int fromRow, int fromCol, int toRow, int toCol,
        object value,
        Color fontColor, Color backgroundColor, bool merge = false, bool bold = true, int fontSize = 10,
        ExcelBorderStyle style = ExcelBorderStyle.Thin,
        ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center,
        int autoFitMinWidth = 0)
    {
        using var rng = worksheet.Cells[fromRow, fromCol, toRow, toCol];

        AddValueToRange(worksheet, fromRow, fromCol, toRow, toCol, value, merge, bold, fontSize, style,
            horizontalAlignment, autoFitMinWidth);
        FillRangeIfRequired(rng, fontColor, backgroundColor);
    }

    private static void SetRangeValue(ExcelRange rng, object value, bool bold, int fontSize,
        ExcelHorizontalAlignment horizontalAlignment)
    {
        rng.Value = value;
        rng.Style.Font.Bold = bold;
        rng.Style.Font.Size = fontSize;
        rng.Style.HorizontalAlignment = horizontalAlignment;
    }

    private static void SetRangeFormula(ExcelRange rng, string formula, bool bold, int fontSize,
        ExcelHorizontalAlignment horizontalAlignment)
    {
        rng.Formula = formula;
        rng.Style.Font.Bold = bold;
        rng.Style.Font.Size = fontSize;
        rng.Style.HorizontalAlignment = horizontalAlignment;
    }

    private static void FillRangeIfRequired(ExcelRange rng, Color fontColor, Color backgroundColor)
    {
        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
        rng.Style.Fill.BackgroundColor.SetColor(backgroundColor);
        rng.Style.Font.Color.SetColor(fontColor);
    }

    private static void MergeRangeIfRequired(bool merge, ExcelRange rng)
    {
        if (merge)
            rng.Merge = true;
    }

    private static void AutofitIfSpecified(ExcelRange rng, int autoFitMinWidth)
    {
        if (autoFitMinWidth > 0)
            rng.AutoFitColumns(autoFitMinWidth);
    }

    private static void DefineRangeBorders(ExcelRange rng, ExcelBorderStyle style = ExcelBorderStyle.Thin)
    {
        rng.Style.Border.Bottom.Style = style;
        rng.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        rng.Style.Border.Top.Style = style;
        rng.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
        rng.Style.Border.Left.Style = style;
        rng.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
        rng.Style.Border.Right.Style = style;
        rng.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

        rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
    }
}