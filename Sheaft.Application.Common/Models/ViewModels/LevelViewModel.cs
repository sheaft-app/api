using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class LevelViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Name { get; set; }
        public int RequiredPoints { get; set; }
        public IEnumerable<LevelRewardViewModel> Rewards { get; set; }
    }
}
