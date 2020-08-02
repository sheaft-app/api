using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Reward : IEntity
    {
        protected Reward()
        {
        }

        public Reward(Guid id, string name, Department department)
        {
            Id = id;
            Department = department;

            SetName(name);
        }

        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Image { get; private set; }
        public string Contact { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Url { get; private set; }

        public virtual User Winner { get; private set; }
        public virtual Department Department { get; private set; }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(MessageKind.Reward_Name_Required);

            Name = name;
        }

        public void AssignRewardToUser(User user)
        {
            Winner = user;
        }
    }
}