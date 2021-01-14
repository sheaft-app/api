using Sheaft.Domain.Enums;
using Sheaft.Domain.Interop;
using System;

namespace Sheaft.Domain.Models
{
    public class CardRegistration : IEntity
    {
        protected CardRegistration() { }

        public CardRegistration(Guid id, User user)
        {
            Id = id;
            User = user;
        }

        public Guid Id { get; private set; }
        public string Identifier { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn  { get; private set; }
        public DateTimeOffset? RemovedOn  { get; private set; }
        public string PreRegistrationData { get; private set; }
        public string RegistrationData { get; private set; }
        public string AccessKey { get; private set; }
        public string CardRegistrationURL { get; private set; }
        public CardStatus Status { get; private set; }
        public string ResultCode { get; private set; }
        public virtual User User { get; set; }

        public void SetIdentifier(string identifier)
        {
            if (identifier == null)
                return;

            Identifier = identifier;
        }

        public void SetPreRegistrationData(string preregistrationData)
        {
            if (preregistrationData == null)
                return;

            PreRegistrationData = preregistrationData;
        }

        public void SetRegistrationData(string registrationData)
        {
            if (registrationData == null)
                return;

            RegistrationData = registrationData;
        }

        public void SetAccessKey(string accessKey)
        {
            if (accessKey == null)
                return;

            AccessKey = accessKey;
        }

        public void SetUrl(string cardRegistrationURL)
        {
            if (cardRegistrationURL == null)
                return;

            CardRegistrationURL = cardRegistrationURL;
        }

        public void SetStatus(CardStatus status)
        {
            if (status == null)
                return;

            Status = status;
        }

        public void SetResult(string resultCode)
        {
            if (resultCode == null)
                return;

            ResultCode = resultCode;
        }
    }
}