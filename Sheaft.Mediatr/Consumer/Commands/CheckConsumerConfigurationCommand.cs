using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Mediatr.Legal.Commands;
using Sheaft.Mediatr.Wallet.Commands;

namespace Sheaft.Mediatr.Consumer.Commands
{
    public class CheckConsumerConfigurationCommand : Command
    {
        [JsonConstructor]
        public CheckConsumerConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
            ConsumerId = requestUser.Id;
        }

        public Guid ConsumerId { get; set; }
    }

    public class CheckConsumerConfigurationCommandHandler : CommandsHandler,
        IRequestHandler<CheckConsumerConfigurationCommand, Result>
    {
        public CheckConsumerConfigurationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CheckConsumerConfigurationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckConsumerConfigurationCommand request, CancellationToken token)
        {
            var business =
                await _mediatr.Process(
                    new CheckConsumerLegalConfigurationCommand(request.RequestUser) {UserId = request.ConsumerId}, token);
            if (!business.Succeeded)
                return Failure(business);

            var wallet =
                await _mediatr.Process(
                    new CheckWalletPaymentsConfigurationCommand(request.RequestUser) {UserId = request.ConsumerId}, token);
            if (!wallet.Succeeded)
                return Failure(wallet);

            return Success();
        }
    }
}