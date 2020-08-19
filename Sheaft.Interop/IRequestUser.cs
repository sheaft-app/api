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
        public IImpersonification ImpersonifiedBy { get; }
        public string RequestId { get; }
        bool IsAuthenticated { get; }
        bool IsConsumer { get; }
        bool IsImpersonating { get; }
    }

    public interface IImpersonification
    {
        public Guid Id { get; }
        public string Name { get; }
    }
}