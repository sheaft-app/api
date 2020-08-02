using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class PurchaseOrderUser
    {
        protected PurchaseOrderUser()
        {
        }

        protected PurchaseOrderUser(User user): this(user.Company)
        {
            if (user.Company != null)
                return;

            Id = user.Id;
            Name = user.FirstName + " " + user.LastName;
            Kind = ProfileKind.Consumer;
            Email = user.Email;
            Phone = user.Phone;
            Picture = user.Picture;
        }

        protected PurchaseOrderUser(Company company)
        {
            if (company == null)
                return;

            Id = company.Id;
            Name = company.Name;
            Kind = company.Kind;
            Email = company.Email;
            Phone = company.Phone;
            Picture = company.Picture;
        }

        public Guid Id { get; private set; }
        public ProfileKind Kind { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Picture { get; private set; }
    }

    public class PurchaseOrderVendor : PurchaseOrderUser
    {
        protected PurchaseOrderVendor() { }

        public PurchaseOrderVendor(User user) : base(user)
        {
        }
        public PurchaseOrderVendor(Company company) : base(company)
        {
        }
    }

    public class PurchaseOrderSender : PurchaseOrderUser
    {
        protected PurchaseOrderSender() { }

        public PurchaseOrderSender(User user) : base(user)
        {
        }
    }
}