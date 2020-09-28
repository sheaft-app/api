using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using System;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Domain.Models;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Handlers
{
    public class LegalCommandsHandler : ResultsHandler,
           IRequestHandler<CreateBusinessLegalCommand, Result<Guid>>,
           IRequestHandler<CreateConsumerLegalCommand, Result<Guid>>,
           IRequestHandler<UpdateBusinessLegalCommand, Result<bool>>,
           IRequestHandler<UpdateConsumerLegalCommand, Result<bool>>,
           IRequestHandler<CheckBusinessLegalConfigurationCommand, Result<bool>>,
           IRequestHandler<CheckConsumerLegalConfigurationCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public LegalCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<LegalCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateBusinessLegalCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var business = await _context.GetByIdAsync<Business>(request.UserId, token);
                await _context.EnsureNotExists<BusinessLegal>(c => c.User.Id == business.Id, token);

                var legal = new BusinessLegal(
                    Guid.NewGuid(),
                    business,
                    request.Kind,
                    request.Email,
                    request.Siret,
                    request.VatIdentifier,
                    new LegalAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country),
                    new Owner(business.Id,
                        request.Owner.FirstName,
                        request.Owner.LastName,
                        request.Owner.Email,
                        request.Owner.BirthDate,
                        new OwnerAddress(request.Owner.Address.Line1, request.Owner.Address.Line2, request.Owner.Address.Zipcode, request.Owner.Address.City, request.Owner.Address.Country),
                        request.Owner.Nationality,
                        request.Owner.CountryOfResidence
                    ));

                await _context.AddAsync(legal, token);
                await _context.SaveChangesAsync(token);

                var userResult = await _pspService.CreateBusinessAsync(legal, token);
                if (!userResult.Success)
                    return Failed<Guid>(userResult.Exception);

                legal.User.SetIdentifier(userResult.Data);
                _context.Update(legal.User);
                await _context.SaveChangesAsync(token);

                if (request.Kind == LegalKind.Business && business.Kind == ProfileKind.Producer)
                {
                    var result = await _mediatr.Process(new CreateDeclarationCommand(request.RequestUser) { LegalId = legal.Id }, token);
                    if (result.Success)
                    {
                        var uboDeclaration = await _context.GetByIdAsync<UboDeclaration>(result.Data, token);
                        legal.SetUboDeclaration(uboDeclaration);

                        _context.Update(legal);
                        await _context.SaveChangesAsync(token);
                    }
                }

                return Ok(legal.Id);
            });
        }

        public async Task<Result<Guid>> Handle(CreateConsumerLegalCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var consumer = await _context.GetByIdAsync<Consumer>(request.UserId, token);
                await _context.EnsureNotExists<ConsumerLegal>(c => c.User.Id == consumer.Id, token);

                var ownerAddress = new OwnerAddress(request.Address.Line1,
                    request.Address.Line2,
                    request.Address.Zipcode,
                    request.Address.City,
                    request.Address.Country
                );

                var legal = new ConsumerLegal(Guid.NewGuid(),
                    consumer,
                    new Owner(consumer.Id,
                        request.FirstName,
                        request.LastName,
                        request.Email,
                        request.BirthDate,
                        ownerAddress,
                        request.Nationality,
                        request.CountryOfResidence
                    ));

                await _context.AddAsync(legal, token);
                await _context.SaveChangesAsync(token);

                return Ok(legal.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdateBusinessLegalCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var legalAddress = new LegalAddress(request.Address.Line1,
                    request.Address.Line2,
                    request.Address.Zipcode,
                    request.Address.City,
                    request.Address.Country);

                var ownerAddress = new OwnerAddress(request.Address.Line1,
                    request.Address.Line2,
                    request.Address.Zipcode,
                    request.Address.City,
                    request.Address.Country
                );

                var legal = await _context.GetByIdAsync<BusinessLegal>(request.Id, token);

                legal.SetKind(request.Kind);
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

                //TODO update PSP Data

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(UpdateConsumerLegalCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var legal = await _context.GetByIdAsync<ConsumerLegal>(request.Id, token);

                var ownerAddress = new OwnerAddress(request.Address.Line1,
                    request.Address.Line2,
                    request.Address.Zipcode,
                    request.Address.City,
                    request.Address.Country
                );

                legal.Owner.SetFirstname(request.FirstName);
                legal.Owner.SetLastname(request.LastName);
                legal.Owner.SetEmail(request.Email);
                legal.Owner.SetBirthDate(request.BirthDate);
                legal.Owner.SetNationality(request.Nationality);
                legal.Owner.SetCountryOfResidence(request.CountryOfResidence);
                legal.Owner.SetAddress(ownerAddress);

                //TODO update PSP Data

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(CheckBusinessLegalConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(b => b.User.Id == request.UserId, token);
                if (string.IsNullOrWhiteSpace(legal.User.Identifier))
                {
                    var userResult = await _pspService.CreateBusinessAsync(legal, token);
                    if (!userResult.Success)
                        return Failed<bool>(userResult.Exception);

                    legal.User.SetIdentifier(userResult.Data);
                    _context.Update(legal.User);

                    await _context.SaveChangesAsync(token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckConsumerLegalConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var legal = await _context.GetSingleAsync<ConsumerLegal>(b => b.User.Id == request.UserId, token);
                if (string.IsNullOrWhiteSpace(legal.User.Identifier))
                {
                    var userResult = await _pspService.CreateConsumerAsync(legal, token);
                    if (!userResult.Success)
                        return Failed<bool>(userResult.Exception);

                    legal.User.SetIdentifier(userResult.Data);
                    _context.Update(legal.User);

                    await _context.SaveChangesAsync(token);
                }

                return Ok(true);
            });
        }
    }
}