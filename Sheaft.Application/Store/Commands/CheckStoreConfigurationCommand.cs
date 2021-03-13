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

namespace Sheaft.Application.Store.Commands
{
    public class CheckStoreConfigurationCommand : Command
    {
        [JsonConstructor]
        public CheckStoreConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid StoreId { get; set; }
    }

    public class CheckStoreConfigurationCommandHandler : CommandsHandler,
        IRequestHandler<CheckStoreConfigurationCommand, Result>
    {
        public CheckStoreConfigurationCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckStoreConfigurationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckStoreConfigurationCommand request, CancellationToken token)
        {
            var business =
                await _mediatr.Process(
                    new CheckBusinessLegalConfigurationCommand(request.RequestUser) {UserId = request.StoreId}, token);
            if (!business.Succeeded)
                return Failure(business.Exception);

            var wallet =
                await _mediatr.Process(
                    new CheckWalletPaymentsConfigurationCommand(request.RequestUser) {UserId = request.StoreId}, token);
            if (!wallet.Succeeded)
                return Failure(wallet.Exception);

            return Success();
        }
    }
}