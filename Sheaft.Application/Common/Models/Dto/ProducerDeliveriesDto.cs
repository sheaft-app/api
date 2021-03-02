﻿using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Dto
{
    public class ProducerDeliveriesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<DeliveryDto> Deliveries { get; set; }
    }
}