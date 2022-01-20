using System;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class Sender 
    {
        protected Sender() { }

        public Sender(User user)
        {
            Id = user.Id;
            Name = $"{user.Firstname} {user.Lastname}";
            Email = user.Email;
            Phone = user.Phone;
            CompanyId = user.CompanyId;
        }
        
        public Sender(User user, Company company)
        {
            Id = user.Id;
            Name = company.Name;
            Email = company.Email;
            Phone = company.Phone;
            CompanyId = company.Id;
        }
        
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public Guid Id { get; private set; }
        public Guid? CompanyId { get; private set; }
    }
}