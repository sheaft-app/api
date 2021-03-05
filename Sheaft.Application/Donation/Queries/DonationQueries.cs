
using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Donation.Queries
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