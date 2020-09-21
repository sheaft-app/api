using Sheaft.Core;
using Sheaft.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface IImageService
    {
        Task<Result<string>> HandleUserImageAsync(Guid id, string picture, CancellationToken token);
        Task<Result<string>> HandleProductImageAsync(Product entity, string picture, CancellationToken token);
    }
}