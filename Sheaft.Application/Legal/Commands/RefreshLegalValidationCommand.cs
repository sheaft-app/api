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
using Sheaft.Application.Withholding.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Legal.Commands
{
    public class RefreshLegalValidationCommand : Command
    {
        [JsonConstructor]
        public RefreshLegalValidationCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshLegalValidationCommandHandler : CommandsHandler,
        IRequestHandler<RefreshLegalValidationCommand, Result>
    {
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;

        public RefreshLegalValidationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<RefreshLegalValidationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspOptions = pspOptions.Value;
            _pspService = pspService;
        }

        public async Task<Result> Handle(RefreshLegalValidationCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<Domain.Legal>(b => b.User.Identifier == request.Identifier, token);
            var validation = LegalValidation.NotSpecified;
            if (legal is BusinessLegal)
            {
                var userResult = await _pspService.GetCompanyAsync(request.Identifier, token);
                if (!userResult.Succeeded)
                    return Failure(userResult.Exception);

                validation = userResult.Data.KYCLevel;
            }
            else
            {
                var userResult = await _pspService.GetConsumerAsync(request.Identifier, token);
                if (!userResult.Succeeded)
                    return Failure(userResult.Exception);

                validation = userResult.Data.KYCLevel;
            }

            legal.SetValidation(validation);
            await _context.SaveChangesAsync(token);

            if (validation == LegalValidation.Regular && legal.User.Kind == ProfileKind.Producer)
                _mediatr.Post(new CreateWithholdingCommand(request.RequestUser)
                {
                    UserId = legal.User.Id,
                    Amount = _pspOptions.ProducerFees
                });

            return Success();
        }
    }
}