using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.PickingOrder.Commands;
using Sheaft.Mediatr.Product.Commands;
using Sheaft.Mediatr.Transaction.Commands;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.Mediatr.Job.Commands
{
    public class ResetJobCommand : Command
    {
        protected ResetJobCommand()
        {
            
        }
        [JsonConstructor]
        public ResetJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }
    public class ResetJobCommandHandler : CommandsHandler,
        IRequestHandler<ResetJobCommand, Result>
    {
        public ResetJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<ResetJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ResetJobCommand request, CancellationToken token)
        {
            var entity = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if(entity.UserId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            entity.ResetJob();
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
