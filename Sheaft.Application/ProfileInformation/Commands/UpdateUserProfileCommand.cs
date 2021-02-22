﻿using System;
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
using Sheaft.Domain;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.ProfileInformation.Commands
{
    public class UpdateUserProfileCommand : Command
    {
        [JsonConstructor]
        public UpdateUserProfileCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
    }

    public class UpdateUserProfileCommandHandler : CommandsHandler,
        IRequestHandler<UpdateUserProfileCommand, Result>
    {
        public UpdateUserProfileCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateUserProfileCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateUserProfileCommand request, CancellationToken token)
        {
            var entity = await _context.FindByIdAsync<Domain.User>(request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.ProfileInformation.SetSummary(request.Summary);
            entity.ProfileInformation.SetDescription(request.Description);
            entity.ProfileInformation.SetFacebook(request.Facebook);
            entity.ProfileInformation.SetTwitter(request.Facebook);
            entity.ProfileInformation.SetWebsite(request.Facebook);
            entity.ProfileInformation.SetInstagram(request.Facebook);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}