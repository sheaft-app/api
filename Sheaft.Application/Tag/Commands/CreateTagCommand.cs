using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Tag.Commands
{
    public class CreateTagCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateTagCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public TagKind Kind { get; set; }
        public string Picture { get; set; }
        public string Icon { get; set; }
        public bool Available { get; set; }
    }
    
    public class CreateTagCommandHandler : CommandsHandler,
        IRequestHandler<CreateTagCommand, Result<Guid>>
    {
        public CreateTagCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateTagCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateTagCommand request, CancellationToken token)
        {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var entity = new Domain.Tag(Guid.NewGuid(), request.Kind, request.Name, request.Description, request.Picture);
                    entity.SetAvailable(request.Available);

                    await _context.AddAsync(entity, token);
                    await _context.SaveChangesAsync(token);

                    var imageResult = await _mediatr.Process(new UpdateTagPictureCommand(request.RequestUser) { TagId = entity.Id, Picture = request.Picture }, token);
                    if (!imageResult.Succeeded)
                        return Failure<Guid>(imageResult.Exception);

                    var iconResult = await _mediatr.Process(new UpdateTagIconCommand(request.RequestUser) { TagId = entity.Id, Icon = request.Icon }, token);
                    if (!iconResult.Succeeded)
                        return Failure<Guid>(iconResult.Exception);

                    await transaction.CommitAsync(token);
                    return Success(entity.Id);
                }
        }
    }
}
