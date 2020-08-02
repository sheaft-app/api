using Sheaft.Models.Dto;
using Sheaft.Interop;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface ITagQueries
    {
        IQueryable<TagDto> GetTag(Guid id, IRequestUser currentUser);
        IQueryable<TagDto> GetTags(IRequestUser currentUser);
    }
}