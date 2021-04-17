﻿using System;
using System.Collections.Generic;
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

namespace Sheaft.Mediatr.BusinessClosing.Commands
{
    public class DeleteBusinessClosingsCommand : Command
    {
        [JsonConstructor]
        public DeleteBusinessClosingsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public List<Guid> ClosingIds { get; set; }
    }

    public class DeleteBusinessClosingsCommandHandler : CommandsHandler,
        IRequestHandler<DeleteBusinessClosingsCommand, Result>
    {
        public DeleteBusinessClosingsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteBusinessClosingsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteBusinessClosingsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var closingId in request.ClosingIds)
                {
                    var result = await _mediatr.Process(
                        new DeleteBusinessClosingCommand(request.RequestUser)
                            {ClosingId = closingId}, token);
                    
                    if (!result.Succeeded)
                        return Failure(result);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}