using System;

namespace Sheaft.Core.Options
{
    public class RoleOptions
    {
        public const string SETTING = "Roles";
        public RoleDefinition Admin { get; set; }
        public RoleDefinition Owner { get; set; }
        public RoleDefinition User { get; set; }
        public RoleDefinition Store { get; set; }
        public RoleDefinition Producer { get; set; }
        public RoleDefinition Consumer { get; set; }
        public RoleDefinition Support { get; set; }
        public RoleDefinition Anonymous { get; set; }
    }

    public class RoleDefinition
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }
}
