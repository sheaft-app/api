using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{
    public class UserEventsHandler :
        INotificationHandler<UserSponsoredEvent>
    {
        private readonly IAppDbContext _context;
        private readonly ISignalrService _signalrService;

        public UserEventsHandler(IAppDbContext context, ISignalrService signalrService)
        {
            _context = context;
            _signalrService = signalrService;
        }

        public async Task Handle(UserSponsoredEvent accountEvent, CancellationToken token)
        {
            var user = await _context.GetByIdAsync<User>(accountEvent.SponsorId, token);
            var sponsoring = await _context.GetSingleAsync<Sponsoring>(c => c.Sponsor.Id == accountEvent.SponsorId && c.Sponsored.Id == accountEvent.SponsoredId, token);

            await _signalrService.SendNotificationToUserAsync(accountEvent.SponsorId, nameof(SponsoringUsedEvent), new { SponsoredName = sponsoring.Sponsored.Company != null ? sponsoring.Sponsored.Company.Name : $"{sponsoring.Sponsored.FirstName} {sponsoring.Sponsored.LastName}" });
            await _signalrService.SendNotificationToUserAsync(accountEvent.SponsoredId, nameof(NewSponsoredEvent), new { SponsorName = user.Company != null ? user.Company.Name : $"{user.FirstName} {user.LastName}" });
        }
    }
}