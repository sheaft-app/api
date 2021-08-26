using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Store.Commands
{
    public class GenerateStoresFileCommand : Command
    {
        protected GenerateStoresFileCommand()
        {
            
        }
        [JsonConstructor]
        public GenerateStoresFileCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class GenerateStoresFileCommandHandler : CommandsHandler,
        IRequestHandler<GenerateStoresFileCommand, Result>
    {
        private readonly IBlobService _blobService;
        private readonly IIdSerializer _idSerializer;

        public GenerateStoresFileCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IBlobService blobService,
            IIdSerializer idSerializer,
            ILogger<GenerateStoresFileCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
            _idSerializer = idSerializer;
        }

        public async Task<Result> Handle(GenerateStoresFileCommand request, CancellationToken token)
        {
            var stores = await _context.Stores.ToListAsync(token);
            var strs = stores.Select(p => new StoreListItem(p, _idSerializer));

            var result = await _blobService.UploadStoresListAsync(
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(strs,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new DefaultContractResolver {NamingStrategy = new CamelCaseNamingStrategy()}
                    })), token);

            if (!result.Succeeded)
                return Failure(result);

            return Success();
        }

        internal class StoreListItem
        {
            internal StoreListItem(Domain.Store user, IIdSerializer serializer)
            {
                Address = new AddressItem(user.Address);
                Id = serializer.Serialize("Query", nameof(Store), user.Id);
                Name = user.Name;
                Picture = user.Picture;
                HasProducers = user.ProducersCount > 0;
                Producers = user.ProducersCount;
            }

            public string Id { get; set; }
            public string Name { get; set; }
            public string Picture { get; set; }
            public bool HasProducers { get; set; }
            public int Producers { get; set; }
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