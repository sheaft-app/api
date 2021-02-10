using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckBusinessLegalConfigurationCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckBusinessLegalConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
    public class CheckBusinessLegalConfigurationCommandHandler : CommandsHandler,
           IRequestHandler<CheckBusinessLegalConfigurationCommand, Result<bool>>
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

        public async Task<Result<bool>> Handle(CheckBusinessLegalConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(b => b.User.Id == request.UserId, token);
                if (string.IsNullOrWhiteSpace(legal.User.Identifier))
                {
                    var userResult = await _pspService.CreateBusinessAsync(legal, token);
                    if (!userResult.Success)
                        return Failed<bool>(userResult.Exception);

                    legal.User.SetIdentifier(userResult.Data);
                    await _context.SaveChangesAsync(token);
                }

                return Ok(true);
            });
        }
    }
}
