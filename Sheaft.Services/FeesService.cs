using Microsoft.Extensions.Options;
using Sheaft.Options;

namespace Sheaft.Services
{
    public class FeesService : IFeesService
    {
        private readonly PspOptions _pspOptions;

        public FeesService(IOptionsSnapshot<PspOptions> pspOptions)
        {
            _pspOptions = pspOptions.Value;
        }

        public decimal GetFees(decimal amount)
        {
            return amount * _pspOptions.Percent + _pspOptions.FixedAmount;
        }
    }
}
