using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Batch : IEntity
    {
        protected Batch()
        {
        }

        public Batch(Guid id, string number, Producer producer)
        {
            Id = id;
            Number = number;
            ProducerId = producer.Id;
            Producer = producer;
        }

        public Guid Id { get; private set; }
        public string Number { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset? DLC { get; private set; }
        public DateTimeOffset? DLUO { get; private set; }
        public string Comment { get; private set; }
        public Guid ProducerId { get; private set; }
        public virtual Producer Producer { get; private set; }
        public byte[] RowVersion { get; private set; }
        
        public void SetNumber(string number)
        {
            Number = number;
        }
        
        public void SetDLC(DateTimeOffset? dlc)
        {
            DLC = dlc;
        }

        public void SetDLUO(DateTimeOffset? dluo)
        {
            DLUO = dluo;
        }

        public void SetComment(string comment)
        {
            Comment = comment;
        }
    }
}