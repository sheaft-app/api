using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface ITagQueries
    {
        IQueryable<TagDto> GetTag(Guid id, RequestUser currentUser);
        IQueryable<TagDto> GetTags(RequestUser currentUser);
    }
}