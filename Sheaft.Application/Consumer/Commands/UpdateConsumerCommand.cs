using Newtonsoft.Json;
using Sheaft.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class UpdateConsumerCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
    }
    
    public class UpdateConsumerCommandHandler : CommandsHandler,
        IRequestHandler<UpdateConsumerCommand, Result<bool>>
    {
        private readonly RoleOptions _roleOptions;

        public UpdateConsumerCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateConsumerCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result<bool>> Handle(UpdateConsumerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var consumer = await _context.GetByIdAsync<Consumer>(request.Id, token);

                consumer.SetEmail(request.Email);
                consumer.SetPhone(request.Phone);
                consumer.SetFirstname(request.FirstName);
                consumer.SetLastname(request.LastName);

                await _context.SaveChangesAsync(token);

                var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
                {
                    UserId = consumer.Id,
                    Picture = request.Picture,
                    SkipAuthUpdate = true
                }, token);

                if (!resultImage.Success)
                    return Failed<bool>(resultImage.Exception);

                var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
                {
                    Email = consumer.Email,
                    FirstName = consumer.FirstName,
                    LastName = consumer.LastName,
                    Name = consumer.Name,
                    Phone = consumer.Phone,
                    Picture = consumer.Picture,
                    Roles = new List<Guid> { _roleOptions.Consumer.Id },
                    UserId = consumer.Id
                }, token);

                if (!authResult.Success)
                    return authResult;

                return Ok(true);
            });
        }
    }
}
