using System;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class Sender : PurchaseOrderUser
    {
        protected Sender() { }

        public Sender(User user) : base(user)
        {
            UserId = user.Id;
        }
        
        public Sender(User user, Company company) : base(company)
        {
            UserId = user.Id;
            CompanyId = company.Id;
        }
        
        public string Picture { get; private set; }
        public Guid UserId { get; private set; }
        public Guid? CompanyId { get; private set; }
    }
}