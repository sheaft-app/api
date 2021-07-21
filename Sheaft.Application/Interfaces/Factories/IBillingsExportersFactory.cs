using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Factories
{
    public interface IBillingsExportersFactory
    {
        IBillingsFileExporter GetExporter(RequestUser requestUser, string typename);
        Task<IBillingsFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token);
    }
}