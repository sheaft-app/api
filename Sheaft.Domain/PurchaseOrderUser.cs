using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public abstract class PurchaseOrderUser
    {
        protected PurchaseOrderUser()
        {
        }

        protected PurchaseOrderUser(User user)
        {
            Name = user.Name;
            Kind = user.Kind;
            Email = user.Email;
            Phone = user.Phone;
            Picture = user.Picture;
            Address = user.Address?.ToString();
        }

        public ProfileKind Kind { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Picture { get; private set; }
        public string Address { get; private set; }
    }

    public class PurchaseOrderVendor : PurchaseOrderUser
    {
        protected PurchaseOrderVendor() { }

        public PurchaseOrderVendor(Producer vendor) : base(vendor)
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