﻿using System;

namespace Sheaft.Web.Manage.Models
{
    public class RegionViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ProducersCount { get; set; }
        public int? RequiredProducers { get; set; }
        public int StoresCount { get; set; }
        public int ConsumersCount { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
    }
}
