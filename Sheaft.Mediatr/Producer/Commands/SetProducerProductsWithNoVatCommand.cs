﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Producer.Commands
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