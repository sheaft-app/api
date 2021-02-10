using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckConsumerLegalConfigurationCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckConsumerLegalConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
    
    public class CheckConsumerLegalConfigurationCommandHandler : CommandsHandler,
           IRequestHandler<CheckConsumerLegalConfigurationCommand, Result<bool>>
    {
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;

        public CheckConsumerLegalConfigurationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CheckConsumerLegalConfigurationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspOptions = pspOptions.Value;
            _pspService = pspService;
        }
        public async Task<Result<bool>> Handle(CheckConsumerLegalConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<ConsumerLegal>(b => b.User.Id == request.UserId, token);
                if (string.IsNullOrWhiteSpace(legal.User.Identifier))
                {
                    var userResult = await _pspService.CreateConsumerAsync(legal, token);
                    if (!userResult.Success)
                        return Failed<bool>(userResult.Exception);

                    legal.User.SetIdentifier(userResult.Data);
                    await _context.SaveChangesAsync(token);
                }

                return Ok(true);
            });
        }
    }
}
