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
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.Mediatr.Product.Commands
{
    public class RateProductCommand : Command
    {
        protected RateProductCommand()
        {
            
        }
        [JsonConstructor]
        public RateProductCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = RequestUser.Id;
        }

        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
    }

    public class RateProductCommandHandler : CommandsHandler,
        IRequestHandler<RateProductCommand, Result>
    {
        public RateProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RateProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RateProductCommand request, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            var entity = await _context.Products.SingleAsync(e => e.Id == request.ProductId, token);

            entity.AddRating(user, request.Value, request.Comment);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}