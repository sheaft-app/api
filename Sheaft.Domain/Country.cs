using System;

namespace Sheaft.Domain
{
    public class Country
    {
        protected Country()
        {
        }

        public Country(Guid id, string name, string alpha2)
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