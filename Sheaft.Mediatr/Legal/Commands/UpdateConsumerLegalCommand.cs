using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Legal.Commands
{
    public class UpdateConsumerLegalCommand : Command
    {
        [JsonConstructor]
        public UpdateConsumerLegalCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressDto Address { get; set; }
        public LegalValidation Validation { get; set; }
    }

    public class UpdateConsumerLegalCommandHandler : CommandsHandler,
        IRequestHandler<UpdateConsumerLegalCommand, Result>
    {
        private readonly IPspService _pspService;

        public UpdateConsumerLegalCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<UpdateConsumerLegalCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(UpdateConsumerLegalCommand request, CancellationToken token)
        {
            var legal = await _context.GetByIdAsync<ConsumerLegal>(request.LegalId, token);

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
                var userResult = await _mediatr.Process(
                    new CheckConsumerLegalConfigurationCommand(request.RequestUser) {UserId = legal.User.Id}, token);
                if (!userResult.Succeeded)
                    return Failure(userResult);
            }
            else
            {
                var result = await _pspService.UpdateConsumerAsync(legal, token);
                if (!result.Succeeded)
                    return Failure(result);
            }

            return Success();
        }
    }
}