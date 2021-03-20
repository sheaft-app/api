using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Transfer.Commands
{
    public class CheckTransfersCommand : Command
    {
        [JsonConstructor]
        public CheckTransfersCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class CheckTransfersCommandHandler : CommandsHandler,
        IRequestHandler<CheckTransfersCommand, Result>
    {
        public CheckTransfersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckTransfersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckTransfersCommand request, CancellationToken token)
        {
            var skip = 0;
            const int take = 100;

            var transferIds = await GetNextTransferIdsAsync(skip, take, token);
            while (transferIds.Any())
            {
                foreach (var transferId in transferIds)
                {
                    _mediatr.Post(new CheckTransferCommand(request.RequestUser)
                    {
                        TransferId = transferId
                    });
                }

                skip += take;
                transferIds = await GetNextTransferIdsAsync(skip, take, token);
            }

            return Success();
        }

        private async Task<IEnumerable<Guid>> GetNextTransferIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Transfers
                .Get(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created, true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}