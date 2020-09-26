using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{

    public class DonationCommandsHandler : ResultsHandler
    {
        private readonly IPspService _pspService;

        public DonationCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<DonationCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }
    }
}