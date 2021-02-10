using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class UpdateTagCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateTagCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TagKind Kind { get; set; }
        public string Picture { get; set; }
        public string Icon { get; set; }
        public bool Available { get; set; }
    }
    
    public class UpdateTagCommandHandler : CommandsHandler,
        IRequestHandler<UpdateTagCommand, Result<bool>>
    {
        public UpdateTagCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateTagCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(UpdateTagCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var entity = await _context.GetByIdAsync<Tag>(request.Id, token);

                    entity.SetName(request.Name);
                    entity.SetDescription(request.Description);
                    entity.SetKind(request.Kind);
                    entity.SetAvailable(request.Available);

                    await _context.SaveChangesAsync(token);

                    var imageResult = await _mediatr.Process(new UpdateTagPictureCommand(request.RequestUser) { TagId = entity.Id, Picture = request.Picture }, token);
                    if (!imageResult.Success)
                        return Failed<bool>(imageResult.Exception);

                    var iconResult = await _mediatr.Process(new UpdateTagIconCommand(request.RequestUser) { TagId = entity.Id, Icon = request.Icon }, token);
                    if (!iconResult.Success)
                        return Failed<bool>(iconResult.Exception);

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }
    }
}
