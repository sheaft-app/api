using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Sheaft.Domain
{
    public interface ITrackedUser
    {
        public RequestUser RequestUser { get; }
    }

    public class RequestUser
    {
        public RequestUser() : this(Guid.NewGuid().ToString("N"))
        {
        }

        public RequestUser(string requestId, Impersonification impersonification = null) : this(Guid.Empty, "Anonymous",
            null, null, requestId, impersonification)
        {
        }

        public RequestUser(string name, string requestId, Impersonification impersonification = null) : this(Guid.Empty,
            name, null, null, requestId, impersonification)
        {
        }

        [JsonConstructor]
        public RequestUser(Guid? id, string name, string email, List<string> roles, string requestId = null,
            Impersonification impersonifiedBy = null)
        {
            Id = id ?? Guid.Empty;
            Name = name ?? string.Empty;
            Email = email ?? string.Empty;
            Roles = roles?.ToList() ?? new List<string>();
            RequestId = requestId ?? Guid.NewGuid().ToString("N");
            ImpersonifiedBy = impersonifiedBy;
        }

        public Guid Id { get; set; }
        public string Name { get; set;}
        public string Email { get; set;}
        public List<string> Roles { get; set;}
        public Impersonification ImpersonifiedBy { get; set;}
        public string RequestId { get; set;}

        public bool IsAuthenticated => Id != Guid.Empty;
        public bool IsImpersonating => ImpersonifiedBy != null;
    }

    public class Impersonification
    {
        public Impersonification(Guid id, string name = null)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}