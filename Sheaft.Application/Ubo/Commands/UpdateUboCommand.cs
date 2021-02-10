using System;
using System.Linq;
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
    public class UpdateUboCommand : Command
    {
        [JsonConstructor]
        public UpdateUboCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public AddressInput Address { get; set; }
        public BirthAddressInput BirthPlace { get; set; }
    }

    public class UpdateUboCommandHandler : CommandsHandler,
        IRequestHandler<UpdateUboCommand, Result>
    {
        private readonly IPspService _pspService;

        public UpdateUboCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<UpdateUboCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(UpdateUboCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(
                c => c.Declaration.Ubos.Any(u => u.Id == request.Id), token);
            var ubo = legal.Declaration.Ubos.FirstOrDefault(u => u.Id == request.Id);

            ubo.SetFirstName(request.FirstName);
            ubo.SetLastName(request.LastName);
            ubo.SetBirthDate(request.BirthDate);

            var address = new UboAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                request.Address.City, request.Address.Country);
            ubo.SetAddress(address);

            var birthPlace = new BirthAddress(request.BirthPlace.City, request.BirthPlace.Country);
            ubo.SetBirthPlace(birthPlace);

            var result = await _pspService.UpdateUboAsync(ubo, legal.Declaration, legal.User, token);
            if (!result.Succeeded)
                return Failure(result.Exception);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}