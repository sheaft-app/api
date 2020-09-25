﻿using Sheaft.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface IIdentifierService
    {
        Task<Result<string>> GetNextOrderReferenceAsync(Guid serialNumber, CancellationToken token);
        Task<Result<string>> GetNextProductReferenceAsync(Guid serialNumber, CancellationToken token);
        Task<Result<string>> GetNextSponsoringCode(CancellationToken token);
    }
}
