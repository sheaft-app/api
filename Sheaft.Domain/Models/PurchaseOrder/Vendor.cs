using System;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class Vendor : PurchaseOrderUser
    {
        protected Vendor() { }

        public Vendor(Company vendor) : base(vendor)
        {
            CompanyId = vendor.Id;
        }

        public Guid CompanyId { get; private set; }
    }
}