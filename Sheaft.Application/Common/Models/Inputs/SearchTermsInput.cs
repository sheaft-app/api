using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class SearchTermsInput
    {
        public string Text { get; set; }
        public string Sort { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? MaxDistance { get; set; }
        public int? Page { get; set; }
        public int? Take { get; set; }
    }
}