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
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;

namespace Sheaft.Application.User.Commands
{
    public class AddPictureToUserProfileCommand : Command
    {
        [JsonConstructor]
        public AddPictureToUserProfileCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid UserId { get; set; }
        public PictureInput Picture { get; set; }
    }

    public class AddPictureToUserProfileCommandHandler : CommandsHandler,
        IRequestHandler<AddPictureToUserProfileCommand, Result>
    {
        public AddPictureToUserProfileCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<AddPictureToUserProfileCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(AddPictureToUserProfileCommand request, CancellationToken token)
        {
        }
    }
}