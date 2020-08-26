﻿using Sheaft.Exceptions;
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

            SetName(name);
            SetDepartment(department);
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

        public void SetImage(string image)
        {
            if (image == null)
                return;

            Image = image;
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

        public void AssignRewardToUser(User user)
        {
            Winner = user;
        }

        public void SetDepartment(Department department)
        {
            Department = department;
        }

        public void Remove()
        {
        }

        public void Restore()
        {
            RemovedOn = null;
        }
    }
}