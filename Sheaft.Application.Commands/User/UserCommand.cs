using System;

namespace Sheaft.Application.Commands
{
    public abstract class UserCommand<T> : Command<T>
    {
        protected UserCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
    }
}
