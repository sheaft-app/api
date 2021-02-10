using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Ubo.Commands
{
    public class CreateUboCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateUboCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public AddressInput Address { get; set; }
        public BirthAddressInput BirthPlace { get; set; }
    }

    public class CreateUboCommandHandler : CommandsHandler,
        IRequestHandler<CreateUboCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public CreateUboCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<CreateUboCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateUboCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(
                c => c.Declaration.Id == request.DeclarationId, token);
            var ubo = new Domain.Ubo(Guid.NewGuid(),
                request.FirstName,
                request.LastName,
                request.BirthDate,
                new UboAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country),
                new BirthAddress(request.BirthPlace.City, request.BirthPlace.Country),
                request.Nationality);

            legal.Declaration.AddUbo(ubo);

            await _context.SaveChangesAsync(token);

            var result = await _pspService.CreateUboAsync(ubo, legal.Declaration, legal.User, token);
            if (!result.Succeeded)
                return Failure<Guid>(result.Exception);

            ubo.SetIdentifier(result.Data);

            await _context.SaveChangesAsync(token);
            return Success(ubo.Id);
        }
    }
}