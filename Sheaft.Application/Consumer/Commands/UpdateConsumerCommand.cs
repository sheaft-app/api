using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Auth.Commands;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Application.Picture.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Consumer.Commands
{
    public class UpdateConsumerCommand : Command
    {
        [JsonConstructor]
        public UpdateConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ConsumerId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
    }

    public class UpdateConsumerCommandHandler : CommandsHandler,
        IRequestHandler<UpdateConsumerCommand, Result>
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

        public async Task<Result> Handle(UpdateConsumerCommand request, CancellationToken token)
        {
            var consumer = await _context.GetByIdAsync<Domain.Consumer>(request.ConsumerId, token);
            if(consumer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

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

            if (!resultImage.Succeeded)
                return Failure(resultImage.Exception);

            var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
            {
                Email = consumer.Email,
                FirstName = consumer.FirstName,
                LastName = consumer.LastName,
                Name = consumer.Name,
                Phone = consumer.Phone,
                Picture = consumer.Picture,
                Roles = new List<Guid> {_roleOptions.Consumer.Id},
                UserId = consumer.Id
            }, token);

            if (!authResult.Succeeded)
                return authResult;

            return Success();
        }
    }
}