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
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Tag.Commands
{
    public class UpdateTagCommand : Command
    {
        [JsonConstructor]
        public UpdateTagCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TagId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TagKind Kind { get; set; }
        public string Picture { get; set; }
        public string Icon { get; set; }
        public bool Available { get; set; }
    }

    public class UpdateTagCommandHandler : CommandsHandler,
        IRequestHandler<UpdateTagCommand, Result>
    {
        public UpdateTagCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateTagCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateTagCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var entity = await _context.Tags.SingleAsync(e => e.Id == request.TagId, token);

                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetKind(request.Kind);
                entity.SetAvailable(request.Available);

                await _context.SaveChangesAsync(token);

                var imageResult = await _mediatr.Process(
                    new UpdateTagPictureCommand(request.RequestUser) {TagId = entity.Id, Picture = request.Picture},
                    token);
                if (!imageResult.Succeeded)
                    return Failure(imageResult);

                var iconResult = await _mediatr.Process(
                    new UpdateTagIconCommand(request.RequestUser) {TagId = entity.Id, Icon = request.Icon}, token);
                if (!iconResult.Succeeded)
                    return Failure(iconResult);

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}