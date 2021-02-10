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
    public class UpdateConsumerLegalCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateConsumerLegalCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressInput Address { get; set; }
        public LegalValidation Validation { get; set; }
    }
    public class UpdateConsumerLegalCommandHandler : CommandsHandler,
           IRequestHandler<UpdateConsumerLegalCommand, Result<bool>>
    {
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;

        public UpdateConsumerLegalCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<UpdateConsumerLegalCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspOptions = pspOptions.Value;
            _pspService = pspService;
        }

        public async Task<Result<bool>> Handle(UpdateConsumerLegalCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetByIdAsync<ConsumerLegal>(request.Id, token);

                var ownerAddress = new OwnerAddress(request.Address.Line1,
                    request.Address.Line2,
                    request.Address.Zipcode,
                    request.Address.City,
                    request.Address.Country
                );

                legal.SetValidation(request.Validation);
                legal.Owner.SetFirstname(request.FirstName);
                legal.Owner.SetLastname(request.LastName);
                legal.Owner.SetEmail(request.Email);
                legal.Owner.SetBirthDate(request.BirthDate);
                legal.Owner.SetNationality(request.Nationality);
                legal.Owner.SetCountryOfResidence(request.CountryOfResidence);
                legal.Owner.SetAddress(ownerAddress);

                await _context.SaveChangesAsync(token);

                if (string.IsNullOrWhiteSpace(legal.User.Identifier))
                {
                    var userResult = await _mediatr.Process(new CheckConsumerLegalConfigurationCommand(request.RequestUser) { UserId = legal.User.Id }, token);
                    if (!userResult.Success)
                        return Failed<bool>(userResult.Exception);
                }
                else
                {
                    var result = await _pspService.UpdateConsumerAsync(legal, token);
                    if (!result.Success)
                        return Failed<bool>(result.Exception);
                }

                return Ok(true);
            });
        }
    }
}
