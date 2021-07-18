using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class RecallClient : ITrackCreation
    { 
        protected RecallClient()
        {
        }
        
        public RecallClient(User client)
        {
            ClientId = client.Id;
            Client = client;
        }

        public Guid ClientId { get; private set; }
        public Guid RecallId { get; private set; }
        public bool RecallSent { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public virtual User Client { get; private set; }

        public void SetRecallAsSent(bool sent = true)
        {
            RecallSent = sent;
        }
    }
}