using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class PointsDto
    {
        public DateTimeOffset CreatedOn { get; set; }
        public PointKind Kind { get; set; }
        public int Quantity { get; set; }
    }

    public class OwnerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset Birthdate { get; set; }
        public OwnerKind Kind { get; set; }
        public string Nationality { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}