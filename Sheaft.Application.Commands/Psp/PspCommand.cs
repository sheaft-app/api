using System;
using Sheaft.Core;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Commands
{
    public abstract class PspCommand: Command<bool>
    {
        protected PspCommand(RequestUser requestUser, PspEventKind kind, string identifier, DateTimeOffset executedOn) 
            : base(requestUser)
        {
            Identifier = identifier;
            ExecutedOn = executedOn;
            Kind = kind;
        }

        public string Identifier { get; set; }
        public PspEventKind Kind { get; set; }
        public DateTimeOffset ExecutedOn { get; set; }
    }
}
