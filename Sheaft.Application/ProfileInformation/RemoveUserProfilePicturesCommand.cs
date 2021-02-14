﻿using System;
using System.Collections.Generic;
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

namespace Sheaft.Application.User.Commands
{
    public class RemoveUserProfilePicturesCommand : Command
    {
        [JsonConstructor]
        public RemoveUserProfilePicturesCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid UserId { get; set; }
        public List<Guid> PictureIds { get; set; }
    }

    public class RemoveUserProfilePicturesCommandHandler : CommandsHandler,
        IRequestHandler<RemoveUserProfilePicturesCommand, Result>
    {
        public RemoveUserProfilePicturesCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RemoveUserProfilePicturesCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RemoveUserProfilePicturesCommand request, CancellationToken token)
        {
        }
    }
}