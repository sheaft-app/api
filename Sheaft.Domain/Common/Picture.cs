using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain.BaseClass
{
    public abstract class Picture: ITrackUpdate
    {
        protected Picture()
        {
        }
        
        protected Picture(string url, int position)
        {
            Url = url;
            Position = position;
        }

        public DateTimeOffset UpdatedOn { get; private set; }
        public string Url { get; private set; }
        public int Position { get; set; }
    }
}