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
using Sheaft.Application.User.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Product.Commands
{
    public class RateProductCommand : Command
    {
        [JsonConstructor]
        public RateProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
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
            var user = await _context.GetByIdAsync<Domain.User>(request.UserId, token);
            var entity = await _context.GetByIdAsync<Domain.Product>(request.ProductId, token);

            entity.AddRating(user, request.Value, request.Comment);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(new CreateUserPointsCommand(request.RequestUser)
            {
                CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.RateProduct, UserId = request.UserId
            });

            return Success();
        }
    }
}