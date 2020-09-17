using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Consumer : User
    {
        protected Consumer()
        {
        }

        public Consumer(Guid id, string email, string firstname, string lastname, string phone = null)
            : base(id, ProfileKind.Consumer, $"{firstname} {lastname}", firstname, lastname, email, phone)
        {
            Anonymous = false;
        }

        public bool Anonymous { get; private set; }

        public void SetAnonymous(bool anonymous)
        {
            Anonymous = anonymous;
        }
    }
}