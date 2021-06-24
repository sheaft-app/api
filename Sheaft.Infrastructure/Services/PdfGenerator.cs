using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Razor.Templating.Core;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Services;
using Sheaft.Core;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace Sheaft.Infrastructure.Services
{
    public class PdfGenerator : SheaftService, IPdfGenerator
    {
        private readonly IConverter _converter;

        public PdfGenerator(
            IConverter converter,
            ILogger<PdfGenerator> logger)
            : base(logger)
        {
            _converter = converter;
        }

        public async Task<Result<byte[]>> GeneratePdfAsync<T>(string filename, string templateId, T data,
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
                HtmlContent = await RazorTemplateEngine.RenderAsync($"~/Templates/{templateId}.cshtml", data),
                WebSettings = {DefaultEncoding = "utf-8"},
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = {objectSettings}
            };

            var result = _converter.Convert(pdf);
            return Success(result);
        }
    }
}