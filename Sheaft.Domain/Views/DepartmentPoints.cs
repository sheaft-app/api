using System;

namespace Sheaft.Domain.Views
{
    public class DepartmentPoints
    {
        public Guid DepartmentId { get; set; }
        public Guid RegionId { get; set; }
        public string DepartmentName { get; set; }
        public string RegionName { get; set; }
        public string Code { get; set; }
        public int? Points { get; set; }
        public int Users { get; set; }
        public long Position { get; set; }
    }
}