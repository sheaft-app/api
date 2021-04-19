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

namespace Sheaft.Mediatr.Returnable.Commands
{
    public class UpdateReturnableCommand : Command
    {
        protected UpdateReturnableCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateReturnableCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ReturnableId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal Vat { get; set; }
    }

    public class UpdateReturnableCommandHandler : CommandsHandler,
        IRequestHandler<UpdateReturnableCommand, Result>
    {
        public UpdateReturnableCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateReturnableCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateReturnableCommand request, CancellationToken token)
        {
            var entity = await _context.Returnables.SingleAsync(e => e.Id == request.ReturnableId, token);
            entity.SetName(request.Name);
            entity.SetDescription(request.Description);
            entity.SetWholeSalePrice(request.WholeSalePrice);
            entity.SetVat(request.Vat);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}