using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Producer.Commands
{
    public class UpdateProducerTagsCommand : Command
    {
        protected UpdateProducerTagsCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateProducerTagsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }

    public class UpdateProducerTagsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProducerTagsCommand, Result>
    {
        public UpdateProducerTagsCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<UpdateProducerTagsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateProducerTagsCommand request, CancellationToken token)
        {
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);

            var productTags = await _context.Products.Where(p => p.ProducerId == producer.Id).SelectMany(p => p.Tags)
                .Select(p => p.Tag).Distinct().ToListAsync(token);
            producer.SetTags(productTags);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}