using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core;

namespace Sheaft.Application.Interfaces.Infrastructure
{
    public interface IPdfGenerator
    {
        Task<Result<byte[]>> GeneratePdfAsync<T>(string filename, string templateId, T data, CancellationToken token);
    }
}