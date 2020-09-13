using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Domain.Models
{
    public class BusinessLegal : Legal
    {
        private List<Ubo> _ubos;
        
        protected BusinessLegal()
        {
        }

        public BusinessLegal(Guid id, Business business, LegalKind legalKind, string email, LegalAddress address, Owner owner)
            : base(id, legalKind, owner)
        {
            Address = address;
            Email = email;
            Business = business;
        }

        public string Email { get; private set; }
        public virtual LegalAddress Address { get; private set; }
        public virtual Business Business { get; private set; }
        public virtual IReadOnlyCollection<Ubo> Ubos => _ubos?.AsReadOnly();

        public void SetLegalKind(LegalKind legal)
        {
            Kind = legal;
        }

        public void SetUbos(IEnumerable<Ubo> ubos)
        {
            if (Ubos != null)
                _ubos = new List<Ubo>();

            foreach(var ubo in ubos)
            {
                _ubos.Add(ubo);
            }
        }
    }
}