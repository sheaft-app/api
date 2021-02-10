using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Returnable.Commands
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
            var producer = await _context.GetByIdAsync<Domain.Producer>(request.RequestUser.Id, token);
            var returnable = new Domain.Returnable(Guid.NewGuid(), ReturnableKind.Container, producer, request.Name,
                request.WholeSalePrice, request.Vat, request.Description);

            await _context.AddAsync(returnable, token);
            await _context.SaveChangesAsync(token);

            return Success(returnable.Id);
        }
    }
}