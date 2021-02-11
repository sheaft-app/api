using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Application.Legal.Commands;
using Sheaft.Application.Wallet.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Producer.Commands
{
    public class UpdateProducerProductsCommand : Command
    {
        [JsonConstructor]
        public UpdateProducerProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }

    public class UpdateProducerProductsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProducerProductsCommand, Result>
    {
        public UpdateProducerProductsCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<UpdateProducerProductsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateProducerProductsCommand request, CancellationToken token)
        {
            var producer = await _context.FindByIdAsync<Domain.Producer>(request.ProducerId, token);
            producer.HasProducts = await _context.Products.AnyAsync(p => p.Producer.Id == producer.Id, token);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}