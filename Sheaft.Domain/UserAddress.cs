using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public class UserAddress : BaseAddress
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