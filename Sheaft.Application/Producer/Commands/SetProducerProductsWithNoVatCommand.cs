using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class SetProducerProductsWithNoVatCommand : Command<bool>
    {
        [JsonConstructor]
        public SetProducerProductsWithNoVatCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
    
    public class SetProducerProductsWithNoVatCommandHandler : CommandsHandler,
        IRequestHandler<SetProducerProductsWithNoVatCommand, Result<bool>>
    {
        private readonly RoleOptions _roleOptions;
        private readonly IBlobService _blobService;

        public SetProducerProductsWithNoVatCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IBlobService blobService,
            ILogger<SetProducerProductsWithNoVatCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _blobService = blobService;
        }
        public async Task<Result<bool>> Handle(SetProducerProductsWithNoVatCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.ProducerId, token);
                if (!producer.NotSubjectToVat)
                    return Ok(false);

                var products = await _context.FindAsync<Product>(p => p.Producer.Id == producer.Id, token);
                foreach(var product in products)
                {
                    product.SetVat(0);
                    await _context.SaveChangesAsync(token);
                }

                return Ok(true);
            });
        }        
    }
}
