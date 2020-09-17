using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public abstract class PspCommand: Command<bool>
    {
        protected PspCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn) 
            : base(requestUser)
        {
            Identifier = identifier;
            ExecutedOn = executedOn;
        }

        public string Identifier { get; set; }
        public DateTimeOffset ExecutedOn { get; set; }
    }
}
