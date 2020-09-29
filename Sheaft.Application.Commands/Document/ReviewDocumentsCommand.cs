using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class ReviewDocumentsCommand : Command<bool>
    {
        [JsonConstructor]
        public ReviewDocumentsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
