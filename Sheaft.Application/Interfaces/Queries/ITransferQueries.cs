using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface ITransferQueries
    {
        IQueryable<TransferDto> GetTransfer(Guid id, RequestUser currentUser);
        IQueryable<TransferDto> GetTransfers(RequestUser currentUser);
    }
}