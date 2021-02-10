using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;

namespace Sheaft.Application.Producer.Commands
{
    public class UpdateProducerTagsCommand : Command
    {
        [JsonConstructor]
        public UpdateProducerTagsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }

    public class UpdateProducerTagsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProducerTagsCommand, Result>
    {
        private readonly RoleOptions _roleOptions;
        private readonly IBlobService _blobService;

        public UpdateProducerTagsCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IBlobService blobService,
            ILogger<UpdateProducerTagsCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _blobService = blobService;
        }

        public async Task<Result> Handle(UpdateProducerTagsCommand request, CancellationToken token)
        {
            var producer = await _context.GetByIdAsync<Domain.Producer>(request.ProducerId, token);

            var productTags = await _context.Products.Get(p => p.Producer.Id == producer.Id).SelectMany(p => p.Tags)
                .Select(p => p.Tag).Distinct().ToListAsync(token);
            producer.SetTags(productTags);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}