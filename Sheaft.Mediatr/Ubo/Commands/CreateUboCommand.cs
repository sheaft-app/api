using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Ubo.Commands
{
    public class CreateUboCommand : Command<Guid>
    {
        protected CreateUboCommand()
        {
            
        }
        [JsonConstructor]
        public CreateUboCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public AddressDto Address { get; set; }
        public BirthAddressDto BirthPlace { get; set; }
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
            var legal = await _context.Set<BusinessLegal>()
                .SingleOrDefaultAsync(c => c.DeclarationId == request.DeclarationId, token);
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
                return Failure<Guid>(result);

            ubo.SetIdentifier(result.Data);

            await _context.SaveChangesAsync(token);
            return Success(ubo.Id);
        }
    }
}