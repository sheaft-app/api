using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Legal.Commands
{
    public class CreateBusinessLegalCommand : Command<Guid>
    {
        protected CreateBusinessLegalCommand()
        {
            
        }
        [JsonConstructor]
        public CreateBusinessLegalCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public LegalKind Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string VatIdentifier { get; set; }
        public string Siret { get; set; }
        public OwnerInputDto Owner { get; set; }
        public AddressDto Address { get; set; }
        public RegistrationKind? RegistrationKind { get; set; }
        public string RegistrationCity { get; set; }
        public string RegistrationCode { get; set; }
    }

    public class CreateBusinessLegalCommandHandler : CommandsHandler,
        IRequestHandler<CreateBusinessLegalCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public CreateBusinessLegalCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<CreateBusinessLegalCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateBusinessLegalCommand request, CancellationToken token)
        {
            var business = await _context.Businesses.SingleAsync(e => e.Id == request.UserId, token);
            var legals = business.SetLegals(
                request.Kind,
                request.Name,
                request.Email,
                request.Siret,
                request.VatIdentifier,
                new LegalAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country),
                new Owner(
                    request.Owner.FirstName,
                    request.Owner.LastName,
                    request.Owner.Email,
                    request.Owner.BirthDate,
                    new OwnerAddress(request.Owner.Address.Line1, request.Owner.Address.Line2,
                        request.Owner.Address.Zipcode, request.Owner.Address.City, request.Owner.Address.Country),
                    request.Owner.Nationality,
                    request.Owner.CountryOfResidence
                ),
                request.RegistrationCity,
                request.RegistrationCode,
                request.RegistrationKind);
            
            await _context.SaveChangesAsync(token);

            if (string.IsNullOrWhiteSpace(business.Identifier))
            {
                var userResult = await _mediatr.Process(
                    new CheckBusinessLegalConfigurationCommand(request.RequestUser) {UserId = business.Id}, token);
                if (!userResult.Succeeded)
                    return Failure<Guid>(userResult);
            }
            else
            {
                var result = await _pspService.UpdateBusinessAsync(legals, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result);
            }

            return Success(legals.Id);
        }
    }
}