using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface ITagQueries
    {
        IQueryable<TagDto> GetTag(Guid id, RequestUser currentUser);
        IQueryable<TagDto> GetTags(RequestUser currentUser);
    }
}