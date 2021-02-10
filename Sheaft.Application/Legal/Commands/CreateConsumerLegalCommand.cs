using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CreateConsumerLegalCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateConsumerLegalCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressInput Address { get; set; }
    }
    
    public class CreateConsumerLegalCommandHandler : CommandsHandler,
           IRequestHandler<CreateConsumerLegalCommand, Result<Guid>>
    {
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;

        public CreateConsumerLegalCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreateConsumerLegalCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspOptions = pspOptions.Value;
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateConsumerLegalCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var consumer = await _context.GetByIdAsync<Consumer>(request.UserId, token);
                await _context.EnsureNotExists<ConsumerLegal>(c => c.User.Id == consumer.Id, token);

                var ownerAddress = new OwnerAddress(request.Address.Line1,
                    request.Address.Line2,
                    request.Address.Zipcode,
                    request.Address.City,
                    request.Address.Country
                );

                var legal = new ConsumerLegal(Guid.NewGuid(),
                    consumer,
                    new Owner(consumer.Id,
                        request.FirstName,
                        request.LastName,
                        request.Email,
                        request.BirthDate,
                        ownerAddress,
                        request.Nationality,
                        request.CountryOfResidence
                    ));

                await _context.AddAsync(legal, token);
                await _context.SaveChangesAsync(token);

                if (string.IsNullOrWhiteSpace(legal.User.Identifier))
                {
                    var userResult = await _mediatr.Process(new CheckConsumerLegalConfigurationCommand(request.RequestUser) { UserId = legal.User.Id }, token);
                    if (!userResult.Success)
                        return Failed<Guid>(userResult.Exception);
                }
                else
                {
                    var result = await _pspService.UpdateConsumerAsync(legal, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);
                }

                return Ok(legal.Id);
            });
        }
    }
}
