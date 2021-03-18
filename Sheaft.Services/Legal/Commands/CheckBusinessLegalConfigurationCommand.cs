using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Services.Legal.Commands
{
    public class CheckBusinessLegalConfigurationCommand : Command
    {
        [JsonConstructor]
        public CheckBusinessLegalConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }

    public class CheckBusinessLegalConfigurationCommandHandler : CommandsHandler,
        IRequestHandler<CheckBusinessLegalConfigurationCommand, Result>
    {
        private readonly IPspService _pspService;

        public CheckBusinessLegalConfigurationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<CheckBusinessLegalConfigurationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(CheckBusinessLegalConfigurationCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(b => b.User.Id == request.UserId, token);
            if (string.IsNullOrWhiteSpace(legal.User.Identifier))
            {
                var userResult = await _pspService.CreateBusinessAsync(legal, token);
                if (!userResult.Succeeded)
                    return Failure(userResult.Exception);

                legal.User.SetIdentifier(userResult.Data);
                await _context.SaveChangesAsync(token);
            }

            return Success();
        }
    }
}