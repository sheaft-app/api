using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Setting : IIdEntity
    {
        protected Setting() { }

        public Setting(string name, SettingKind kind)
        {
            Name = name;
            Kind = kind;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public SettingKind Kind { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}