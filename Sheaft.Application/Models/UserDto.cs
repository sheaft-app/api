using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public ProfileKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public AddressDto Address { get; set; }
    }

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