using Sheaft.Core;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckConsumerConfigurationCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckConsumerConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class CheckConsumerConfigurationCommandHandler : CommandsHandler,
        IRequestHandler<CheckConsumerConfigurationCommand, Result<bool>>
    {
        private readonly RoleOptions _roleOptions;

        public CheckConsumerConfigurationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CheckConsumerConfigurationCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckConsumerConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var business = await _mediatr.Process(new CheckConsumerLegalConfigurationCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Process(new CheckWalletPaymentsConfigurationCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                return Ok(true);
            });
        }
    }
}
