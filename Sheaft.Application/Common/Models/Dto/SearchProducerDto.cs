using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class SearchProducerDto
    {
        public Guid Producer_id { get; set; }
        public string Producer_name { get; set; }
        public string Producer_line1 { get; set; }
        public string Producer_line2 { get; set; }
        public string Producer_zipcode { get; set; }
        public string Producer_city { get; set; }
        public string Producer_email { get; set; }
        public string Producer_phone { get; set; }
        public decimal Producer_longitude { get; set; }
        public decimal Producer_latitude { get; set; }
        public string Producer_picture { get; set; }
        public int Producer_products_count { get; set; }
        public IEnumerable<string> Producer_tags { get; set; }
    }

}