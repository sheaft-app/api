using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class UpdateOrderInput : OrderInput
    {
        public Guid Id { get; set; } 
        public Guid? UserId { get; set; } 
    }
}