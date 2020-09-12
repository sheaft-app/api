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
    public class TransactionQueries : ITransactionQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public TransactionQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<T> GetTransaction<T>(Guid id, RequestUser currentUser) where T : BaseTransactionDto
        {
            try
            {
                return _context.Transactions
                        .Get(c => c.Id == id && c.Author.Id == currentUser.Id)
                        .ProjectTo<T>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<T>().AsQueryable();
            }
        }

        public IQueryable<T> GetTransaction<T>(string identifier, RequestUser currentUser) where T : BaseTransactionDto
        {
            try
            {
                return _context.Transactions
                        .Get(c => c.Identifier == identifier && c.Author.Id == currentUser.Id)
                        .ProjectTo<T>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<T>().AsQueryable();
            }
        }

        public IQueryable<T> GetTransactions<T>(RequestUser currentUser) where T : BaseTransactionDto
        {
            try
            {
                return _context.Transactions
                        .Get(c => c.Author.Id == currentUser.Id)
                        .ProjectTo<T>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<T>().AsQueryable();
            }
        }
    }
}