using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Services.Payin.Queries
{
    public class PayinQueries : IPayinQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public PayinQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<WebPayinDto> GetWebPayinTransaction(string identifier, RequestUser currentUser)
        {
            return _context.Payins
                    .OfType<WebPayin>()
                    .Get(c => c.Identifier == identifier && c.Author.Id == currentUser.Id)
                    .ProjectTo<WebPayinDto>(_configurationProvider);
        }

        public IQueryable<WebPayinDto> GetWebPayinTransaction(Guid id, RequestUser currentUser)
        {
            return _context.Payins
                    .OfType<WebPayin>()
                    .Get(c => c.Id == id && c.Author.Id == currentUser.Id)
                    .ProjectTo<WebPayinDto>(_configurationProvider);
        }
    }
}