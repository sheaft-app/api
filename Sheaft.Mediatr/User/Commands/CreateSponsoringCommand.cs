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

namespace Sheaft.Mediatr.User.Commands
{
    public class CreateSponsoringCommand : Command
    {
        protected CreateSponsoringCommand()
        {
            
        }
        [JsonConstructor]
        public CreateSponsoringCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Code { get; set; }
    }

    public class CreateSponsoringCommandHandler : CommandsHandler,
        IRequestHandler<CreateSponsoringCommand, Result>
    {
        public CreateSponsoringCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateSponsoringCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CreateSponsoringCommand request, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            var sponsor = await _context.Users.SingleOrDefaultAsync(u => u.SponsorshipCode == request.Code, token);
            if (sponsor == null)
                return Failure("Le sponsor est introuvable.");

            await _context.AddAsync(new Sponsoring(sponsor, user), token);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}