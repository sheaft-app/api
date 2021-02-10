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
    public class UpdateReturnableCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateReturnableCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal Vat { get; set; }
    }
    
    public class UpdateReturnableCommandHandler : CommandsHandler,
        IRequestHandler<UpdateReturnableCommand, Result<bool>>
    {
        public UpdateReturnableCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateReturnableCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(UpdateReturnableCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Returnable>(request.Id, token);
                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetWholeSalePrice(request.WholeSalePrice);
                entity.SetVat(request.Vat);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
