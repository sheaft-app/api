using System.Collections.Generic;

namespace Sheaft.Web.Manage.Models
{
    public class DeliveryModeViewModel : ShortDeliveryModeViewModel
    {
        public List<ClosingViewModel> Closings { get; set; }
        public List<TimeSlotViewModel> DeliveryHours { get; set; }
        public List<ShortAgreementViewModel> Agreements { get; set; }
    }
}
