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
using Sheaft.Domain;

namespace Sheaft.Mediatr.Producer.Commands
{
    public class UpdateProducerProductsCommand : Command
    {
        protected UpdateProducerProductsCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateProducerProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }

    public class UpdateProducerProductsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProducerProductsCommand, Result>
    {
        public UpdateProducerProductsCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<UpdateProducerProductsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateProducerProductsCommand request, CancellationToken token)
        {
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);
            producer.HasProducts = await _context.Products.AnyAsync(p => p.ProducerId == producer.Id, token);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}