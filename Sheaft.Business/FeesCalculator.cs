using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Options;

namespace Sheaft.Business
{
    public class FeesCalculator : IFeesCalculator
    {
        private readonly PspOptions _pspOptions;

        public FeesCalculator(IOptionsSnapshot<PspOptions> pspOptions)
        {
            _pspOptions = pspOptions.Value;
        }

        public decimal GetFees(decimal amount)
        {
            var fees = (amount * _pspOptions.Percent) + _pspOptions.FixedAmount;
            return fees + fees * _pspOptions.VatPercent;
        }
    }
}
