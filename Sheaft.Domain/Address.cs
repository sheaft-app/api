using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Address
    {
        protected Address()
        {
        }

        public Address(Department department)
        {
            Department = department;
        }

        public Address(string line1, string line2, string zipcode, string city, Department department, double? longitude = null, double? latitude = null)
        {
            if (string.IsNullOrWhiteSpace(line1))
                throw new ValidationException(MessageKind.Address_Line1_Required);

            if (string.IsNullOrWhiteSpace(zipcode))
                throw new ValidationException(MessageKind.Address_Zipcode_Required);

            if (string.IsNullOrWhiteSpace(city))
                throw new ValidationException(MessageKind.Address_City_Required);

            Line1 = line1;
            Line2 = line2;
            Zipcode = zipcode;
            City = city;
            Longitude = longitude;
            Latitude = latitude;
            Department = department;
        }

        public string Line1 { get; private set; }
        public string Line2 { get; private set; }
        public string Zipcode { get; private set; }
        public string City { get; private set; }
        public double? Longitude { get; private set; }
        public double? Latitude { get; private set; }
        public virtual Department Department { get; private set; }

        public static string GetDepartmentCode(string zipcode)
        {
            var departmentCode = zipcode.Substring(0, 2);
            if (departmentCode == "20" && int.Parse(zipcode) < 20200)
                departmentCode = "2A";

            if (departmentCode == "20" && int.Parse(zipcode) >= 20200)
                departmentCode = "2B";

            return departmentCode;
        }
    }
}