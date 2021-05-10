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
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Returnable.Commands
{
    public class CreateReturnableCommand : Command<Guid>
    {
        protected CreateReturnableCommand()
        {
            
        }
        [JsonConstructor]
        public CreateReturnableCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = RequestUser.Id;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal Vat { get; set; }
        public Guid UserId { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
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
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.UserId, token);
            var returnable = new Domain.Returnable(Guid.NewGuid(), ReturnableKind.Container, producer, request.Name,
                request.WholeSalePrice, request.Vat, request.Description);

            await _context.AddAsync(returnable, token);
            await _context.SaveChangesAsync(token);

            return Success(returnable.Id);
        }
    }
}