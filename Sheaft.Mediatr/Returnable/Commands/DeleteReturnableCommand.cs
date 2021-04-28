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
using Sheaft.Domain;

namespace Sheaft.Mediatr.Returnable.Commands
{
    public class DeleteReturnableCommand : Command
    {
        protected DeleteReturnableCommand()
        {
            
        }
        [JsonConstructor]
        public DeleteReturnableCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ReturnableId { get; set; }
    }

    public class DeleteReturnableCommandHandler : CommandsHandler,
        IRequestHandler<DeleteReturnableCommand, Result>
    {
        public DeleteReturnableCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteReturnableCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteReturnableCommand request, CancellationToken token)
        {
            var entity = await _context.Returnables.SingleAsync(e => e.Id == request.ReturnableId, token);

            _context.Remove(entity);

            var products = await _context.Products
                .Where(p => p.ReturnableId == entity.Id)
                .ToListAsync(token);

            foreach (var product in products)
                product.SetReturnable(null);
            
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}