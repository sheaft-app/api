using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Transfer.Queries
{
    public class TransferQueries : ITransferQueries
    {
        private readonly IAppDbContext _context;
        private readonly IConfigurationProvider _configurationProvider;

        public TransferQueries(IAppDbContext context, IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<TransferDto> GetTransfer(Guid id, RequestUser currentUser)
        {
            return _context.Transfers
                    .Get(d => d.Id == id, true)
                    .ProjectTo<TransferDto>(_configurationProvider);
        }

        public IQueryable<TransferDto> GetTransfers(RequestUser currentUser)
        {
            return _context.Transfers
                    .Get(null, true)
                    .ProjectTo<TransferDto>(_configurationProvider);
        }
    }
}