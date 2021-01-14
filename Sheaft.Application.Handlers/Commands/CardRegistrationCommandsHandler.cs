using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Sheaft.Application.Models;

namespace Sheaft.Application.Handlers
{
    public class CardRegistrationCommandsHandler : ResultsHandler,
        IRequestHandler<CreateCardRegistrationCommand, Result<Guid>>,
        IRequestHandler<ValidateCardRegistrationCommand, Result<Guid>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;

        public CardRegistrationCommandsHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IPspService pspService,
            ICapingDeliveriesService capingDeliveriesService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CardRegistrationCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _capingDeliveriesService = capingDeliveriesService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateCardRegistrationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var user = await _context.GetByIdAsync<User>(request.UserId, token);

                    var cardRegistration = new CardRegistration(Guid.NewGuid(), user);
                    await _context.AddAsync(cardRegistration, token);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreateCardRegistrationAsync(cardRegistration, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    cardRegistration.SetIdentifier(result.Data.CardId);
                    cardRegistration.SetUrl(result.Data.CardRegistrationURL);
                    cardRegistration.SetStatus(result.Data.Status);
                    cardRegistration.SetResult(result.Data.ResultCode);
                    
                    cardRegistration.SetPreRegistrationData(result.Data.PreregistrationData);
                    cardRegistration.SetRegistrationData(result.Data.RegistrationData);
                    cardRegistration.SetAccessKey(result.Data.AccessKey);                    

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(cardRegistration.Id);
                }
            });
        }

        public async Task<Result<Guid>> Handle(ValidateCardRegistrationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var cardRegistration = await _context.GetByIdAsync<CardRegistration>(request.CardId, token);
                var result = await _pspService.ValidateCardAsync(cardRegistration, request.RegistrationData, token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                var card = new Card(Guid.NewGuid(), result.Data, null, request.Remember, cardRegistration);
                await _context.AddAsync(card, token);
                await _context.SaveChangesAsync(token);

                return Ok(card.Id);
            });
        }
    }
}
