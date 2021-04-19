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

namespace Sheaft.Mediatr.Store.Commands
{
    public class CheckStoreConfigurationCommand : Command
    {
        protected CheckStoreConfigurationCommand()
        {
            
        }
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
                return Failure(business);

            var wallet =
                await _mediatr.Process(
                    new CheckWalletPaymentsConfigurationCommand(request.RequestUser) {UserId = request.StoreId}, token);
            if (!wallet.Succeeded)
                return Failure(wallet);

            return Success();
        }
    }
}