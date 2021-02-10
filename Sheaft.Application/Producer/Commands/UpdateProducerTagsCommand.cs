using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class UpdateProducerTagsCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateProducerTagsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
    
    public class UpdateProducerTagsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProducerTagsCommand, Result<bool>>
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
        public async Task<Result<bool>> Handle(UpdateProducerTagsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.ProducerId, token);

                var productTags = await _context.Products.Get(p => p.Producer.Id == producer.Id).SelectMany(p => p.Tags).Select(p => p.Tag).Distinct().ToListAsync(token);
                producer.SetTags(productTags);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
