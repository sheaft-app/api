using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sheaft.Domain.Common;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class BatchNumber : IEntity
    {
        protected BatchNumber()
        {
        }

        public BatchNumber(string number, User user)
        {
            Id = Guid.NewGuid();
            Number = number;
            CreatedById = user.Id;
            SupplierId = user.CompanyId.Value;
        }

        public Guid Id { get; }
        public string Number { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public DateTimeOffset? DLC { get; set; }
        public DateTimeOffset? DDM { get; set; }
        public Guid SupplierId { get; private set; }
        public Guid CreatedById { get; private set; }
        public User CreatedBy { get; private set; }
        public byte[] RowVersion { get; private set; }
        
        public void Restore()
        {
            Removed = false;
        }
    }
}