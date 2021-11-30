using System.Collections.Generic;

namespace Sheaft.Application.Configurations
{
    public class CorsConfiguration
    {
        public const string SETTING = "Cors";
        public IEnumerable<string> Origins { get; set; }
        public IEnumerable<string> Methods { get; set; }
        public IEnumerable<string> Headers { get; set; }
    }
}
