using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class LevelDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int RequiredPoints { get; set; }
        public IEnumerable<RewardDto> Rewards { get; set; }
    }
}