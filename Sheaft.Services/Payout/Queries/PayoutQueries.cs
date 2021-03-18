using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Services.Payout.Queries
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