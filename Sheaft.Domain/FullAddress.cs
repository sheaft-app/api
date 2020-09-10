using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public class FullAddress : LocationAddress
    {
        protected FullAddress()
        {
        }

        public FullAddress(Department department)
        {
            Department = department;
        }

        public FullAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country, Department department, double? longitude = null, double? latitude = null)
            : base(line1, line2, zipcode, city, country, longitude, latitude)
        {
            Department = department;
        }

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