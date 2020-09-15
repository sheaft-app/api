using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
{
    public class LegalQueries : ILegalQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public LegalQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<T> GetLegal<T>(Guid id, RequestUser currentUser) where T : BaseLegalDto
        {
            try
            {
                return _context.Legals
                        .Get(c => c.Id == id && c.Owner.Id == currentUser.Id)
                        .ProjectTo<T>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<T>().AsQueryable();
            }
        }

        public IQueryable<T> GetLegals<T>(RequestUser currentUser) where T : BaseLegalDto
        {
            try
            {
                return _context.Legals
                        .Get(c => c.Owner.Id == currentUser.Id)
                        .ProjectTo<T>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<T>().AsQueryable();
            }
        }
    }
}