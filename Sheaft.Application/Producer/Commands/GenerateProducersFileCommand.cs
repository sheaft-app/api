using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;

namespace Sheaft.Application.Producer.Commands
{
    public class GenerateProducersFileCommand : Command
    {
        [JsonConstructor]
        public GenerateProducersFileCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class GenerateProducersFileCommandHandler : CommandsHandler,
        IRequestHandler<GenerateProducersFileCommand, Result>
    {
        private readonly IBlobService _blobService;

        public GenerateProducersFileCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IBlobService blobService,
            ILogger<GenerateProducersFileCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result> Handle(GenerateProducersFileCommand request, CancellationToken token)
        {
            var producers = await _context.GetAsync<Domain.Producer>(p => p.CanDirectSell, token);
            var prods = producers.Select(p => new ProducerListItem(p));

            var result = await _blobService.UploadProducersListAsync(
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(prods,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new DefaultContractResolver {NamingStrategy = new CamelCaseNamingStrategy()}
                    })), token);

            if (!result.Succeeded)
                return Failure(result.Exception);

            return Success();
        }

        internal class ProducerListItem
        {
            internal ProducerListItem(Domain.User user)
            {
                Address = new AddressItem(user.Address);
                Id = user.Id.ToString("N");
                Name = user.Name;
                Picture = user.Picture;
            }

            public string Id { get; set; }
            public string Name { get; set; }
            public string Picture { get; set; }
            public AddressItem Address { get; set; }
        }

        internal class AddressItem
        {
            internal AddressItem(UserAddress address)
            {
                Line1 = address.Line1;
                Line2 = address.Line2;
                Zipcode = address.Zipcode;
                City = address.City;
                Latitude = address.Latitude;
                Longitude = address.Longitude;
            }

            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string Zipcode { get; set; }
            public string City { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
        }
    }
}