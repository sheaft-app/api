using Sheaft.Models.Dto;
using Sheaft.Interop;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IPackagingQueries
    {
        IQueryable<PackagingDto> GetPackaging(Guid id, IRequestUser currentUser);
        IQueryable<PackagingDto> GetPackagings(IRequestUser currentUser);
    }
}