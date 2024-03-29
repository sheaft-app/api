﻿using System;
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

namespace Sheaft.Mediatr.PayinRefund.Commands
{
    public class CheckPayinRefundCommand : Command
    {
        protected CheckPayinRefundCommand()
        {
            
        }
        [JsonConstructor]
        public CheckPayinRefundCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid PayinRefundId { get; set; }
    }

    public class CheckPayinRefundCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayinRefundCommand, Result>
    {
        public CheckPayinRefundCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckPayinRefundCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckPayinRefundCommand request, CancellationToken token)
        {
            var payinRefund = await _context.Set<Domain.PayinRefund>().SingleAsync(e => e.Id == request.PayinRefundId, token);
            if (payinRefund.Status == TransactionStatus.Created || payinRefund.Status == TransactionStatus.Waiting)
            {
                var result =
                    await _mediatr.Process(
                        new RefreshPayinRefundStatusCommand(request.RequestUser, payinRefund.Identifier), token);
                if (!result.Succeeded)
                    return Failure(result);
            }

            return Success();
        }
    }
}