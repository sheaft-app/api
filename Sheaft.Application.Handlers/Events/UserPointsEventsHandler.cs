using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Infrastructure.Interop;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{
    public class UserPointsEventsHandler :
        INotificationHandler<UserPointsCreatedEvent>
    {
        private readonly ISignalrService _signalrService;

        public UserPointsEventsHandler(ISignalrService signalrService)
        {
            _signalrService = signalrService;
        }

        public async Task Handle(UserPointsCreatedEvent appEvent, CancellationToken token)
        {
            await _signalrService.SendNotificationToUserAsync(appEvent.UserId, nameof(UserPointsCreatedEvent), appEvent);
        }
    }
}