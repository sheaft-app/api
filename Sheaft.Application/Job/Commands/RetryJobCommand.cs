using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class RetryJobCommand : Command<bool>
    {
        [JsonConstructor]
        public RetryJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    public class RetryJobCommandHandler : CommandsHandler,
        IRequestHandler<RetryJobCommand, Result<bool>>
    {
        public RetryJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<RetryJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(RetryJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.RetryJob();
                await _context.SaveChangesAsync(token);

                EnqueueJobCommand(entity, request.RequestUser);
                return Ok(true);
            });
        }

        private void EnqueueJobCommand(Job entity, RequestUser requestUser)
        {
            switch (entity.Kind)
            {
                case JobKind.ExportPickingOrders:
                    var exportPickingOrderCommand = JsonConvert.DeserializeObject<ExportPickingOrderCommand>(entity.Command);
                    _mediatr.Post(new ExportPickingOrderCommand(requestUser) { JobId = exportPickingOrderCommand.JobId, PurchaseOrderIds = exportPickingOrderCommand.PurchaseOrderIds });
                    break;
                case JobKind.ExportUserData:
                    var exportUserDataCommand = JsonConvert.DeserializeObject<ExportUserDataCommand>(entity.Command);
                    _mediatr.Post(new ExportUserDataCommand(requestUser) { Id = exportUserDataCommand.Id });
                    break;
                case JobKind.ImportProducts:
                    var importProductsCommand = JsonConvert.DeserializeObject<ImportProductsCommand>(entity.Command);
                    _mediatr.Post(new ImportProductsCommand(requestUser) { Id = importProductsCommand.Id });
                    break;
            }
        }
    }
}
