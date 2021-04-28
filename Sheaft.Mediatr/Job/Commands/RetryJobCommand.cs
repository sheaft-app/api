using System;
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
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.PickingOrders.Commands;
using Sheaft.Mediatr.Product.Commands;
using Sheaft.Mediatr.Transactions.Commands;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.Mediatr.Job.Commands
{
    public class RetryJobCommand : Command
    {
        protected RetryJobCommand()
        {
            
        }
        [JsonConstructor]
        public RetryJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }

    public class RetryJobCommandHandler : CommandsHandler,
        IRequestHandler<RetryJobCommand, Result>
    {
        public RetryJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RetryJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RetryJobCommand request, CancellationToken token)
        {
            var entity = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if(entity.UserId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            entity.RetryJob();
            await _context.SaveChangesAsync(token);

            switch (entity.Kind)
            {
                case JobKind.ExportPickingOrders:
                    _mediatr.Post(entity.GetCommand<ExportPickingOrderCommand>());
                    break;
                case JobKind.ExportUserData:
                    _mediatr.Post(entity.GetCommand<ExportUserDataCommand>());
                    break;
                case JobKind.ImportProducts:
                    _mediatr.Post(entity.GetCommand<ImportProductsCommand>());
                    break;
                case JobKind.ExportUserTransactions:
                    _mediatr.Post(entity.GetCommand<ExportTransactionsCommand>());
                    break;
            }
            
            return Success();
        }
    }
}