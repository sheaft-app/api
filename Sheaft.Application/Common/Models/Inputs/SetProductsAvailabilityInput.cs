﻿using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class SetProductsAvailabilityInput
    {
        public IEnumerable<Guid> Ids { get; set; }
        public bool Available { get; set; }
    }
}