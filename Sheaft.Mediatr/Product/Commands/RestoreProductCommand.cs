﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.Product.Commands
{
    public class RestoreProductCommand : Command
    {
        protected RestoreProductCommand()
        {
            
        }
        [JsonConstructor]
        public RestoreProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
    }

    public class RestoreProductCommandHandler : CommandsHandler,
        IRequestHandler<RestoreProductCommand, Result>
    {
        public RestoreProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreProductCommand request, CancellationToken token)
        {
            var entity =
                await _context.Products.SingleOrDefaultAsync(a => a.Id == request.ProductId && a.RemovedOn.HasValue, token);
            
            if(entity.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
                
            _context.Restore(entity);
            await _context.SaveChangesAsync(token);
            
            _mediatr.Post(new UpdateProducerProductsCommand(request.RequestUser) {ProducerId = entity.Producer.Id});
            return Success();
        }
    }
}