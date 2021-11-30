using System;

namespace Sheaft.Domain
{
    public class UserRole
    {
        protected UserRole()
        {
        }
        
        public UserRole(Guid roleId)
        {
            RoleId = roleId;
        }

        public Guid RoleId { get; }
        public Role Role { get; private set; }
    }
}