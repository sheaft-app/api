using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Interfaces.Importers
{
    public interface IProductsFileImporter
    {
        Task<Result<IEnumerable<ImportedProductDto>>> ImportAsync(byte[] productsFile, IEnumerable<KeyValuePair<Guid, string>> tags, CancellationToken token);
    }
}