using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class UpdateOrderInput : OrderInput
    {
        public Guid Id { get; set; } 
    }
}