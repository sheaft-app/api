using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;

namespace Sheaft.Application.Producer.Commands
{
    public class SetProducerProductsWithNoVatCommand : Command
    {
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
            var producer = await _context.GetByIdAsync<Domain.Producer>(request.ProducerId, token);
            if (!producer.NotSubjectToVat)
                return Failure();

            var products = await _context.FindAsync<Domain.Product>(p => p.Producer.Id == producer.Id, token);
            foreach (var product in products)
            {
                product.SetVat(0);
                await _context.SaveChangesAsync(token);
            }

            return Success();
        }
    }
}