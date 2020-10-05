using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class PayinEventsHandler : EventsHandler,
        INotificationHandler<PayinSucceededEvent>
    {
        private readonly IConfiguration _configuration;

        public PayinEventsHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
            _configuration = configuration;
        }

        public async Task Handle(PayinSucceededEvent payinEvent, CancellationToken token)
        {
            var payin = await _context.GetByIdAsync<Payin>(payinEvent.PayinId, token);
            if (payin.Order.Payin.Id != payin.Id)
                return;

            if (payin.Status != TransactionStatus.Succeeded)
                return;

            await _emailService.SendTemplatedEmailAsync(payin.Order.User.Email, payin.Order.User.Name, _emailTemplateOptions.PayinSucceededEvent, new { payin.Order.User.FirstName, payin.Order.User.LastName, payin.Order.TotalPrice, payin.Order.CreatedOn, payin.Order.ProductsCount, payin.Order.Reference, MyOrdersUrl = $"{_configuration.GetValue<string>("Urls:Portal")}/#/my-orders" }, token);
        }
    }
}