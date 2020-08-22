using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure;

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
            try
            {
                return _context.Tags
                        .Get(d => d.Id == id, true)
                        .ProjectTo<TagDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<TagDto>().AsQueryable();
            }
        }

        public IQueryable<TagDto> GetTags(RequestUser currentUser)
        {
            try
            {
                return _context.Tags
                        .Get(null, true)
                        .ProjectTo<TagDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<TagDto>().AsQueryable();
            }
        }

        private static IQueryable<TagDto> GetAsDto(IQueryable<Tag> query)
        {
            return query
                .Select(c => new TagDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Image = c.Image,
                    Kind = c.Kind,
                    UpdatedOn = c.UpdatedOn,
                    CreatedOn = c.CreatedOn
                });
        }
    }
}