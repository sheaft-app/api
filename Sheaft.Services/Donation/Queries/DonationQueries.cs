using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Services.Donation.Queries
{
    public class DonationQueries : IDonationQueries
    {
        private readonly IAppDbContext _context;
        private readonly IConfigurationProvider _configurationProvider;

        public DonationQueries(IAppDbContext context, IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<DonationDto> GetDonation(Guid id, RequestUser currentUser)
        {
            return _context.Donations
                .Get(d => d.Id == id && d.Author.Id == currentUser.Id)
                .ProjectTo<DonationDto>(_configurationProvider);
        }

        public IQueryable<DonationDto> GetDonations(RequestUser currentUser)
        {
            return _context.Donations
                .Get(d => d.Author.Id == currentUser.Id)
                .ProjectTo<DonationDto>(_configurationProvider);
        }
    }
}