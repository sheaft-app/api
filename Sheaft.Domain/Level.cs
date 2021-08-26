using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Level : IEntity
    {
        protected Level()
        {
        }

        public Level(Guid id, string name, int requiredPoints)
        {

            Id = id;
            RequiredPoints = requiredPoints;
            Rewards = new List<Reward>();
            
            SetName(name);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Name { get; private set; }
        public int RequiredPoints { get; private set; }
        public virtual ICollection<Reward> Rewards { get; private set; }

        public void SetRewards(IEnumerable<Reward> rewards)
        {
            if (Rewards == null)
                Rewards = new List<Reward>();

            Rewards.Clear();

            if (rewards != null && rewards.Any())
                AddRewards(rewards);
        }

        public void AddRewards(IEnumerable<Reward> rewards)
        {
            foreach(var reward in rewards)
            {
                AddReward(reward);
            }
        }

        public void AddReward(Reward reward)
        {
            if (Rewards == null)
                Rewards = new List<Reward>();

            Rewards.Add(reward);
        }

        public void RemoveReward(Reward reward)
        {
            Rewards.Remove(reward);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw SheaftException.Validation("Le nom est requis.");

            Name = name;
        }

        public void SetRequiredPoints(int requiredPoints)
        {
            RequiredPoints = requiredPoints;
        }
    }
}