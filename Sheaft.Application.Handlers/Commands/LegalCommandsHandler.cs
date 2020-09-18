using Sheaft.Application.Commands;
using Sheaft.Infrastructure.Interop;
using System;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Domain.Models;
using Sheaft.Interop.Enums;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{
    public class LegalCommandsHandler : ResultsHandler,
           IRequestHandler<CreateBusinessLegalCommand, Result<Guid>>,
           IRequestHandler<CreateConsumerLegalCommand, Result<Guid>>,
           IRequestHandler<UpdateBusinessLegalCommand, Result<bool>>,
           IRequestHandler<UpdateConsumerLegalCommand, Result<bool>>
    {
        private readonly IMediator _mediatr;
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;

        public LegalCommandsHandler(
            IMediator mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<LegalCommandsHandler> logger) : base(logger)
        {
            _pspService = pspService;
            _mediatr = mediatr;
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateBusinessLegalCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var business = await _context.GetByIdAsync<Business>(request.UserId, token);
                var legal = await _context.FindSingleAsync<BusinessLegal>(c => c.Business.Id == business.Id, token);

                if (legal == null)
                {
                    var legalAddress = new LegalAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country);
                    var ownerAddress = new OwnerAddress(request.Owner.Address.Line1, request.Owner.Address.Line2, request.Owner.Address.Zipcode, request.Owner.Address.City, request.Owner.Address.Country);

                    legal = new BusinessLegal(Guid.NewGuid(),
                        business,
                        request.Kind,
                        request.Email,
                        legalAddress,
                        new Owner(business.Id,
                            request.Owner.FirstName,
                            request.Owner.LastName,
                            request.Owner.Email,
                            request.Owner.BirthDate,
                            ownerAddress,
                            request.Owner.Nationality,
                            request.Owner.CountryOfResidence
                        ));

                    await _context.AddAsync(legal, token);
                    await _context.SaveChangesAsync(token);
                }

                if (string.IsNullOrWhiteSpace(legal.Business.Identifier))
                {
                    var userResult = await _pspService.CreateBusinessAsync(legal, token);
                    if (!userResult.Success)
                        return Failed<Guid>(userResult.Exception);

                    business.SetIdentifier(userResult.Data);
                    _context.Update(business);

                    await _context.SaveChangesAsync(token);
                }

                var wallet = await _context.FindSingleAsync<Wallet>(c => c.User.Id == business.Id && c.Kind == WalletKind.Payments, token);
                if (wallet == null)
                {
                    var walletResult = await _mediatr.Send(new CreateWalletCommand(request.RequestUser)
                    {
                        Name = "Paiement",
                        Kind = WalletKind.Payments,
                        UserId = business.Id
                    }, token);

                    if (!walletResult.Success)
                        return Failed<Guid>(walletResult.Exception);
                }

                if (business.Kind == ProfileKind.Producer)
                {
                    var result = await _mediatr.Send(new CreateDeclarationCommand(request.RequestUser)
                    {
                        LegalId = legal.Id
                    }, token);

                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    var uboDeclaration = await _context.GetByIdAsync<UboDeclaration>(result.Data, token);
                    legal.SetUboDeclaration(uboDeclaration);

                    _context.Update(legal);
                    await _context.SaveChangesAsync(token);
                }

                return Ok(legal.Id);
            });
        }

        public async Task<Result<Guid>> Handle(CreateConsumerLegalCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var consumer = await _context.GetByIdAsync<Consumer>(request.UserId, token);
                var legal = await _context.FindSingleAsync<ConsumerLegal>(c => c.Consumer.Id == consumer.Id, token);

                if (legal == null)
                {
                    var ownerAddress = new OwnerAddress(request.Address.Line1,
                        request.Address.Line2,
                        request.Address.Zipcode,
                        request.Address.City,
                        request.Address.Country
                    );

                    legal = new ConsumerLegal(Guid.NewGuid(),
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
                }

                if (string.IsNullOrWhiteSpace(legal.Consumer.Identifier))
                {
                    var userResult = await _pspService.CreateConsumerAsync(legal, token);
                    if (!userResult.Success)
                        return Failed<Guid>(userResult.Exception);

                    consumer.SetIdentifier(userResult.Data);
                    _context.Update(consumer);

                    await _context.SaveChangesAsync(token);
                }

                var wallet = await _context.FindSingleAsync<Wallet>(c => c.User.Id == consumer.Id && c.Kind == WalletKind.Payments, token);
                if (wallet == null)
                {
                    var walletResult = await _mediatr.Send(new CreateWalletCommand(request.RequestUser)
                    {
                        Name = "Paiement",
                        Kind = WalletKind.Payments,
                        UserId = consumer.Id
                    }, token);

                    if (!walletResult.Success)
                        return Failed<Guid>(walletResult.Exception);
                }

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
    }
}