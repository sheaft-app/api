using System;
using System.Collections.Generic;

namespace Sheaft.Web.Manage.Models
{
    public class ProducerViewModel : UserViewModel
    {
        public bool OpenForNewBusiness { get; set; }
        public List<Guid> Tags { get; set; }
        public bool NotSubjectToVat { get; set; }
        public List<ClosingViewModel> Closings { get; set; }
    }
}
