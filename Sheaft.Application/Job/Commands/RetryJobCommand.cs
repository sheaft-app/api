using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Application.PickingOrders.Commands;
using Sheaft.Application.Product.Commands;
using Sheaft.Application.Transactions.Commands;
using Sheaft.Application.User.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Job.Commands
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
                throw SheaftException.Forbidden();

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
                    var exportTransactionsCommand = JsonConvert.DeserializeObject<ExportUserTransactionsCommand>(entity.Command);
                    _mediatr.Post(new ExportUserTransactionsCommand(request.RequestUser) { JobId = exportTransactionsCommand.JobId, From = exportTransactionsCommand.From, To = exportTransactionsCommand.To});
                    break;
            }
            
            return Success();
        }
    }
}