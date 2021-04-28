using System;
using System.Text.RegularExpressions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class UserAddress : Address
    {
        protected UserAddress()
        {
        }

        public UserAddress(Department department)
        {
            Department = department;
        }

        public UserAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country, Department department, double? longitude = null, double? latitude = null)
            : base(line1, line2, zipcode, city, country)
        {
            Department = department;
            Longitude = longitude;
            Latitude = latitude;
        }

        public double? Longitude { get; private set; }
        public double? Latitude { get; private set; }
        public Guid DepartmentId { get; set; }
        public virtual Department Department { get; private set; }

        public static string GetDepartmentCode(string code)
        {
            if (code == null || code.Length < 2)
                return null;

            var corsicaRegex = new Regex("^20[0-9]{3}");
            if (corsicaRegex.Match(code).Success)
            {
                var departmentCode = int.Parse(code);
                if (departmentCode < 20200)
                    return "2A";

                if (departmentCode >= 20200)
                    return "2B";
            }

            var regex = new Regex("(^([2[A-B]{2})|^([0-9]{2}))([0-9]{0,3})");
            if (regex.Match(code).Success)
                return code.Substring(0, 2);
            
            return null;
        }

        public override string ToString()
        {
            return $"{Line1}\n{Line2}\n{Zipcode},{City}";
        }
    }
}