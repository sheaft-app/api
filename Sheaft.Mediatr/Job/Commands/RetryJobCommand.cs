using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
            var entity = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(entity.User.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            entity.RetryJob();
            await _context.SaveChangesAsync(token);

            switch (entity.Kind)
            {
                case JobKind.ExportPickingOrders:
                    var exportPickingOrderCommand = JsonConvert.DeserializeObject<ExportPickingOrderCommand>(entity.Command);
                    _mediatr.Post(new ExportPickingOrderCommand(request.RequestUser) { JobId = exportPickingOrderCommand.JobId, PurchaseOrderIds = exportPickingOrderCommand.PurchaseOrderIds });
                    break;
                case JobKind.ExportUserData:
                    var exportUserDataCommand = JsonConvert.DeserializeObject<ExportUserDataCommand>(entity.Command);
                    _mediatr.Post(new ExportUserDataCommand(request.RequestUser) { JobId = exportUserDataCommand.JobId });
                    break;
                case JobKind.ImportProducts:
                    var importProductsCommand = JsonConvert.DeserializeObject<ImportProductsCommand>(entity.Command);
                    _mediatr.Post(new ImportProductsCommand(request.RequestUser) { JobId = importProductsCommand.JobId });
                    break;
                case JobKind.ExportUserTransactions:
                    var exportTransactionsCommand = JsonConvert.DeserializeObject<ExportTransactionsCommand>(entity.Command);
                    _mediatr.Post(new ExportTransactionsCommand(request.RequestUser) { JobId = exportTransactionsCommand.JobId, From = exportTransactionsCommand.From, To = exportTransactionsCommand.To});
                    break;
            }
            
            return Success();
        }
    }
}