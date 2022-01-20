using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sheaft.Domain.Security;

namespace Sheaft.Domain.Common
{
    public interface ITrackedUser
    {
        public RequestUser RequestUser { get; }
    }

    public class RequestUser
    {
        public RequestUser() 
            : this(null, null, null)
        {
        }

        [JsonConstructor]
        public RequestUser(Guid? id, string name, string email, string firstname = null, string lastname = null, string phone = null, string picture = null, List<string> roles = null, Guid? companyId = null)
        {
            Id = id ?? Guid.Empty;
            Name = name ?? RoleDefinition.Anonymous;
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
            Phone = phone;
            Picture = picture;
            CompanyId = companyId;
            Roles = roles?.ToList() ?? new List<string>{RoleDefinition.Anonymous};
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }
        public string Picture { get; }
        public string Lastname { get; }
        public string Firstname { get; }
        public List<string> Roles { get; }
        public Guid? CompanyId { get; }
        public bool IsAuthenticated() => Id != Guid.Empty;
        public bool IsInRole(string role) => Roles.Contains(role);
        public bool IsInRoles(IEnumerable<string> roles) => Roles.Intersect(roles).Any();
    }
}