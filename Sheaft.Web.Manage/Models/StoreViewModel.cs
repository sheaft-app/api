using System;
using System.Collections.Generic;

namespace Sheaft.Web.Manage.Models
{
    public class StoreViewModel: UserViewModel
    {
        public string Reason { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public List<Guid> Tags { get; set; }
        public List<TimeSlotViewModel> OpeningHours { get; set; }
        public List<ClosingViewModel> Closings { get; set; }
    }
}
