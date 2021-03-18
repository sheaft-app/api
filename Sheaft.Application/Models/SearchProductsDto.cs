using System;

namespace Sheaft.Application.Models
{
    public class SearchProductsDto : SearchTermsDto
    {
        public Guid? ProducerId { get; set; }
    }
}