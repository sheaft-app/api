using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Payin.Queries
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

        public IQueryable<PayinDto> GetPayin(string identifier, RequestUser currentUser)
        {
            return _context.Payins
                    .OfType<Domain.Payin>()
                    .Get(c => c.Identifier == identifier && c.Author.Id == currentUser.Id)
                    .ProjectTo<PayinDto>(_configurationProvider);
        }

        public IQueryable<PayinDto> GetPayin(Guid id, RequestUser currentUser)
        {
            return _context.Payins
                    .OfType<Domain.Payin>()
                    .Get(c => c.Id == id && c.Author.Id == currentUser.Id)
                    .ProjectTo<PayinDto>(_configurationProvider);
        }
        
        public IQueryable<WebPayinDto> GetWebPayin(Guid id, RequestUser currentUser)
        {
            return _context.Payins
                .OfType<Domain.WebPayin>()
                .Get(c => c.Id == id && c.Author.Id == currentUser.Id)
                .ProjectTo<WebPayinDto>(_configurationProvider);
        }
        
        public IQueryable<PayinDto> GetPreAuthorizedPayin(Guid id, RequestUser currentUser)
        {
            return _context.Payins
                .OfType<Domain.PreAuthorizedPayin>()
                .Get(c => c.Id == id && c.Author.Id == currentUser.Id)
                .ProjectTo<PayinDto>(_configurationProvider);
        }
    }
}