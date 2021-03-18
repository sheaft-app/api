using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Services.Product.Commands
{
    public class AddPictureToProductCommand : Command
    {
        [JsonConstructor]
        public AddPictureToProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid ProductId { get; set; }
        public string Picture { get; set; }
    }

    public class AddPictureToProductCommandHandler : CommandsHandler,
        IRequestHandler<AddPictureToProductCommand, Result>
    {
        private readonly IPictureService _imageService;

        public AddPictureToProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<AddPictureToProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result> Handle(AddPictureToProductCommand request, CancellationToken token)
        {
            var entity = await _context.FindByIdAsync<Domain.Product>(request.ProductId, token);
            if(entity.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            var id = Guid.NewGuid();
            var result = await _imageService.HandleProductPictureAsync(entity, id, request.Picture, token);
            if (!result.Succeeded)
                return Failure(result.Exception);
            
            entity.AddPicture(new ProductPicture(id, result.Data));
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}