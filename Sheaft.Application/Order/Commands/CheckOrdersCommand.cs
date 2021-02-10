﻿using System;
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
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Order.Commands
{
    public class CheckOrdersCommand : Command
    {
        [JsonConstructor]
        public CheckOrdersCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }

    public class CheckOrdersCommandHandler : CommandsHandler,
        IRequestHandler<CheckOrdersCommand, Result>
    {
        public CheckOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckOrdersCommand request, CancellationToken token)
        {
            var skip = 0;
            const int take = 100;

            var orderIds = await GetNextOrderIdsAsync(skip, take, token);

            while (orderIds.Any())
            {
                foreach (var orderId in orderIds)
                {
                    _mediatr.Post(new CheckOrderCommand(request.RequestUser)
                    {
                        OrderId = orderId
                    });
                }

                skip += take;
                orderIds = await GetNextOrderIdsAsync(skip, take, token);
            }

            return Success();
        }

        private async Task<IEnumerable<Guid>> GetNextOrderIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Orders
                .Get(c => (c.Payin == null || c.Payin.Status == TransactionStatus.Failed)
                          && (c.Status == OrderStatus.Waiting || c.Status == OrderStatus.Created), true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}