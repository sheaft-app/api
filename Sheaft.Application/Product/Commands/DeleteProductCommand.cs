using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class DeleteProductCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class DeleteProductCommandHandler : CommandsHandler,
        IRequestHandler<DeleteProductCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;

        public DeleteProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<DeleteProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);
                _context.Remove(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
