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
    public class UpdateJobCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public class UpdateJobCommandHandler : CommandsHandler,
        IRequestHandler<UpdateJobCommand, Result<bool>>
    {
        public UpdateJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<UpdateJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(UpdateJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.SetName(request.Name);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
