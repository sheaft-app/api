﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Application.Legal.Commands;
using Sheaft.Application.Wallet.Commands;
using Sheaft.Domain;

namespace Sheaft.Application.Producer.Commands
{
    public class CheckProducerConfigurationCommand : Command
    {
        [JsonConstructor]
        public CheckProducerConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }

    public class CheckProducerConfigurationCommandHandler : CommandsHandler,
        IRequestHandler<CheckProducerConfigurationCommand, Result>
    {
        public CheckProducerConfigurationCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckProducerConfigurationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckProducerConfigurationCommand request, CancellationToken token)
        {
            var business =
                await _mediatr.Process(
                    new CheckBusinessLegalConfigurationCommand(request.RequestUser) {UserId = request.ProducerId},
                    token);
            if (!business.Succeeded)
                return Failure(business.Exception);

            var wallet =
                await _mediatr.Process(
                    new CheckWalletPaymentsConfigurationCommand(request.RequestUser) {UserId = request.ProducerId},
                    token);
            if (!wallet.Succeeded)
                return Failure(wallet.Exception);

            return Success();
        }
    }
}