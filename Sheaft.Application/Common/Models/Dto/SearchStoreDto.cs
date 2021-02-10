using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Dto
{
    public class SearchStoreDto
    {
        public Guid Store_id { get; set; }
        public string Store_name { get; set; }
        public string Store_line1 { get; set; }
        public string Store_line2 { get; set; }
        public string Store_zipcode { get; set; }
        public string Store_city { get; set; }
        public string Store_email { get; set; }
        public string Store_phone { get; set; }
        public string Store_longitude { get; set; }
        public string Store_latitude { get; set; }
        public string Store_picture { get; set; }
        public IEnumerable<string> Store_tags { get; set; }
    }

}