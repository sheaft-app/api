using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{

    public class AddressSuggestDto
    {
        public string Zipcode { get; set; }
        public string City { get; set; }
    }
}