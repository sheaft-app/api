using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Transfer.Queries
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