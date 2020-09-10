
using Sheaft.Interop.Enums;

namespace Sheaft.Models.ViewModels
{

    public class AddressViewModel
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public CountryIsoCode Country { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
