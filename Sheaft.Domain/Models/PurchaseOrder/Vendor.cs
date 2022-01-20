using System;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class Vendor
    {
        protected Vendor() { }

        public Vendor(Company vendor)
        {
            Id = vendor.Id;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
    }
}