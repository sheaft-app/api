using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class SearchProductsInput : SearchTermsInput
    {
        public Guid? ProducerId { get; set; }
    }
}