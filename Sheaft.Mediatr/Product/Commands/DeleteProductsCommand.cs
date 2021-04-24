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
        protected DeleteProductsCommand()
        {
        }

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
                Result result = null;
                foreach (var id in request.ProductIds)
                {
                    result = await _mediatr.Process(new DeleteProductCommand(request.RequestUser) {ProductId = id},
                        token);
                    if (!result.Succeeded)
                        break;
                }

                if (result is {Succeeded: false})
                    return result;
                
                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}