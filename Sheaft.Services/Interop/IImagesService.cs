using Sheaft.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Services.Interop
{
    public interface IImageService
    {
        Task<string> HandleUserImageAsync(Guid id, string picture, CancellationToken token);
        Task<string> HandleProductImageAsync(Product entity, string picture, CancellationToken token);
    }
}