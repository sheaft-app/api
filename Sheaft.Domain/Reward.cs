using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Reward : IEntity
    {
        protected Reward()
        {
        }

        public Reward(Guid id, string name, Department department)
        {
            Id = id;

            SetName(name);
            SetDepartment(department);
        }

        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Picture { get; private set; }
        public string Contact { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Url { get; private set; }

        public virtual Consumer Winner { get; private set; }
        public virtual Department Department { get; private set; }
        public virtual Level Level { get; private set; }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(MessageKind.Reward_Name_Required);

            Name = name;
        }

        public void SetDescription(string description)
        {
            if (description == null)
                return;

            Description = description;
        }

        public void SetPicture(string picture)
        {
            if (picture == null)
                return;

            Picture = picture;
        }

        public void SetContact(string contact)
        {
            if (contact == null)
                return;

            Contact = contact;
        }

        public void SetEmail(string email)
        {
            if (email == null)
                return;

            Email = email;
        }

        public void SetPhone(string phone)
        {
            if (phone == null)
                return;

            Phone = phone;
        }

        public void SetUrl(string url)
        {
            if (url == null)
                return;

            Url = url;
        }

        public void AssignRewardToUser(Consumer user)
        {
            Winner = user;
        }

        public void SetDepartment(Department department)
        {
            Department = department;
        }
    }
}