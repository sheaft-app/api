﻿using Sheaft.Exceptions;
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

        public Level(Guid id, string name, int requiredPoints)
        {

            Id = id;
            RequiredPoints = requiredPoints;

            SetName(name);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Name { get; private set; }
        public int RequiredPoints { get; private set; }
        public virtual IReadOnlyCollection<Reward> Rewards { get { return _rewards.AsReadOnly(); } }

        public void SetRewards(IEnumerable<Reward> rewards)
        {
            if (!Rewards.Any())
                _rewards = new List<Reward>();

            _rewards.Clear();

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
            if (!Rewards.Any())
                _rewards = new List<Reward>();

            _rewards.Add(reward);
        }

        public void RemoveReward(Reward reward)
        {
            if (!Rewards.Any())
                _rewards = new List<Reward>();

            _rewards.Remove(reward);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(MessageKind.Level_Name_Required);

            Name = name;
        }

        public void SetRequiredPoints(int requiredPoints)
        {
            RequiredPoints = requiredPoints;
        }
    }
}