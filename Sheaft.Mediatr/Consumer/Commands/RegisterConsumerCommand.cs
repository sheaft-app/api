using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.Mediatr.Auth.Commands;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.Mediatr.Consumer.Commands
{
    public class RegisterConsumerCommand : Command<Guid>
    {
        protected RegisterConsumerCommand()
        {
        }

        [JsonConstructor]
        public RegisterConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
            ConsumerId = RequestUser.Id;
        }

        public Guid ConsumerId { get; set; }
        public string SponsoringCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            ConsumerId = RequestUser.Id;
        }
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
            var consumer = await _context.Consumers
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == request.ConsumerId || r.Email == request.Email, token);

            if (consumer is { RemovedOn: null })
                return Failure<Guid>("Un compte existe déjà avec ces informations.");

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                if (consumer != null)
                {
                    _context.Restore(consumer);
                }
                else
                {
                    consumer = new Domain.Consumer(request.ConsumerId, request.Email, request.FirstName,
                        request.LastName,
                        request.Phone);

                    await _context.AddAsync(consumer, token);
                }

                await _context.SaveChangesAsync(token);

                var resultImage = await _mediatr.Process(new UpdateUserPreviewCommand(request.RequestUser)
                {
                    UserId = consumer.Id,
                    Picture = request.Picture,
                    SkipAuthUpdate = true
                }, token);

                if (!resultImage.Succeeded)
                    return Failure<Guid>(resultImage);

                var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
                {
                    Email = consumer.Email,
                    FirstName = consumer.FirstName,
                    LastName = consumer.LastName,
                    Name = consumer.Name,
                    Phone = consumer.Phone,
                    Picture = resultImage.Data,
                    Roles = new List<Guid> { _roleOptions.Consumer.Id },
                    UserId = consumer.Id
                }, token);

                if (!authResult.Succeeded)
                    return Failure<Guid>(authResult);

                await transaction.CommitAsync(token);

                if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                {
                    _mediatr.Post(new CreateSponsoringCommand(request.RequestUser)
                        { Code = request.SponsoringCode, UserId = consumer.Id });
                }

                return Success(consumer.Id);
            }
        }
    }
}