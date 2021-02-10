using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;

namespace Sheaft.Application.Legal.Commands
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
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;

        public CheckBusinessLegalConfigurationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CheckBusinessLegalConfigurationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspOptions = pspOptions.Value;
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