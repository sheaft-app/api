using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class AgreementViewModel : ShortAgreementViewModel
    {
        public ShortDeliveryModeViewModel DeliveryMode { get; set; }
        public ShortCatalogViewModel Catalog { get; set; }
    }
}
