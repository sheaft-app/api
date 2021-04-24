using System;
using System.Linq;
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
using Sheaft.Domain;

namespace Sheaft.Mediatr.Producer.Commands
{
    public class SetProducerProductsWithNoVatCommand : Command
    {
        protected SetProducerProductsWithNoVatCommand()
        {
            
        }
        [JsonConstructor]
        public SetProducerProductsWithNoVatCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }

    public class SetProducerProductsWithNoVatCommandHandler : CommandsHandler,
        IRequestHandler<SetProducerProductsWithNoVatCommand, Result>
    {
        public SetProducerProductsWithNoVatCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<SetProducerProductsWithNoVatCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetProducerProductsWithNoVatCommand request, CancellationToken token)
        {
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);
            if (!producer.NotSubjectToVat)
                return Failure(MessageKind.BadRequest);

            var products = await _context.Products
                .Where(p => p.Producer.Id == producer.Id)
                .ToListAsync(token);
            
            foreach (var product in products)
                product.SetVat(0);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}