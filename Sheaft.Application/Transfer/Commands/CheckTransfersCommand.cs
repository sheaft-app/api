using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckTransfersCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckTransfersCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
    
    public class CheckTransfersCommandHandler : CommandsHandler,
        IRequestHandler<CheckTransfersCommand, Result<bool>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public CheckTransfersCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CheckTransfersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckTransfersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
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

                return Ok(true);
            });
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
