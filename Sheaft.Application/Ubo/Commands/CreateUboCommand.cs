using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
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
                return await ExecuteAsync(request, async () =>
                {
                    var legal = await _context.GetSingleAsync<BusinessLegal>(
                        c => c.Declaration.Id == request.DeclarationId, token);
                    var ubo = new Ubo(Guid.NewGuid(),
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
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    ubo.SetIdentifier(result.Data);

                    await _context.SaveChangesAsync(token);
                    return Ok(ubo.Id);
                });
            }
        }
}
