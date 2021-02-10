﻿using Microsoft.Extensions.Options;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Options;

namespace Sheaft.Infrastructure.Services
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
            var fees = (amount * _pspOptions.Percent) + _pspOptions.FixedAmount;
            return fees + fees * _pspOptions.VatPercent;
        }
    }
}
