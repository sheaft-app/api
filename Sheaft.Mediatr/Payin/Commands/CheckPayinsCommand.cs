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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Payin.Commands
{
    public class CheckPayinsCommand : Command
    {
        [JsonConstructor]
        public CheckPayinsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class CheckPayinsCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayinsCommand, Result>
    {
        public CheckPayinsCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckPayinsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckPayinsCommand request, CancellationToken token)
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

            return Success();
        }

        private async Task<IEnumerable<Guid>> GetNextPayinIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Payins
                .Where(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}