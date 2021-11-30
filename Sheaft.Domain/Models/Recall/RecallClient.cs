using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class RecallClient : ITrackUpdate
    { 
        protected RecallClient()
        {
        }
        
        public RecallClient(User client)
        {
            ClientId = client.Id;
        }

        public Guid ClientId { get; private set; }
        public Guid RecallId { get; private set; }
        public bool RecallSent { get; set; }
        public DateTimeOffset UpdatedOn { get; private set; }
    }
}