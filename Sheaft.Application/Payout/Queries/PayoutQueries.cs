
using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Payout.Queries
{
    public class PayoutQueries : IPayoutQueries
    {
        private readonly IAppDbContext _context;
        private readonly IConfigurationProvider _configurationProvider;

        public PayoutQueries(IAppDbContext context, IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<PayoutDto> GetPayout(Guid id, RequestUser currentUser)
        {
            return _context.Payouts
                .Get(d => d.Id == id && d.Author.Id == currentUser.Id)
                .ProjectTo<PayoutDto>(_configurationProvider);
        }

        public IQueryable<PayoutDto> GetPayouts(RequestUser currentUser)
        {
            return _context.Payouts
                .Get(d => d.Author.Id == currentUser.Id)
                .ProjectTo<PayoutDto>(_configurationProvider);
        }
    }
}