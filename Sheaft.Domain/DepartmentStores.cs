using System;

namespace Sheaft.Domain.Views
{
    public class DepartmentStores
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public Guid RegionId { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public int? Active { get; set; }
        public int? Created { get; set; }
    }
}