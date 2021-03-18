using System;
using System.Collections.Generic;

namespace Sheaft.Web.Manage.Models
{
    public class ProducerViewModel : UserViewModel
    {
        public bool OpenForNewBusiness { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public bool NotSubjectToVat { get; set; }
    }
}
