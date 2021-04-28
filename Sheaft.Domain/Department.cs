using System;

namespace Sheaft.Domain
{
    public class Department
    {
        public Department()
        {
        }

        public Department(Guid id, string code, string name, Region region)
        {
            Id = id;
            Code = code;
            Name = name;

            Region = region;
            RegionId = region.Id;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int Points { get; private set; }
        public int Position { get; private set; }
        public int ProducersCount { get; private set; }
        public int StoresCount { get; private set; }
        public int ConsumersCount { get; private set; }
        public int? RequiredProducers { get; private set; }
        public Guid RegionId { get; private set; }
        public Guid? LevelId { get; private set; }
        public virtual Region Region { get; private set; }
        public virtual Level Level { get; private set; }

        public void SetName(string name)
        {
            if (name == null)
                return;

            Name = name;
        }

        public void SetRequiredProducers(int? requiredProducers)
        {
            RequiredProducers = requiredProducers;
        }

        public void SetLevel(Level level)
        {
            Level = level;
        }
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