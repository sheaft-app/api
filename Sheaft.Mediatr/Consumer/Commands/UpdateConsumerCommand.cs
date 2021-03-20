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
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Auth.Commands;
using Sheaft.Mediatr.User.Commands;
using Sheaft.Options;

namespace Sheaft.Mediatr.Consumer.Commands
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
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
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
            
            consumer.ProfileInformation.SetSummary(request.Summary);
            consumer.ProfileInformation.SetDescription(request.Description);
            consumer.ProfileInformation.SetFacebook(request.Facebook);
            consumer.ProfileInformation.SetTwitter(request.Twitter);
            consumer.ProfileInformation.SetWebsite(request.Instagram);
            consumer.ProfileInformation.SetInstagram(request.Website);

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