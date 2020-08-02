using Sheaft.Interop;
using System;

namespace Sheaft.Domain.Models
{
    public class Region : IEntity
    {
        protected Region()
        {
        }

        public Region(Guid id, string code, string name)
        {
            Id = id;
            Code = code;
            Name = name;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int Points { get; private set; }
        public int Position { get; private set; }
        public int ProducersCount { get; private set; }
        public int StoresCount { get; private set; }
        public int ConsumersCount { get; private set; }

        public void SetPoints(int points)
        {
            Points = points;
        }
        public void SetPosition(int position)
        {
            Position = position;
        }
        public void SetProducersCount(int count)
        {
            ProducersCount = count;
        }
        public void SetStoresCount(int count)
        {
            StoresCount = count;
        }
        public void SetConsumersCount(int count)
        {
            ConsumersCount = count;
        }
    }
}