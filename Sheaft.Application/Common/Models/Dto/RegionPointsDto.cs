﻿using System;

namespace Sheaft.Application.Common.Models.Dto
{
    public class RegionPointsDto
    {
        public Guid RegionId { get; set; }
        public string RegionName { get; set; }
        public int? Points { get; set; }
        public int Users { get; set; }
        public long Position { get; set; }
    }
}