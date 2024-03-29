﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Tag.Commands
{
    public class UpdateTagPictureCommand : Command<string>
    {
        protected UpdateTagPictureCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateTagPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TagId { get; set; }
        public string Picture { get; set; }
    }

    public class UpdateTagPictureCommandHandler : CommandsHandler,
        IRequestHandler<UpdateTagPictureCommand, Result<string>>
    {
        private readonly IPictureService _imageService;

        public UpdateTagPictureCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<UpdateTagPictureCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result<string>> Handle(UpdateTagPictureCommand request, CancellationToken token)
        {
            var entity = await _context.Tags.SingleAsync(e => e.Id == request.TagId, token);

            var resultImage = await _imageService.HandleTagPictureAsync(entity, request.Picture, token);
            if (!resultImage.Succeeded)
                return Failure<string>(resultImage);

            entity.SetPicture(resultImage.Data);
            await _context.SaveChangesAsync(token);

            return Success<string>(resultImage.Data);
        }
    }
}