using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{

    public class ProducerSuggestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressSuggestDto Address { get; set; }
    }
}