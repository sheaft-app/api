using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class RewardViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Url { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid LevelId { get; set; }
    }
}
