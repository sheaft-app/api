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
    public class RateProductCommand : Command<bool>
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
        IRequestHandler<RateProductCommand, Result<bool>>
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
        
        public async Task<Result<bool>> Handle(RateProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);

                entity.AddRating(user, request.Value, request.Comment);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.RateProduct, UserId = request.RequestUser.Id });
                return Ok(true);
            });
        }
    }
}
