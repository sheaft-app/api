using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateTagPictureCommand : Command<string>
    {
        [JsonConstructor]
        public UpdateTagPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TagId { get; set; }
        public string Picture { get; set; }
    }
}
