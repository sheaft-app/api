﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.User.Commands
{
    public class GenerateUserCodeCommand : Command<string>
    {
        [JsonConstructor]
        public GenerateUserCodeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }

    public class GenerateUserCodeCommandHandler : CommandsHandler,
        IRequestHandler<GenerateUserCodeCommand, Result<string>>
    {
        private readonly IIdentifierService _identifierService;

        public GenerateUserCodeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IIdentifierService identifierService,
            ILogger<GenerateUserCodeCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
        }

        public async Task<Result<string>> Handle(GenerateUserCodeCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.User>(request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            if (!string.IsNullOrWhiteSpace(entity.SponsorshipCode))
                return Success(entity.SponsorshipCode);

            var result = await _identifierService.GetNextSponsoringCode(token);
            if (!result.Succeeded)
                return Failure<string>(result.Exception);

            entity.SetSponsoringCode(result.Data);

            await _context.SaveChangesAsync(token);
            return Success(entity.SponsorshipCode);
        }
    }
}