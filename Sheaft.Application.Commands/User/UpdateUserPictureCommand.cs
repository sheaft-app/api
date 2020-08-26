using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateUserPictureCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateUserPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Picture { get; set; }
    }
}
