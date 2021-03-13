using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Application.Legal.Commands;
using Sheaft.Application.Wallet.Commands;
using Sheaft.Domain;

namespace Sheaft.Application.Consumer.Commands
{
    public class CheckConsumerConfigurationCommand : Command
    {
        [JsonConstructor]
        public CheckConsumerConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
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
                return Failure(business.Exception);

            var wallet =
                await _mediatr.Process(
                    new CheckWalletPaymentsConfigurationCommand(request.RequestUser) {UserId = request.ConsumerId}, token);
            if (!wallet.Succeeded)
                return Failure(wallet.Exception);

            return Success();
        }
    }
}