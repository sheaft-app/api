using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Events
{
    public class NewPspHookEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-hook";

        [JsonConstructor]
        public NewPspHookEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public PspEventKind Kind { get; set; }
        public string Identifier { get; set; }
        public long Date { get; set; }
    }
}
