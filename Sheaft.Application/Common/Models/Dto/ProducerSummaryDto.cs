﻿using System;

namespace Sheaft.Application.Common.Models.Dto
{
    public class ProducerSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public AddressDto Address { get; set; }
    }
}