using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Agreement;
using Sheaft.Mediatr.Consumer.Commands;
using Sheaft.Options;

namespace Sheaft.Mediatr.PreAuthorization
{
    public class CreatePreAuthorizationCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePreAuthorizationCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid OrderId { get; set; }
        public string CardIdentifier { get; set; }
    }
    
    public class CreatePreAuthorizationCommandHandler : CommandsHandler,
        IRequestHandler<CreatePreAuthorizationCommand, Result<Guid>>
    {
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;

        public CreatePreAuthorizationCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreatePreAuthorizationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreatePreAuthorizationCommand request, CancellationToken token)
        {
            var order = await _context.GetByIdAsync<Domain.Order>(request.OrderId, token);

            var checkResult =
                await _mediatr.Process(
                    new CheckConsumerConfigurationCommand(request.RequestUser) {ConsumerId = order.User.Id}, token);
            if (!checkResult.Succeeded)
                return Failure<Guid>(checkResult.Exception);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var card = await _context.FindSingleAsync<Card>(c => c.Identifier == request.CardIdentifier, token);
                if (card == null)
                {
                    card = new Card(Guid.NewGuid(), request.CardIdentifier,
                        $"Carte_{DateTime.UtcNow.ToString("YYYYMMDDTHHmmss")}", order.User);
                    await _context.AddAsync(card, token);
                    await _context.SaveChangesAsync(token);
                }

                var preAuthorization =
                    new Domain.PreAuthorization(Guid.NewGuid(), order, card, _pspOptions.PreAuthorizeUrl);
                await _context.AddAsync(preAuthorization, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreatePreAuthorizationAsync(preAuthorization, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result.Exception);

                preAuthorization.SetIdentifier(result.Data.Identifier);
                preAuthorization.SetStatus(result.Data.Status);
                preAuthorization.SetPaymentStatus(result.Data.PaymentStatus);
                preAuthorization.SetExpirationDate(result.Data.ExpirationDate);
                preAuthorization.SetRemaining(result.Data.Remaining);
                preAuthorization.SetDebited(result.Data.Debited);
                preAuthorization.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                preAuthorization.SetSecureModeRedirectUrl(result.Data.SecureModeRedirectUrl);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                if (preAuthorization.Status == PreAuthorizationStatus.Failed)
                    _mediatr.Post(new PreAuthorizationFailedEvent(preAuthorization.Id));

                return Success(preAuthorization.Id);
            }
        }
    }
}