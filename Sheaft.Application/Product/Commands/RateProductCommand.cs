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

        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
    }

    public class RateProductCommandHandler : CommandsHandler,
        IRequestHandler<RateProductCommand, Result>
    {
        private readonly IBlobService _blobService;

        public RateProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<RateProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result> Handle(RateProductCommand request, CancellationToken token)
        {
            var user = await _context.GetByIdAsync<Domain.User>(request.RequestUser.Id, token);
            var entity = await _context.GetByIdAsync<Domain.Product>(request.Id, token);

            entity.AddRating(user, request.Value, request.Comment);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(new CreateUserPointsCommand(request.RequestUser)
            {
                CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.RateProduct, UserId = request.RequestUser.Id
            });
            
            return Success();
        }
    }
}