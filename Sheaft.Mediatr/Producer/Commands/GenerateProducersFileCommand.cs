using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Producer.Commands
{
    public class GenerateProducersFileCommand : Command
    {
        protected GenerateProducersFileCommand()
        {
            
        }
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
            var producers = await _context.Producers.ToListAsync(token);
            var prods = producers.Select(p => new ProducerListItem(p));

            var result = await _blobService.UploadProducersListAsync(
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(prods,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new DefaultContractResolver {NamingStrategy = new CamelCaseNamingStrategy()}
                    })), token);

            if (!result.Succeeded)
                return Failure(result);

            return Success();
        }

        internal class ProducerListItem
        {
            internal ProducerListItem(Domain.Producer user)
            {
                Address = new AddressItem(user.Address);
                Id = user.Id.ToString("N");
                Name = user.Name;
                Picture = user.Picture;
                HasProducts = user.HasProducts;
            }

            public string Id { get; set; }
            public string Name { get; set; }
            public string Picture { get; set; }
            public bool HasProducts { get; set; }
            public AddressItem Address { get; set; }
        }

        internal class AddressItem
        {
            internal AddressItem(UserAddress address)
            {
                if (address == null)
                    return;
                
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