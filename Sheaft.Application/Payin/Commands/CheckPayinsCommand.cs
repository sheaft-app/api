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
    public class CheckPayinsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckPayinsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
    
    public class CheckPayinsCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayinsCommand, Result<bool>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public CheckPayinsCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CheckPayinsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckPayinsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var skip = 0;
                const int take = 100;

                var payinIds = await GetNextPayinIdsAsync(skip, take, token);
                while (payinIds.Any())
                {
                    foreach (var payinId in payinIds)
                    {
                        _mediatr.Post(new CheckPayinCommand(request.RequestUser)
                        {
                            PayinId = payinId
                        });
                    }

                    skip += take;
                    payinIds = await GetNextPayinIdsAsync(skip, take, token);
                }

                return Ok(true);
            });
        }

        private async Task<IEnumerable<Guid>> GetNextPayinIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Payins
                .Get(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created, true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
