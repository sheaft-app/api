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
    public class CreateBusinessLegalCommand : Command<Guid>
    {
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
        public CreateOwnerDto Owner { get; set; }
        public AddressDto Address { get; set; }
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
            var business = await _context.GetByIdAsync<Domain.Business>(request.UserId, token);
            await _context.EnsureNotExists<BusinessLegal>(c => c.User.Id == business.Id, token);

            var legal = new BusinessLegal(
                Guid.NewGuid(),
                business,
                request.Kind,
                request.Name,
                request.Email,
                request.Siret,
                request.VatIdentifier,
                new LegalAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country),
                new Owner(business.Id,
                    request.Owner.FirstName,
                    request.Owner.LastName,
                    request.Owner.Email,
                    request.Owner.BirthDate,
                    new OwnerAddress(request.Owner.Address.Line1, request.Owner.Address.Line2,
                        request.Owner.Address.Zipcode, request.Owner.Address.City, request.Owner.Address.Country),
                    request.Owner.Nationality,
                    request.Owner.CountryOfResidence
                ));

            await _context.AddAsync(legal, token);
            await _context.SaveChangesAsync(token);

            if (string.IsNullOrWhiteSpace(legal.User.Identifier))
            {
                var userResult = await _mediatr.Process(
                    new CheckBusinessLegalConfigurationCommand(request.RequestUser) {UserId = legal.User.Id}, token);
                if (!userResult.Succeeded)
                    return Failure<Guid>(userResult);
            }
            else
            {
                var result = await _pspService.UpdateBusinessAsync(legal, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result);
            }

            return Success(legal.Id);
        }
    }
}