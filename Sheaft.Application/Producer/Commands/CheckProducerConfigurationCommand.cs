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
    public class CheckProducerConfigurationCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckProducerConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
    
    public class CheckProducerConfigurationCommandHandler : CommandsHandler,
        IRequestHandler<CheckProducerConfigurationCommand, Result<bool>>
    {
        private readonly RoleOptions _roleOptions;
        private readonly IBlobService _blobService;

        public CheckProducerConfigurationCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IBlobService blobService,
            ILogger<CheckProducerConfigurationCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _blobService = blobService;
        }

        public async Task<Result<bool>> Handle(CheckProducerConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var business = await _mediatr.Process(new CheckBusinessLegalConfigurationCommand(request.RequestUser) { UserId = request.ProducerId }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Process(new CheckWalletPaymentsConfigurationCommand(request.RequestUser) { UserId = request.ProducerId }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                return Ok(true);
            });
        }
    }
}
