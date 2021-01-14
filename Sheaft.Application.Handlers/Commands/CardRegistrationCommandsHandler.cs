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
        IRequestHandler<CreateCardRegistrationCommand, Result<CardRegistrationDto>>
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

        public async Task<Result<CardRegistrationDto>> Handle(CreateCardRegistrationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.UserId, token);

                var result = await _pspService.CreateCardRegistrationAsync(user, token);
                if (!result.Success)
                    return Failed<CardRegistrationDto>(result.Exception);

                return Ok(new CardRegistrationDto{
                    Identifier = result.Data.Id,
                    AccessKey = result.Data.AccessKey,
                    PreRegistrationData = result.Data.PreregistrationData,
                    Url = result.Data.CardRegistrationURL
                });
            });
        }
    }
}
