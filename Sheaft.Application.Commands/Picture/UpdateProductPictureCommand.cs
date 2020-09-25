using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateProductPictureCommand : Command<string>
    {
        [JsonConstructor]
        public UpdateProductPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public string Picture { get; set; }
    }
}
