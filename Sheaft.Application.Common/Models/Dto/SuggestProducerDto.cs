using System;

namespace Sheaft.Application.Common.Models.Dto
{

    public class SuggestProducerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SuggestAddressDto Address { get; set; }
    }
}