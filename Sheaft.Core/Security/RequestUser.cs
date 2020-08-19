using Sheaft.Interop;
using System;
using System.Collections.Generic;

namespace Sheaft.Core.Security
{
    public class RequestUser : IRequestUser
    {
        public RequestUser():this(Guid.NewGuid().ToString("N"))
        {
        }

        public RequestUser(string requestId, Guid? impersonifiedById = null, string impersonifiedByName = null) : this(Guid.Empty, "Anonymous", null, null, null, requestId, impersonifiedById, impersonifiedByName)
        {
        }

        public RequestUser(string name, string requestId, Guid? impersonifiedById = null, string impersonifiedByName = null) : this(Guid.Empty, name, null, null, null, requestId, impersonifiedById, impersonifiedByName)
        {
        }

        public RequestUser(Guid? id, string name, string email, IEnumerable<string> roles, Guid? companyId = null, string requestId = null, Guid? impersonifiedById = null, string impersonifiedByName = null)
        {
            Id = id ?? Guid.Empty;
            Name = name ?? string.Empty;
            Email = email ?? string.Empty;
            Roles = roles ?? new List<string>();
            CompanyId = companyId ?? Guid.Empty;
            RequestId = requestId ?? Guid.NewGuid().ToString("N");
            ImpersonifiedBy = impersonifiedById.HasValue && impersonifiedById != Guid.Empty ? new Impersonification(impersonifiedById.Value, impersonifiedByName) : null;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public IEnumerable<string> Roles { get; }
        public Guid CompanyId { get; }
        public IImpersonification ImpersonifiedBy { get; }
        public string RequestId { get; }

        public bool IsAuthenticated => Id != Guid.Empty;
        public bool IsConsumer => CompanyId == Guid.Empty;
        public bool IsImpersonating => ImpersonifiedBy != null;
    }

    public class Impersonification : IImpersonification
    {
        public Impersonification(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}