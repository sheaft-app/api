using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Domain.Extensions;

namespace Sheaft.Mediatr.Picking.Commands
{
    public class GeneratePickingFormCommand : Command
    {
        protected GeneratePickingFormCommand()
        {
        }

        [JsonConstructor]
        public GeneratePickingFormCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PickingId { get; set; }
    }

    public class GeneratePickingFormCommandHandler : CommandsHandler,
        IRequestHandler<GeneratePickingFormCommand, Result>
    {
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IBlobService _blobService;
        private readonly IPickingOrdersExportersFactory _pickingOrdersExportersFactory;

        public GeneratePickingFormCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPdfGenerator pdfGenerator,
            IBlobService blobService,
            IPickingOrdersExportersFactory pickingOrdersExportersFactory,
            ILogger<GeneratePickingFormCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pdfGenerator = pdfGenerator;
            _blobService = blobService;
            _pickingOrdersExportersFactory = pickingOrdersExportersFactory;
        }

        public async Task<Result> Handle(GeneratePickingFormCommand request, CancellationToken token)
        {
            var picking = await _context.Pickings
                .SingleOrDefaultAsync(c => c.Id == request.PickingId, token);
            if (picking == null)
                return Failure("La préparation est introuvable.");
            
            var exporter = await _pickingOrdersExportersFactory.GetExporterAsync(request.RequestUser, token);

            var purchaseOrderIds = picking.PurchaseOrders.Select(po => po.Id);
            var purchaseOrdersQuery = _context.PurchaseOrders
                .Where(po => purchaseOrderIds.Contains(po.Id));
            
            var pickingOrdersExportResult = await exporter.ExportAsync(request.RequestUser, purchaseOrdersQuery, token);
            if (!pickingOrdersExportResult.Succeeded)
                throw pickingOrdersExportResult.Exception;
            
            var response = await _blobService.UploadPickingOrderFileAsync(picking.ProducerId, $"Preparation_{picking.CreatedOn:dd-MM-yyyy}.{pickingOrdersExportResult.Data.Extension}",
                pickingOrdersExportResult.Data.Data, token);
            if (!response.Succeeded)
                throw response.Exception;

            picking.SetPreparationUrl(response.Data);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}