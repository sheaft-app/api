using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Infrastructure;
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
            return _context.Transactions
                    .OfType<WebPayinTransaction>()
                    .Get(c => c.Identifier == identifier && c.Author.Id == currentUser.Id)
                    .ProjectTo<WebPayinTransactionDto>(_configurationProvider);
        }

        public IQueryable<WebPayinTransactionDto> GetWebPayinTransaction(Guid id, RequestUser currentUser)
        {
            return _context.Transactions
                    .OfType<WebPayinTransaction>()
                    .Get(c => c.Id == id && c.Author.Id == currentUser.Id)
                    .ProjectTo<WebPayinTransactionDto>(_configurationProvider);
        }
    }
}