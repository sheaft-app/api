using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class Level : IEntity
    {
        private List<Reward> _rewards;

        protected Level()
        {
        }

        public Level(Guid id, string name, int number, int requiredPoints)
        {

            Id = id;
            Number = number;
            RequiredPoints = requiredPoints;

            SetName(name);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Name { get; private set; }
        public int Number { get; private set; }
        public int RequiredPoints { get; private set; }
        public virtual IReadOnlyCollection<Reward> Rewards { get { return _rewards.AsReadOnly(); } }

        public void SetRewards(IEnumerable<Reward> rewards)
        {
            _rewards = new List<Reward>();

            if (rewards != null && rewards.Any())
                AddRewards(rewards);
        }

        private void AddRewards(IEnumerable<Reward> rewards)
        {
            _rewards.AddRange(rewards);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(MessageKind.Level_Name_Required);

            Name = name;
        }
    }
}