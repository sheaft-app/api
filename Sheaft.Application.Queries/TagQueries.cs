using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Queries
{
    public class TagQueries : ITagQueries
    {
        private readonly IAppDbContext _context;
        private readonly IConfigurationProvider _configurationProvider;

        public TagQueries(IAppDbContext context, IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<TagDto> GetTag(Guid id, RequestUser currentUser)
        {
            return _context.Tags
                    .Get(d => d.Id == id, true)
                    .ProjectTo<TagDto>(_configurationProvider);
        }

        public IQueryable<TagDto> GetTags(RequestUser currentUser)
        {
            return _context.Tags
                    .Get(null, true)
                    .ProjectTo<TagDto>(_configurationProvider);
        }
    }
}