using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class CreateReturnableCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateReturnableCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal Vat { get; set; }
    }
    
    public class CreateReturnableCommandHandler : CommandsHandler,
        IRequestHandler<CreateReturnableCommand, Result<Guid>>
    {
        public CreateReturnableCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateReturnableCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateReturnableCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.RequestUser.Id, token);
                var returnable = new Returnable(Guid.NewGuid(), ReturnableKind.Container, producer, request.Name, request.WholeSalePrice, request.Vat, request.Description);

                await _context.AddAsync(returnable, token);
                await _context.SaveChangesAsync(token);

                return Created(returnable.Id);
            });
        }
    }
}
