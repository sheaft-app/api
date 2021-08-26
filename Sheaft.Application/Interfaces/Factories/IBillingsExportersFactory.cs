using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Exporters;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Factories
{
    public interface IBillingsExportersFactory
    {
        IBillingsFileExporter GetExporter(RequestUser requestUser, string typename);
        Task<IBillingsFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token);
    }
}