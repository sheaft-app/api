using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Legal.Commands
{
    public class UpdateBusinessLegalCommand : Command
    {
        protected UpdateBusinessLegalCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateBusinessLegalCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }
        public LegalKind Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string VatIdentifier { get; set; }
        public string Siret { get; set; }
        public OwnerDto Owner { get; set; }
        public AddressDto Address { get; set; }
        public LegalValidation Validation { get; set; }
    }

    public class UpdateBusinessLegalCommandHandler : CommandsHandler,
        IRequestHandler<UpdateBusinessLegalCommand, Result>
    {
        private readonly IPspService _pspService;

        public UpdateBusinessLegalCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<UpdateBusinessLegalCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(UpdateBusinessLegalCommand request, CancellationToken token)
        {
            var legalAddress = new LegalAddress(request.Address.Line1,
                request.Address.Line2,
                request.Address.Zipcode,
                request.Address.City,
                request.Address.Country);

            var ownerAddress = new OwnerAddress(request.Owner.Address.Line1,
                request.Owner.Address.Line2,
                request.Owner.Address.Zipcode,
                request.Owner.Address.City,
                request.Owner.Address.Country
            );

            var legal = await _context.Set<BusinessLegal>().SingleAsync(e => e.Id == request.LegalId, token);

            legal.SetKind(request.Kind);
            legal.SetValidation(request.Validation);
            legal.SetName(request.Name);
            legal.SetEmail(request.Email);
            legal.SetAddress(legalAddress);
            legal.SetSiret(request.Siret);
            legal.SetVatIdentifier(request.VatIdentifier);

            legal.Owner.SetFirstname(request.Owner.FirstName);
            legal.Owner.SetLastname(request.Owner.LastName);
            legal.Owner.SetEmail(request.Owner.Email);
            legal.Owner.SetBirthDate(request.Owner.BirthDate);
            legal.Owner.SetNationality(request.Owner.Nationality);
            legal.Owner.SetCountryOfResidence(request.Owner.CountryOfResidence);
            legal.Owner.SetAddress(ownerAddress);

            await _context.SaveChangesAsync(token);

            if (string.IsNullOrWhiteSpace(legal.User.Identifier))
            {
                var userResult = await _mediatr.Process(
                    new CheckBusinessLegalConfigurationCommand(request.RequestUser) {UserId = legal.UserId}, token);
                if (!userResult.Succeeded)
                    return Failure(userResult);
            }
            else
            {
                var result = await _pspService.UpdateBusinessAsync(legal, token);
                if (!result.Succeeded)
                    return Failure(result);
            }

            return Success();
        }
    }
}