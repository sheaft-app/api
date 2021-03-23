using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class StoreViewModel: UserViewModel
    {
        public string Reason { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public IEnumerable<TimeSlotViewModel> OpeningHours { get; set; }
    }
}
