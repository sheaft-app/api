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
    public class DeleteAgreementCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    public class DeleteAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<DeleteAgreementCommand, Result<bool>>
    {
        public DeleteAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(DeleteAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Agreement>(request.Id, token);

                _context.Remove(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}
