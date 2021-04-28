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

namespace Sheaft.Mediatr.Donation.Queries
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
                .Where(d => d.Id == id && d.AuthorId == currentUser.Id)
                .ProjectTo<DonationDto>(_configurationProvider);
        }

        public IQueryable<DonationDto> GetDonations(RequestUser currentUser)
        {
            return _context.Donations
                .Where(d => d.AuthorId == currentUser.Id)
                .ProjectTo<DonationDto>(_configurationProvider);
        }
    }
}