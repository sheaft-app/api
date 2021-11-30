namespace Sheaft.Domain.BaseClass
{
    public abstract class PurchaseOrderUser
    {
        protected PurchaseOrderUser()
        {
        }

        protected PurchaseOrderUser(User user)
        {
            Name = user.Name;
            Email = user.Email;
            Phone = user.Phone;
        }

        protected PurchaseOrderUser(Company company)
        {
            Name = company.Name;
            Email = company.Email;
            Phone = company.Phone;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
    }
}