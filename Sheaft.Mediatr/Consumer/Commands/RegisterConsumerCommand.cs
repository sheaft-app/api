using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Mediatr.Auth.Commands;
using Sheaft.Mediatr.Sponsor.Commands;
using Sheaft.Mediatr.User.Commands;
using Sheaft.Options;

namespace Sheaft.Mediatr.Consumer.Commands
{
    public class RegisterConsumerCommand : Command<Guid>
    {
        [JsonConstructor]
        public RegisterConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ConsumerId { get; set; }
        public string SponsoringCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
    }

    public class RegisterConsumerCommandHandler : CommandsHandler,
        IRequestHandler<RegisterConsumerCommand, Result<Guid>>
    {
        private readonly RoleOptions _roleOptions;

        public RegisterConsumerCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RegisterConsumerCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result<Guid>> Handle(RegisterConsumerCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var consumer =
                    await _context.FindSingleAsync<Domain.Consumer>(
                        r => r.Id == request.ConsumerId || r.Email == request.Email, token);
                if (consumer != null)
                    return Failure<Guid>(MessageKind.Register_User_AlreadyExists);

                consumer = new Domain.Consumer(request.ConsumerId, request.Email, request.FirstName, request.LastName,
                    request.Phone);
                await _context.AddAsync(consumer, token);
                await _context.SaveChangesAsync(token);

                var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
                {
                    UserId = consumer.Id,
                    Picture = request.Picture,
                    SkipAuthUpdate = true
                }, token);

                if (!resultImage.Succeeded)
                    return Failure<Guid>(resultImage.Exception);

                var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
                {
                    Email = consumer.Email,
                    FirstName = consumer.FirstName,
                    LastName = consumer.LastName,
                    Name = consumer.Name,
                    Phone = consumer.Phone,
                    Picture = resultImage.Data,
                    Roles = new List<Guid> {_roleOptions.Consumer.Id},
                    UserId = consumer.Id
                }, token);

                if (!authResult.Succeeded)
                    return Failure<Guid>(authResult.Exception);

                await transaction.CommitAsync(token);

                if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                {
                    _mediatr.Post(new CreateSponsoringCommand(request.RequestUser)
                        {Code = request.SponsoringCode, UserId = consumer.Id});
                }

                return Success(consumer.Id);
            }
        }
    }
}