using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IDonationQueries
    {
        IQueryable<DonationDto> GetDonation(Guid id, RequestUser currentUser);
        IQueryable<DonationDto> GetDonations(RequestUser currentUser);
    }
}