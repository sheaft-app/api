using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Product.Commands
{
    public class RemoveProductPicturesCommand : Command
    {
        protected RemoveProductPicturesCommand()
        {
            
        }
        [JsonConstructor]
        public RemoveProductPicturesCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid ProductId { get; set; }
        public List<Guid> PictureIds { get; set; }
    }

    public class RemoveProductPicturesCommandHandler : CommandsHandler,
        IRequestHandler<RemoveProductPicturesCommand, Result>
    {
        public RemoveProductPicturesCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RemoveProductPicturesCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RemoveProductPicturesCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var pictureId in request.PictureIds)
                {
                    result =
                        await _mediatr.Process(
                            new RemoveProductPictureCommand(request.RequestUser)
                                {PictureId = pictureId, ProductId = request.ProductId}, token);

                    if (!result.Succeeded)
                        break;
                }

                if (result is {Succeeded: false})
                    return result;

                await transaction.CommitAsync(token);
            }

            return Success();
        }
    }
}