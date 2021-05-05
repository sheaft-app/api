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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Legal.Commands
{
    public class CreateConsumerLegalCommand : Command<Guid>
    {
        protected CreateConsumerLegalCommand()
        {
        }

        [JsonConstructor]
        public CreateConsumerLegalCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = requestUser.Id;
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressDto Address { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = user.Id;
        }
    }

    public class CreateConsumerLegalCommandHandler : CommandsHandler,
        IRequestHandler<CreateConsumerLegalCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public CreateConsumerLegalCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<CreateConsumerLegalCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateConsumerLegalCommand request, CancellationToken token)
        {
            var consumer = await _context.Consumers.SingleAsync(e => e.Id == request.UserId, token);
            var legals = consumer.SetLegals(new Owner(
                request.FirstName,
                request.LastName,
                request.Email,
                request.BirthDate,
                new OwnerAddress(request.Address.Line1,
                    request.Address.Line2,
                    request.Address.Zipcode,
                    request.Address.City,
                    request.Address.Country
                ),
                request.Nationality,
                request.CountryOfResidence
            ));

            await _context.SaveChangesAsync(token);

            if (string.IsNullOrWhiteSpace(consumer.Identifier))
            {
                var userResult = await _mediatr.Process(
                    new CheckConsumerLegalConfigurationCommand(request.RequestUser) {UserId = consumer.Id}, token);
                if (!userResult.Succeeded)
                    return Failure<Guid>(userResult);
            }
            else
            {
                var result = await _pspService.UpdateConsumerAsync(legals, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result);
            }

            return Success(legals.Id);
        }
    }
}