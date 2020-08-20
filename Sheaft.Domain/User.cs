using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class User : IEntity
    {
        private List<UserPoints> _points;

        protected User()
        {
        }

        public User(Guid id, UserKind userType, string email, string firstname, string lastname, string phone = null, Company company = null)
        {
            Id = id;
            Company = company;
            UserType = userType;

            SetEmail(email);
            SetPhone(phone);
            SetFirstname(firstname);
            SetLastname(lastname);

            _points = new List<UserPoints>();

            RefreshPoints();
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public UserKind UserType { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }
        public string Picture { get; private set; }
        public string Reason { get; private set; }
        public string Code { get; private set; }
        public int TotalPoints { get; private set; }
        public bool Anonymous { get; private set; }
        public virtual Department Department { get; private set; }
        public virtual Company Company { get; private set; }
        public virtual IReadOnlyCollection<UserPoints> Points { get { return _points.AsReadOnly(); } }

        public void SetFirstname(string firstname)
        {
            if (string.IsNullOrWhiteSpace(firstname))
                throw new ValidationException(MessageKind.User_Firstname_Required);

            FirstName = firstname;
        }

        public void SetLastname(string lastname)
        {
            if (string.IsNullOrWhiteSpace(lastname))
                throw new ValidationException(MessageKind.User_Lastname_Required);

            LastName = lastname;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException(MessageKind.User_Email_Required);

            Email = email;
        }

        public void SetPhone(string phone)
        {
            if (phone == null)
                return;

            Phone = phone;
        }

        public void SetPicture(string imageUrl)
        {
            if (imageUrl == null)
                return;

            Picture = imageUrl;
        }

        public void SetAnonymous(bool anonymous)
        {
            Anonymous = anonymous;
        }

        public void SetDepartment(Department department)
        {
            Department = department;
        }

        public void SetSponsoringCode(string code)
        {
            Code = code;
        }

        public void AddPoints(PointKind kind, int quantity, DateTimeOffset? createdOn = null)
        {
            if (Points == null)
                _points = new List<UserPoints>();

            _points.Add(new UserPoints(kind, quantity, createdOn ?? DateTimeOffset.UtcNow));
            RefreshPoints();
        }

        public void CloseAccount(string reason)
        {
            Email = string.Empty;
            Phone = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Reason = reason;
            Picture = null;
        }

        private void RefreshPoints()
        {
            TotalPoints = _points.Sum(c => c.Quantity);
        }
    }
}