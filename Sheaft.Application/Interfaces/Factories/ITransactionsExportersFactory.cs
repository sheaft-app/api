using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Factories
{
    public interface ITransactionsExportersFactory
    {
        ITransactionsFileExporter GetExporter(RequestUser requestUser, string typename);
        Task<ITransactionsFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token);
    }
}