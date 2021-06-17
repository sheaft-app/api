using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class PurchaseOrderUserDto
    {
        public Guid Id { get; set; }
        public ProfileKind Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public string Address { get; set; }
    }
}