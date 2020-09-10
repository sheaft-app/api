using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Nationality
    {
        protected Nationality()
        {
        }

        public Nationality(Guid id, string name, string alpha2)
        {
            Id = id;
            Name = name;
            Alpha2 = alpha2;
        }

        public Guid Id { get; private set; }
        public string Alpha2 { get; set; }
        public string Name { get; set; }
    }
}