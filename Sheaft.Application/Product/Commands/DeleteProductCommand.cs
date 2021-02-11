using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Producer.Commands;
using Sheaft.Domain;

namespace Sheaft.Application.Product.Commands
{
    public class DeleteProductCommand : Command
    {
        [JsonConstructor]
        public DeleteProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
    }

    public class DeleteProductCommandHandler : CommandsHandler,
        IRequestHandler<DeleteProductCommand, Result>
    {
        public DeleteProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Product>(request.ProductId, token);
            
            _context.Remove(entity);
            await _context.SaveChangesAsync(token);
            
            _mediatr.Post(new UpdateProducerProductsCommand(request.RequestUser) {ProducerId = entity.Producer.Id});
            return Success();
        }
    }
}