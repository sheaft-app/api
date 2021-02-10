using System;

namespace Sheaft.Application.Models
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public LevelDto Level { get; set; }
    }
}