﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.PreAuthorization.Commands
{
    public class CheckPreAuthorizationsCommand : Command
    {
        protected CheckPreAuthorizationsCommand()
        {
            
        }
        [JsonConstructor]
        public CheckPreAuthorizationsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class CheckPreAuthorizationsCommandHandler : CommandsHandler,
        IRequestHandler<CheckPreAuthorizationsCommand, Result>
    {
        public CheckPreAuthorizationsCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckPreAuthorizationsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckPreAuthorizationsCommand request, CancellationToken token)
        {
            var skip = 0;
            const int take = 100;

            var preAuthorizationIds = await GetNextPreAuthorizationIdsAsync(skip, take, token);
            while (preAuthorizationIds.Any())
            {
                foreach (var preAuthorizationId in preAuthorizationIds)
                    _mediatr.Post(new RefreshPreAuthorizationStatusCommand(request.RequestUser, preAuthorizationId));

                skip += take;
                preAuthorizationIds = await GetNextPreAuthorizationIdsAsync(skip, take, token);
            }

            return Success();
        }

        private async Task<IEnumerable<string>> GetNextPreAuthorizationIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.PreAuthorizations
                .Where(c => !c.Processed && (c.Status == PreAuthorizationStatus.Created || c.Status == PreAuthorizationStatus.Succeeded))
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Identifier)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}