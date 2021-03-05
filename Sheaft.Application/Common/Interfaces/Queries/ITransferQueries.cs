using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface ITransferQueries
    {
        IQueryable<TransferDto> GetTransfer(Guid id, RequestUser currentUser);
        IQueryable<TransferDto> GetTransfers(RequestUser currentUser);
    }
}