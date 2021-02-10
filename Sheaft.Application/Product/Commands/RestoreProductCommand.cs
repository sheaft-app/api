using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Product.Commands
{
    public class RestoreProductCommand : Command
    {
        [JsonConstructor]
        public RestoreProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class RestoreProductCommandHandler : CommandsHandler,
        IRequestHandler<RestoreProductCommand, Result>
    {
        private readonly IBlobService _blobService;

        public RestoreProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<RestoreProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result> Handle(RestoreProductCommand request, CancellationToken token)
        {
            var entity =
                await _context.Products.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
            _context.Restore(entity);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}