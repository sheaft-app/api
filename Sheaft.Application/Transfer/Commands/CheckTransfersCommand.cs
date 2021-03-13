using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Transfer.Commands
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