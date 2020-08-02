using System;
using System.Collections.Generic;

namespace Sheaft.Interop
{
    public interface IRequestUser
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public IEnumerable<string> Roles { get; }
        public Guid CompanyId { get; }
        public string RequestId { get; }
        bool IsAuthenticated { get; }
        bool IsConsumer { get; }
    }
}