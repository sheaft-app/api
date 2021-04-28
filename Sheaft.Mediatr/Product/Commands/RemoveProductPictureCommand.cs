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
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Product.Commands
{
    public class RemoveProductPictureCommand : Command
    {
        protected RemoveProductPictureCommand()
        {
            
        }
        [JsonConstructor]
        public RemoveProductPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid ProductId { get; set; }
        public Guid PictureId { get; set; }
    }

    public class RemoveProductPictureCommandHandler : CommandsHandler,
        IRequestHandler<RemoveProductPictureCommand, Result>
    {
        public RemoveProductPictureCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RemoveProductPictureCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RemoveProductPictureCommand request, CancellationToken token)
        {
            var entity = await _context.Products.SingleAsync(e => e.Id == request.ProductId, token);
            if(entity.ProducerId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            entity.RemovePicture(request.PictureId);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}