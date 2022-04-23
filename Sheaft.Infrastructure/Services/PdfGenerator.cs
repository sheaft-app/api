using Microsoft.Extensions.Logging;
using Razor.Templating.Core;
using Sheaft.Domain;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace Sheaft.Infrastructure.Services;

internal class PdfGenerator : IPdfGenerator
{
    private readonly IConverter _converter;

    public PdfGenerator(IConverter converter)
    {
        _converter = converter;
    }

    public async Task<Result<byte[]>> GeneratePdf<T>(string filename, string templateId, T data,
        CancellationToken token)
    {
        var globalSettings = new GlobalSettings
        {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            DocumentTitle = filename
        };

        var objectSettings = new ObjectSettings
        {
            PagesCount = true,
            HtmlContent = await RazorTemplateEngine.RenderAsync($"~/Templates/{templateId}.cshtml", data ?? throw new ArgumentNullException(nameof(data))),
            WebSettings = {DefaultEncoding = "utf-8"},
        };

        var pdf = new HtmlToPdfDocument()
        {
            GlobalSettings = globalSettings,
            Objects = {objectSettings}
        };

        var result = _converter.Convert(pdf);
        return Result.Success(result);
    }
}