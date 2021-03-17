using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IDonationQueries
    {
        IQueryable<DonationDto> GetDonation(Guid id, RequestUser currentUser);
        IQueryable<DonationDto> GetDonations(RequestUser currentUser);
    }
}