﻿using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Setting : IIdEntity
    {
        protected Setting(){}

        public Setting(Guid id, string name, SettingKind kind)
        {
            Id = id;
            Name = name;
            Kind = kind;
        }
        
        public Guid Id { get; private set; }
        public SettingKind Kind { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}