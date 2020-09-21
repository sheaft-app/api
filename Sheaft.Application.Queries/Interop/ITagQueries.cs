using Sheaft.Application.Models;
using Sheaft.Core;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface ITagQueries
    {
        IQueryable<TagDto> GetTag(Guid id, RequestUser currentUser);
        IQueryable<TagDto> GetTags(RequestUser currentUser);
    }
}