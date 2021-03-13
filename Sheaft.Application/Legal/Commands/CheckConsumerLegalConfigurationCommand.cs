using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;

namespace Sheaft.Application.Legal.Commands
{
    public class CheckConsumerLegalConfigurationCommand : Command
    {
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
            var legal = await _context.GetSingleAsync<ConsumerLegal>(b => b.User.Id == request.UserId, token);
            if (string.IsNullOrWhiteSpace(legal.User.Identifier))
            {
                var userResult = await _pspService.CreateConsumerAsync(legal, token);
                if (!userResult.Succeeded)
                    return Failure(userResult.Exception);

                legal.User.SetIdentifier(userResult.Data);
                await _context.SaveChangesAsync(token);
            }

            return Success();
        }
    }
}