using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Queries
{
    public class TransactionQueries : ITransactionQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public TransactionQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<WebPayinTransactionDto> GetWebPayinTransaction(string identifier, RequestUser currentUser)
        {
            return _context.Payins
                    .OfType<WebPayin>()
                    .Get(c => c.Identifier == identifier && c.Author.Id == currentUser.Id)
                    .ProjectTo<WebPayinTransactionDto>(_configurationProvider);
        }

        public IQueryable<WebPayinTransactionDto> GetWebPayinTransaction(Guid id, RequestUser currentUser)
        {
            return _context.Payins
                    .OfType<WebPayin>()
                    .Get(c => c.Id == id && c.Author.Id == currentUser.Id)
                    .ProjectTo<WebPayinTransactionDto>(_configurationProvider);
        }
    }
}