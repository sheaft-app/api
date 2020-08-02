using System;

namespace Sheaft.Models.Dto
{
    public class DepartmentUserPointsDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public Guid DepartmentId { get; set; }
        public int? Points { get; set; }
        public long Position { get; set; }
    }
}