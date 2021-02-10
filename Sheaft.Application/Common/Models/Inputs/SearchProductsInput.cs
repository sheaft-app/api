using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class SearchProductsInput : SearchTermsInput
    {
        public Guid? ProducerId { get; set; }
    }
}