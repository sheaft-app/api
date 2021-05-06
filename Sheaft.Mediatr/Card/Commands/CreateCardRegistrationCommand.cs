using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Card.Commands
{
    public class CreateCardRegistrationCommand : Command<CardRegistrationDto>
    {
        protected CreateCardRegistrationCommand()
        {
            
        }
        [JsonConstructor]
        public CreateCardRegistrationCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = RequestUser.Id;
        }

        public Guid UserId { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
    }
    
    public class CreateCardRegistrationCommandHandler : CommandsHandler,
        IRequestHandler<CreateCardRegistrationCommand, Result<CardRegistrationDto>>
    {
        private readonly IPspService _pspService;

        public CreateCardRegistrationCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IPspService pspService,
            ILogger<CreateCardRegistrationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<CardRegistrationDto>> Handle(CreateCardRegistrationCommand request,
            CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);

            var result = await _pspService.CreateCardRegistrationAsync(user, token);
            if (!result.Succeeded)
                return Failure<CardRegistrationDto>(result);

            return Success(new CardRegistrationDto
            {
                Identifier = result.Data.Id,
                AccessKey = result.Data.AccessKey,
                PreRegistrationData = result.Data.PreregistrationData,
                Url = result.Data.CardRegistrationURL
            });
        }
    }
}