using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Legal.Commands
{
    public class CheckConsumerLegalConfigurationCommand : Command
    {
        protected CheckConsumerLegalConfigurationCommand()
        {
            
        }
        [JsonConstructor]
        public CheckConsumerLegalConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }

    public class CheckConsumerLegalConfigurationCommandHandler : CommandsHandler,
        IRequestHandler<CheckConsumerLegalConfigurationCommand, Result>
    {
        private readonly IPspService _pspService;

        public CheckConsumerLegalConfigurationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<CheckConsumerLegalConfigurationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(CheckConsumerLegalConfigurationCommand request, CancellationToken token)
        {
            var legal = await _context.Set<ConsumerLegal>().SingleOrDefaultAsync(b => b.User.Id == request.UserId, token);
            if (string.IsNullOrWhiteSpace(legal.User.Identifier))
            {
                var userResult = await _pspService.CreateConsumerAsync(legal, token);
                if (!userResult.Succeeded)
                    return Failure(userResult);

                legal.User.SetIdentifier(userResult.Data);
                await _context.SaveChangesAsync(token);
            }

            return Success();
        }
    }
}