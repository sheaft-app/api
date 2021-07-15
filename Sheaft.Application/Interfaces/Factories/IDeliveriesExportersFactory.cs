using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Factories
{
    public interface IDeliveriesExportersFactory
    {
        IDeliveriesFileExporter GetExporter(RequestUser requestUser, string typename);
        Task<IDeliveriesFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token);
    }
}