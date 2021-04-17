using System;
using System.Collections.Generic;
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

namespace Sheaft.Mediatr.Product.Commands
{
    public class DeleteProductsCommand : Command
    {
        [JsonConstructor]
        public DeleteProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> ProductIds { get; set; }
    }

    public class DeleteProductsCommandHandler : CommandsHandler,
        IRequestHandler<DeleteProductsCommand, Result>
    {
        public DeleteProductsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteProductsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteProductsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var id in request.ProductIds)
                {
                    var result = await _mediatr.Process(new DeleteProductCommand(request.RequestUser) {ProductId = id}, token);
                    if (!result.Succeeded)
                        return Failure(result);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}