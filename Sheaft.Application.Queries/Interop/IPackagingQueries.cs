using Sheaft.Models.Dto;
using Sheaft.Core;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IPackagingQueries
    {
        IQueryable<PackagingDto> GetPackaging(Guid id, RequestUser currentUser);
        IQueryable<PackagingDto> GetPackagings(RequestUser currentUser);
    }
}