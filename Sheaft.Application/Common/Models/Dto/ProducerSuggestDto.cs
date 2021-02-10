using System;

namespace Sheaft.Application.Common.Models.Dto
{

    public class ProducerSuggestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressSuggestDto Address { get; set; }
    }
}