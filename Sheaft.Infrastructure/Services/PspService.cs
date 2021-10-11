using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MangoPay.SDK;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Address = Sheaft.Domain.Address;
using TransactionStatus = Sheaft.Domain.Enum.TransactionStatus;

namespace Sheaft.Infrastructure.Services
{
    public class PspService : SheaftService, IPspService
    {
        private readonly MangoPayApi _api;
        private readonly PspOptions _pspOptions;

        public PspService(
            MangoPayApi mangoPayApi,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<PspService> logger) : base(logger)
        {
            _api = mangoPayApi;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<PspUserLegalDto>> GetCompanyAsync(string identifier, CancellationToken token)
        {
            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Users.GetLegalAsync(identifier);

            return Success(result.GetCompany());
        }

        public async Task<Result<PspUserNormalDto>> GetConsumerAsync(string identifier, CancellationToken token)
        {
            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Users.GetNaturalAsync(identifier);

            return Success(result.GetConsumer());
        }

        public async Task<Result<string>> CreateConsumerAsync(ConsumerLegal consumerLegal, CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(consumerLegal.User.Identifier))
                return Failure<string>("Impossible de créer le consommateur, l'utilisateur existe déjà.");

            if (!consumerLegal.Owner.BirthDate.HasValue)
                return Failure<string>("La date de naissance de l'acheteur est requise pour le service de paiements.");
            
            if (!consumerLegal.Owner.Nationality.HasValue)
                return Failure<string>("La nationalité de l'acheteur est requise pour le service de paiements.");
            
            if (!consumerLegal.Owner.CountryOfResidence.HasValue)
                return Failure<string>("Le pays de résidence de l'acheteur est requise pour le service de paiements.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Users.CreateAsync(GetIdempotencyKey(consumerLegal.Id),
                new UserNaturalPostDTO(
                    consumerLegal.Owner.Email,
                    consumerLegal.Owner.FirstName,
                    consumerLegal.Owner.LastName,
                    consumerLegal.Owner.BirthDate.Value.DateTime,
                    consumerLegal.Owner.Nationality.Value.GetCountry(),
                    consumerLegal.Owner.CountryOfResidence.Value.GetCountry())
                {
                    Address = consumerLegal.Owner.Address.GetAddress(),
                    Tag = $"Id='{consumerLegal.Id}'"
                });

            return Success<string>(result.Id);
        }

        public async Task<Result<string>> UpdateConsumerAsync(ConsumerLegal consumerLegal, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(consumerLegal.User.Identifier))
                return Failure<string>("Impossible de mettre à jour l'utilisateur, l'identifiant est requis.");

            if (!consumerLegal.Owner.BirthDate.HasValue)
                return Failure<string>("La date de naissance de l'acheteur est requise pour le service de paiements.");
            
            if (!consumerLegal.Owner.Nationality.HasValue)
                return Failure<string>("La nationalité de l'acheteur est requise pour le service de paiements.");
            
            if (!consumerLegal.Owner.CountryOfResidence.HasValue)
                return Failure<string>("Le pays de résidence de l'acheteur est requise pour le service de paiements.");
            
            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Users.UpdateNaturalAsync(
                new UserNaturalPutDTO
                {
                    Email = consumerLegal.Owner.Email,
                    FirstName = consumerLegal.Owner.FirstName,
                    LastName = consumerLegal.Owner.LastName,
                    Birthday = consumerLegal.Owner.BirthDate.Value.DateTime,
                    Nationality = consumerLegal.Owner.Nationality.Value.GetCountry(),
                    CountryOfResidence = consumerLegal.Owner.CountryOfResidence.Value.GetCountry(),
                    Address = consumerLegal.Owner.Address.GetAddress(),
                    Tag = $"Id='{consumerLegal.Id}'"
                }, consumerLegal.User.Identifier);

            return Success<string>(result.Id);
        }

        public async Task<Result<string>> CreateBusinessAsync(BusinessLegal businessLegal, CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(businessLegal.User.Identifier))
                return Failure<string>("Impossible de créer l'utilisateur, l'identifiant existe déjà.");

            if (!businessLegal.Owner.BirthDate.HasValue)
                return Failure<string>("La date de naissance de l'acheteur est requise pour le service de paiements.");
            
            if (!businessLegal.Owner.Nationality.HasValue)
                return Failure<string>("La nationalité de l'acheteur est requise pour le service de paiements.");
            
            if (!businessLegal.Owner.CountryOfResidence.HasValue)
                return Failure<string>("Le pays de résidence de l'acheteur est requise pour le service de paiements.");
            
            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Users.CreateAsync(GetIdempotencyKey(businessLegal.Id),
                new UserLegalPostDTO(
                    businessLegal.Email,
                    businessLegal.Name,
                    businessLegal.Kind.GetLegalPersonType(),
                    businessLegal.Owner.FirstName,
                    businessLegal.Owner.LastName,
                    businessLegal.Owner.BirthDate.Value.DateTime,
                    businessLegal.Owner.Nationality.Value.GetCountry(),
                    businessLegal.Owner.CountryOfResidence.Value.GetCountry())
                {
                    CompanyNumber = businessLegal.Siret,
                    HeadquartersAddress = businessLegal.Address.GetAddress(),
                    LegalRepresentativeAddress = businessLegal.Owner.Address.GetAddress(),
                    LegalRepresentativeEmail = businessLegal.Owner.Email,
                    Tag = $"Id='{businessLegal.Id}'"
                });

            return Success<string>(result.Id);
        }

        public async Task<Result<string>> UpdateBusinessAsync(BusinessLegal businessLegal, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(businessLegal.User.Identifier))
                return Failure<string>("Impossible de mettre à jour l'utilisateur, l'identifiant est requis.");

            if (!businessLegal.Owner.BirthDate.HasValue)
                return Failure<string>("La date de naissance de l'acheteur est requise pour le service de paiements.");
            
            if (!businessLegal.Owner.Nationality.HasValue)
                return Failure<string>("La nationalité de l'acheteur est requise pour le service de paiements.");
            
            if (!businessLegal.Owner.CountryOfResidence.HasValue)
                return Failure<string>("Le pays de résidence de l'acheteur est requise pour le service de paiements.");
            
            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Users.UpdateLegalAsync(
                new UserLegalPutDTO
                {
                    Email = businessLegal.Email,
                    Name = businessLegal.Name,
                    LegalPersonType = businessLegal.Kind.GetLegalPersonType(),
                    LegalRepresentativeFirstName = businessLegal.Owner.FirstName,
                    LegalRepresentativeLastName = businessLegal.Owner.LastName,
                    LegalRepresentativeBirthday = businessLegal.Owner.BirthDate.Value.DateTime,
                    LegalRepresentativeNationality = businessLegal.Owner.Nationality.Value.GetCountry(),
                    LegalRepresentativeCountryOfResidence = businessLegal.Owner.CountryOfResidence.Value.GetCountry(),
                    CompanyNumber = businessLegal.Siret,
                    HeadquartersAddress = businessLegal.Address.GetAddress(),
                    LegalRepresentativeAddress = businessLegal.Owner.Address.GetAddress(),
                    LegalRepresentativeEmail = businessLegal.Owner.Email,
                    Tag = $"Id='{businessLegal.Id}'"
                }, businessLegal.User.Identifier);

            return Success<string>(result.Id);
        }

        public async Task<Result<string>> CreateWalletAsync(Wallet wallet, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(wallet.User.Identifier))
                return Failure<string>("Impossible de créer le wallet, l'utilisateur est requis.");

            if (!string.IsNullOrWhiteSpace(wallet.Identifier))
                return Failure<string>("Impossible de créer le wallet, l'identifiant existe déjà.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Wallets.CreateAsync(
                GetIdempotencyKey(wallet.Id),
                new WalletPostDTO(new List<string> {wallet.User.Identifier},
                    wallet.Name,
                    CurrencyIso.EUR)
                {
                    Tag = $"Id='{wallet.Id}'"
                });

            return Success<string>(result.Id);
        }

        public async Task<Result<string>> CreateBankIbanAsync(BankAccount payment, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(payment.User.Identifier))
                return Failure<string>("Impossible de créer le compte en banque, l'utilisateur est requis.");

            if (!string.IsNullOrWhiteSpace(payment.Identifier) && payment.Identifier.Length > 0)
                return Failure<string>("Impossible de créer le compte en banque, l'identifiant existe déjà.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Users.CreateBankAccountIbanAsync(payment.User.Identifier,
                new BankAccountIbanPostDTO(
                    payment.Owner,
                    new MangoPay.SDK.Entities.Address
                    {
                        AddressLine1 = payment.Line1,
                        AddressLine2 = payment.Line2,
                        PostalCode = payment.Zipcode,
                        City = payment.City,
                        Country = payment.Country.GetCountry()
                    },
                    payment.IBAN)
                {
                    BIC = payment.BIC,
                    Type = BankAccountType.IBAN,
                    Tag = $"Id='{payment.Id}'"
                });

            return Success<string>(result.Id);
        }

        public async Task<Result<bool>> UpdateBankIbanAsync(BankAccount payment, bool isActive, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(payment.User.Identifier))
                return Failure<bool>("Impossible de mettre à jour le compte en banque, l'utilisateur est requis.");

            if (string.IsNullOrWhiteSpace(payment.Identifier))
                return Failure<bool>("Impossible de mettre à jour le compte en banque, l'identifiant est requis.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Users.UpdateBankAccountAsync(payment.User.Identifier,
                new DisactivateBankAccountPutDTO
                {
                    Active = isActive
                }, payment.Identifier);

            return Success(result.Active);
        }

        public async Task<Result<PspCardRegistrationResultDto>> CreateCardRegistrationAsync(User user,
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(user.Identifier))
                return Failure<PspCardRegistrationResultDto>("Impossible de créer la carte, l'utilisateur est requis.");

            await EnsureAccessTokenIsValidAsync(token);
            var result =
                await _api.CardRegistrations.CreateAsync(new CardRegistrationPostDTO(user.Identifier, CurrencyIso.EUR));

            return Success(new PspCardRegistrationResultDto
            {
                Id = result.Id,
                AccessKey = result.AccessKey,
                CardId = result.CardId,
                CardRegistrationURL = result.CardRegistrationURL,
                PreregistrationData = result.PreregistrationData,
                UserId = result.UserId
            });
        }

        public async Task<Result<PspDocumentResultDto>> CreateDocumentAsync(Document document, string userIdentifier,
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(userIdentifier))
                return Failure<PspDocumentResultDto>("Impossible de créer le document, l'utilisateur est requis.");

            if (!string.IsNullOrWhiteSpace(document.Identifier))
                return Failure<PspDocumentResultDto>("Impossible de créer le document, l'identifiant existe déjà.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Users.CreateKycDocumentAsync(GetIdempotencyKey(document.Id), userIdentifier,
                document.Kind.GetDocumentType());

            return Success(new PspDocumentResultDto
            {
                Identifier = result.Id,
                ProcessedOn = result.ProcessedDate,
                ResultCode = result.RefusedReasonType,
                ResultMessage =
                    PspExtensions.GetOperationMessage(result.RefusedReasonType, result.RefusedReasonMessage),
                Status = result.Status.GetValidationStatus()
            });
        }

        public async Task<Result<bool>> AddPageToDocumentAsync(Page page, Document document, string userIdentifier,
            byte[] bytes, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(document.Identifier))
                return Failure<bool>("Impossible d'ajouter la page au document, l'identifiant est requis.");

            await EnsureAccessTokenIsValidAsync(token);

            await _api.Users.CreateKycPageAsync(GetIdempotencyKey(page.Id), userIdentifier, document.Identifier, bytes);
            return Success(true);
        }

        public async Task<Result<PspDocumentResultDto>> SubmitDocumentAsync(Document document, string userIdentifier,
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(userIdentifier))
                return Failure<PspDocumentResultDto>("Impossible d'envoyer le document, l'utilisateur est requis.");

            if (string.IsNullOrWhiteSpace(document.Identifier))
                return Failure<PspDocumentResultDto>("Impossible d'envoyer le document, l'identifiant est requis.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Users.UpdateKycDocumentAsync(
                userIdentifier,
                new KycDocumentPutDTO {Status = KycStatus.VALIDATION_ASKED},
                document.Identifier);

            return Success(new PspDocumentResultDto
            {
                Identifier = result.Id,
                ProcessedOn = result.ProcessedDate,
                ResultCode = result.RefusedReasonType,
                ResultMessage =
                    PspExtensions.GetOperationMessage(result.RefusedReasonType, result.RefusedReasonMessage),
                Status = result.Status.GetValidationStatus()
            });
        }

        public async Task<Result<PspDeclarationResultDto>> CreateUboDeclarationAsync(Declaration declaration,
            User business, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(business.Identifier))
                return Failure<PspDeclarationResultDto>("Impossible de créer la déclaration, l'utilisateur est requis.");

            await EnsureAccessTokenIsValidAsync(token);

            var result =
                await _api.UboDeclarations.CreateUboDeclarationAsync(GetIdempotencyKey(declaration.Id),
                    business.Identifier);

            return Success(new PspDeclarationResultDto
            {
                Identifier = result.Id,
                Status = result.Status.GetDeclarationStatus(),
                ResultMessage = PspExtensions.GetOperationMessage(result.Reason?.ToString("G"), result.Message),
                ProcessedOn = result.ProcessedDate,
                ResultCode = result.Reason?.ToString("G")
            });
        }

        public async Task<Result<PspDeclarationResultDto>> SubmitUboDeclarationAsync(Declaration declaration,
            User business, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(declaration.Identifier))
                return Failure<PspDeclarationResultDto>("Impossible d'envoyer la déclaration, l'identifiant est requis.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.UboDeclarations.UpdateUboDeclarationAsync(
                new UboDeclarationPutDTO(
                    declaration.Ubos.Select(u => new UboDTO
                    {
                        Id = u.Identifier,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Address = u.Address.GetAddress(),
                        Nationality = u.Nationality.GetCountry(),
                        Birthday = u.BirthDate.DateTime,
                        Birthplace = u.BirthPlace.GetBirthplace()
                    }).ToArray(),
                    UboDeclarationType.VALIDATION_ASKED),
                business.Identifier,
                declaration.Identifier);
            return Success(new PspDeclarationResultDto
            {
                Identifier = result.Id,
                Status = result.Status.GetDeclarationStatus(),
                ResultMessage = PspExtensions.GetOperationMessage(result.Reason?.ToString("G"), result.Message),
                ProcessedOn = result.ProcessedDate,
                ResultCode = result.Reason?.ToString("G")
            });
        }

        public async Task<Result<string>> CreateUboAsync(Ubo ubo, Declaration declaration, User business,
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(declaration.Identifier))
                return Failure<string>("Impossible d'ajouter l'ubo, l'identifiant de déclaration est requis.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.UboDeclarations.CreateUboAsync(GetIdempotencyKey(ubo.Id),
                new UboPostDTO(ubo.FirstName,
                    ubo.LastName,
                    ubo.Address.GetAddress(),
                    ubo.Nationality.GetCountry(),
                    ubo.BirthDate.DateTime,
                    ubo.BirthPlace.GetBirthplace()
                )
                {
                    Tag = $"Id='{ubo.Id}'"
                },
                business.Identifier,
                declaration.Identifier);

            return Success<string>(result.Id);
        }

        public async Task<Result<bool>> UpdateUboAsync(Ubo ubo, Declaration declaration, User business,
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(declaration.Identifier))
                return Failure<bool>("Impossible de mettre à jour l'ubo, l'identifiant de déclaration est requis.");

            if (string.IsNullOrWhiteSpace(ubo.Identifier))
                return Failure<bool>("Impossible de mettre à jour l'ubo, l'identifiant est requis.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.UboDeclarations.UpdateUboAsync(
                new UboPutDTO(ubo.FirstName,
                    ubo.LastName,
                    ubo.Address.GetAddress(),
                    ubo.Nationality.GetCountry(),
                    ubo.BirthDate.DateTime,
                    ubo.BirthPlace.GetBirthplace()
                ),
                business.Identifier,
                declaration.Identifier,
                ubo.Identifier);

            return Success(true);
        }

        public async Task<Result<PspPreAuthorizationResultDto>> CreatePreAuthorizationAsync(
            PreAuthorization preAuthorization, string ipAddress, BrowserInfoDto browserInfo, CancellationToken token)
        {
            await EnsureAccessTokenIsValidAsync(token);
            var result = await _api.CardPreAuthorizations.CreateAsync(
                GetIdempotencyKey(preAuthorization.Id),
                new CardPreAuthorizationPostDTO(
                    preAuthorization.Card.User.Identifier,
                    new Money
                    {
                        Amount = preAuthorization.Debited.GetAmount(),
                        Currency = CurrencyIso.EUR
                    },
                    SecureMode.DEFAULT,
                    preAuthorization.Card.Identifier,
                    preAuthorization.SecureModeReturnURL,
                    preAuthorization.Reference)
                {
                    Billing = new Billing
                    {
                        Address = preAuthorization.Card.User.Address.GetAddress()
                    },
                    IpAddress = ipAddress,
                    BrowserInfo = new BrowserInfo
                    {
                        AcceptHeader = browserInfo.AcceptHeader,
                        Language = browserInfo.Language,
                        ColorDepth = browserInfo.ColorDepth.ToString(),
                        JavaEnabled = browserInfo.JavaEnabled,
                        JavascriptEnabled = browserInfo.JavascriptEnabled,
                        ScreenHeight = browserInfo.ScreenHeight.ToString(),
                        ScreenWidth = browserInfo.ScreenWidth.ToString(),
                        UserAgent = browserInfo.UserAgent,
                        TimeZoneOffset = browserInfo.TimeZoneOffset,
                    }
                });

            return Success(new PspPreAuthorizationResultDto
            {
                Identifier = result.Id,
                ResultCode = result.ResultCode,
                ResultMessage = PspExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                Status = result.Status.GetPreAuthorizationStatus(),
                PaymentStatus = result.PaymentStatus.GetPaymentStatus(),
                ExpirationDate = result.ExpirationDate,
                Debited = result.DebitedFunds.Amount.GetAmount(),
                Remaining = result.RemainingFunds.Amount.GetAmount(),
                SecureModeRedirectUrl = result.SecureModeRedirectURL,
                SecureModeNeeded = result.SecureModeNeeded
            });
        }

        public async Task<Result<PspPreAuthorizationResultDto>> GetPreAuthorizationAsync(string identifier,
            CancellationToken token)
        {
            await EnsureAccessTokenIsValidAsync(token);
            var result = await _api.CardPreAuthorizations.GetAsync(identifier);

            return Success(new PspPreAuthorizationResultDto
            {
                Identifier = result.Id,
                ResultCode = result.ResultCode,
                ResultMessage = PspExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                Status = result.Status.GetPreAuthorizationStatus(),
                ExpirationDate = result.ExpirationDate,
                PaymentStatus = result.PaymentStatus.GetPaymentStatus(),
                Debited = result.DebitedFunds.Amount.GetAmount(),
                Remaining = result.RemainingFunds.Amount.GetAmount(),
                SecureModeRedirectUrl = result.SecureModeRedirectURL
            });
        }

        public async Task<Result<PspPaymentResultDto>> CreateTransferAsync(Transfer transaction,
            CancellationToken token)
        {
            return await CreatePspTransferAsync(transaction.Id,
                transaction.Debited,
                transaction.Fees,
                transaction.Author.Identifier,
                transaction.CreditedWallet.Identifier,
                transaction.CreditedWallet.User.Identifier,
                transaction.DebitedWallet.Identifier,
                token);
        }

        public async Task<Result<PspPaymentResultDto>> CreateDonationAsync(Donation transaction,
            CancellationToken token)
        {
            return await CreatePspTransferAsync(transaction.Id,
                transaction.Debited,
                transaction.Fees,
                transaction.Author.Identifier,
                transaction.CreditedWallet.Identifier,
                transaction.CreditedWallet.User.Identifier,
                transaction.DebitedWallet.Identifier,
                token);
        }

        public async Task<Result<PspPaymentResultDto>> CreateWithholdingAsync(Withholding transaction,
            CancellationToken token)
        {
            return await CreatePspTransferAsync(transaction.Id,
                transaction.Debited,
                transaction.Fees,
                transaction.Author.Identifier,
                transaction.CreditedWallet.Identifier,
                transaction.CreditedWallet.User.Identifier,
                transaction.DebitedWallet.Identifier,
                token);
        }

        public async Task<Result<PspPaymentResultDto>> CreatePayoutAsync(Payout transaction, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                return Failure<PspPaymentResultDto>("Impossible de créer le virement psp, l'auteur est requis.");

            if (string.IsNullOrWhiteSpace(transaction.DebitedWallet.Identifier))
                return Failure<PspPaymentResultDto>("Impossible de créer le virement psp, le wallet débité est requis.");

            if (string.IsNullOrWhiteSpace(transaction.BankAccount.Identifier))
                return Failure<PspPaymentResultDto>("Impossible de créer le virement psp, l'identifiant du compte bancaire est requis.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.PayOuts.CreateBankWireAsync(GetIdempotencyKey(transaction.Id),
                new PayOutBankWirePostDTO(
                    transaction.Author.Identifier,
                    transaction.DebitedWallet.Identifier,
                    new Money
                    {
                        Amount = transaction.Debited.GetAmount(),
                        Currency = CurrencyIso.EUR
                    },
                    new Money
                    {
                        Amount = transaction.Fees.GetAmount(),
                        Currency = CurrencyIso.EUR
                    },
                    transaction.BankAccount.Identifier,
                    transaction.Reference,
                    PayOutPaymentType.BANK_WIRE.ToString("G"))
                {
                    Tag = $"Id='{transaction.Id}'"
                });

            return Success(new PspPaymentResultDto
            {
                Credited = result.CreditedFunds.Amount.GetAmount(),
                ProcessedOn = result.ExecutionDate,
                Identifier = result.Id,
                ResultCode = result.ResultCode,
                ResultMessage = PspExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                Debited = result.DebitedFunds.Amount.GetAmount(),
                Fees = result.Fees.Amount.GetAmount(),
                Status = result.Status.GetTransactionStatus()
            });
        }

        public async Task<Result<PspPaymentResultDto>> RefundPayinAsync(PayinRefund transaction,
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                return Failure<PspPaymentResultDto>("Impossible de créer le remboursement du paiement psp, l'auteur est requis.");

            if (string.IsNullOrWhiteSpace(transaction.Payin.Identifier))
                return Failure<PspPaymentResultDto>("Impossible de créer le remboursement du paiement psp, l'identifiant est requis.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.PayIns.CreateRefundAsync(GetIdempotencyKey(transaction.Id),
                transaction.Payin.Identifier,
                new RefundPayInPostDTO(
                    transaction.Author.Identifier,
                    new Money
                    {
                        Amount = transaction.Fees.GetAmount(),
                        Currency = CurrencyIso.EUR
                    },
                    new Money
                    {
                        Amount = transaction.Debited.GetAmount(),
                        Currency = CurrencyIso.EUR
                    })
                {
                    Tag = $"Id='{transaction.Id}'"
                });

            return Success(new PspPaymentResultDto
            {
                Credited = result.CreditedFunds.Amount.GetAmount(),
                ProcessedOn = result.ExecutionDate,
                Identifier = result.Id,
                ResultCode = result.ResultCode,
                ResultMessage = PspExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                Debited = result.DebitedFunds.Amount.GetAmount(),
                Fees = result.Fees.Amount.GetAmount(),
                Status = result.Status.GetTransactionStatus()
            });
        }

        public async Task<Result<PspDocumentResultDto>> GetDocumentAsync(string identifier, CancellationToken token)
        {
            await EnsureAccessTokenIsValidAsync(token);
            var result = await _api.Kyc.GetAsync(identifier);

            return Success(new PspDocumentResultDto
            {
                Identifier = result.Id,
                ResultCode = result.RefusedReasonType,
                ResultMessage =
                    PspExtensions.GetOperationMessage(result.RefusedReasonType, result.RefusedReasonMessage),
                Status = result.Status.GetValidationStatus(),
                ProcessedOn = result.ProcessedDate
            });
        }

        public async Task<Result<PspDeclarationResultDto>> GetDeclarationAsync(string identifier,
            CancellationToken token)
        {
            await EnsureAccessTokenIsValidAsync(token);
            var result = await _api.UboDeclarations.GetUboDeclarationByIdAsync(identifier);

            return Success(new PspDeclarationResultDto
            {
                Identifier = result.Id,
                ResultCode = result.Reason?.ToString("G"),
                ResultMessage = PspExtensions.GetOperationMessage(result.Reason?.ToString("G"), result.Message),
                Status = result.Status.GetDeclarationStatus(),
                ProcessedOn = result.ProcessedDate
            });
        }

        public async Task<Result<PspTransactionResultDto>> GetPayinAsync(string identifier, CancellationToken token)
        {
            await EnsureAccessTokenIsValidAsync(token);
            var result = await _api.PayIns.GetAsync(identifier);

            return Success(new PspTransactionResultDto
            {
                Identifier = result.Id,
                ResultCode = result.ResultCode,
                ResultMessage = PspExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                Status = result.Status.GetTransactionStatus(),
                ProcessedOn = result.ExecutionDate
            });
        }

        public async Task<Result<PspTransactionResultDto>> GetTransferAsync(string identifier, CancellationToken token)
        {
            await EnsureAccessTokenIsValidAsync(token);
            var result = await _api.Transfers.GetAsync(identifier);

            return Success(new PspTransactionResultDto
            {
                Identifier = result.Id,
                ResultCode = result.ResultCode,
                ResultMessage = PspExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                Status = result.Status.GetTransactionStatus(),
                ProcessedOn = result.ExecutionDate
            });
        }

        public async Task<Result<PspTransactionResultDto>> GetRefundAsync(string identifier, CancellationToken token)
        {
            await EnsureAccessTokenIsValidAsync(token);
            var result = await _api.Refunds.GetAsync(identifier);

            return Success(new PspTransactionResultDto
            {
                Identifier = result.Id,
                ResultCode = result.ResultCode,
                ResultMessage = PspExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                Status = result.Status.GetTransactionStatus(),
                ProcessedOn = result.ExecutionDate
            });
        }

        public async Task<Result<PspTransactionResultDto>> GetPayoutAsync(string identifier, CancellationToken token)
        {
            await EnsureAccessTokenIsValidAsync(token);
            var result = await _api.PayOuts.GetAsync(identifier);

            return Success(new PspTransactionResultDto
            {
                Identifier = result.Id,
                ResultCode = result.ResultCode,
                ResultMessage = PspExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                Status = result.Status.GetTransactionStatus(),
                ProcessedOn = result.ExecutionDate
            });
        }

        public async Task<Result<PspPaymentResultDto>> CreatePreAuthorizedPayinAsync(PreAuthorization preAuthorization, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(preAuthorization.PreAuthorizedPayin.Author.Identifier))
                return Failure<PspPaymentResultDto>("Impossible de créer la pré-authorisation psp, l'auteur est requis.");

            if (string.IsNullOrWhiteSpace(preAuthorization.PreAuthorizedPayin.CreditedWallet.Identifier))
                return Failure<PspPaymentResultDto>("Impossible de créer la pré-authorisation psp, le wallet crédité est requis.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.PayIns.CreatePreauthorizedDirectAsync(GetIdempotencyKey(preAuthorization.Id),
                new PayInPreauthorizedDirectPostDTO(
                    preAuthorization.PreAuthorizedPayin.Author.Identifier,
                    new Money
                    {
                        Amount = preAuthorization.PreAuthorizedPayin.Debited.GetAmount(),
                        Currency = CurrencyIso.EUR
                    },
                    new Money
                    {
                        Amount = preAuthorization.PreAuthorizedPayin.Fees.GetAmount(),
                        Currency = CurrencyIso.EUR
                    },
                    preAuthorization.PreAuthorizedPayin.CreditedWallet.Identifier,
                    preAuthorization.Identifier)
                {
                    Tag = $"Id='{preAuthorization.PreAuthorizedPayin.Id}'"
                });

            return Success(new PspPaymentResultDto
            {
                Credited = result.CreditedFunds.Amount.GetAmount(),
                ProcessedOn = result.ExecutionDate,
                Identifier = result.Id,
                ResultCode = result.ResultCode,
                ResultMessage = PspExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                Debited = result.DebitedFunds.Amount.GetAmount(),
                Fees = result.Fees.Amount.GetAmount(),
                Status = result.Status.GetTransactionStatus()
            });
        }

        private async Task<Result<PspPaymentResultDto>> CreatePspTransferAsync(Guid idempotencyKey, decimal debited,
            decimal fees, string author, string creditedWalletIdentifier, string creditedUserIdentifier,
            string debitedWalletIdentifier, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(author))
                return Failure<PspPaymentResultDto>("Impossible de créer le transfer psp, l'auteur est requis.");

            if (string.IsNullOrWhiteSpace(creditedWalletIdentifier))
                return Failure<PspPaymentResultDto>("Impossible de créer le transfer psp, le wallet crédité est requis.");

            if (string.IsNullOrWhiteSpace(debitedWalletIdentifier))
                return Failure<PspPaymentResultDto>("Impossible de créer le transfer psp, le wallet débité est requis.");

            await EnsureAccessTokenIsValidAsync(token);

            var result = await _api.Transfers.CreateAsync(GetIdempotencyKey(idempotencyKey),
                new TransferPostDTO(
                    author,
                    creditedUserIdentifier,
                    new Money
                    {
                        Amount = debited.GetAmount(),
                        Currency = CurrencyIso.EUR
                    },
                    new Money
                    {
                        Amount = fees.GetAmount(),
                        Currency = CurrencyIso.EUR
                    },
                    debitedWalletIdentifier,
                    creditedWalletIdentifier)
                {
                    Tag = $"Id='{idempotencyKey}'"
                });

            return Success(new PspPaymentResultDto
            {
                Credited = result.CreditedFunds.Amount.GetAmount(),
                ProcessedOn = result.ExecutionDate,
                Identifier = result.Id,
                ResultCode = result.ResultCode,
                ResultMessage = PspExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                Debited = result.DebitedFunds.Amount.GetAmount(),
                Fees = result.Fees.Amount.GetAmount(),
                Status = result.Status.GetTransactionStatus()
            });
        }

        private async Task EnsureAccessTokenIsValidAsync(CancellationToken token)
        {
            var oauthToken = await _api.OAuthTokenManager.GetTokenAsync();
            if (oauthToken != null)
            {
                var date = new DateTime(oauthToken.create_time).AddSeconds(oauthToken.expires_in);
                if (date > DateTime.UtcNow)
                    return;
            }

            oauthToken = await _api.AuthenticationManager.CreateTokenAsync();
            _api.OAuthTokenManager.StoreToken(oauthToken);
        }

        private string GetIdempotencyKey(Guid id)
        {
            if (_pspOptions.SkipIdempotencyKey)
                return Guid.NewGuid().ToString("N");

            return id.ToString("N");
        }
    }

    internal static class PspExtensions
    {
        public static PspUserNormalDto GetConsumer(this UserNaturalDTO user)
        {
            if (user == null)
                return null;

            return new PspUserNormalDto
            {
                KYCLevel = user.KYCLevel.GetLevel(),
                ProofOfAddress = user.ProofOfAddress,
                Address = user.Address.GetAddress(),
                AddressObsolete = user.AddressObsolete,
                Birthday = user.Birthday,
                Birthplace = user.Birthplace,
                CountryOfResidence = user.CountryOfResidence.GetCountry(),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Nationality = user.Nationality.GetCountry(),
                ProofOfIdentity = user.ProofOfIdentity
            };
        }

        public static PspUserLegalDto GetCompany(this UserLegalDTO user)
        {
            if (user == null)
                return null;

            return new PspUserLegalDto
            {
                Email = user.Email,
                KYCLevel = user.KYCLevel.GetLevel(),
                CompanyNumber = user.CompanyNumber,
                HeadquartersAddress = user.HeadquartersAddress.GetAddress(),
                HeadquartersAddressObsolete = user.HeadquartersAddressObsolete,
                LegalPersonType = user.LegalPersonType.GetLegalKind(),
                LegalRepresentativeAddress = user.LegalRepresentativeAddress.GetAddress(),
                LegalRepresentativeAddressObsolete = user.LegalRepresentativeAddressObsolete,
                LegalRepresentativeBirthday = user.LegalRepresentativeBirthday,
                LegalRepresentativeCountryOfResidence = user.LegalRepresentativeCountryOfResidence.GetCountry(),
                LegalRepresentativeEmail = user.LegalRepresentativeEmail,
                LegalRepresentativeFirstName = user.LegalRepresentativeFirstName,
                LegalRepresentativeLastName = user.LegalRepresentativeLastName,
                LegalRepresentativeNationality = user.LegalRepresentativeNationality.GetCountry(),
                LegalRepresentativeProofOfIdentity = user.LegalRepresentativeProofOfIdentity,
                Name = user.Name,
                ProofOfRegistration = user.ProofOfRegistration,
                ShareholderDeclaration = user.ShareholderDeclaration,
                Statute = user.Statute
            };
        }

        public static long GetAmount(this decimal amount)
        {
            return (long) (amount * 100);
        }

        public static decimal GetAmount(this long amount)
        {
            return (decimal) (amount / 100.00);
        }

        public static LegalValidation GetLevel(this KycLevel status)
        {
            return (LegalValidation) status;
        }

        public static CountryIsoCode GetCountry(this CountryIso code)
        {
            return (CountryIsoCode) code;
        }

        public static DocumentStatus GetValidationStatus(this KycStatus status)
        {
            return (DocumentStatus) status;
        }

        public static DeclarationStatus GetDeclarationStatus(this UboDeclarationType status)
        {
            return (DeclarationStatus) status;
        }

        public static TransactionStatus GetTransactionStatus(
            this MangoPay.SDK.Core.Enumerations.TransactionStatus status)
        {
            return (TransactionStatus) status;
        }

        public static LegalKind GetLegalKind(this LegalPersonType kind)
        {
            switch (kind)
            {
                case LegalPersonType.BUSINESS:
                    return LegalKind.Business;
                case LegalPersonType.SOLETRADER:
                    return LegalKind.Individual;
                case LegalPersonType.ORGANIZATION:
                    return LegalKind.Organization;
                case LegalPersonType.NotSpecified:
                default:
                    return LegalKind.Natural;
            }
        }

        public static LegalPersonType GetLegalPersonType(this LegalKind kind)
        {
            switch (kind)
            {
                case LegalKind.Business:
                    return LegalPersonType.BUSINESS;
                case LegalKind.Individual:
                    return LegalPersonType.SOLETRADER;
                case LegalKind.Organization:
                    return LegalPersonType.ORGANIZATION;
                case LegalKind.Natural:
                default:
                    return LegalPersonType.NotSpecified;
            }
        }

        public static KycDocumentType GetDocumentType(this DocumentKind kind)
        {
            switch (kind)
            {
                case DocumentKind.AddressProof:
                    return KycDocumentType.ADDRESS_PROOF;
                case DocumentKind.AssociationProof:
                    return KycDocumentType.ARTICLES_OF_ASSOCIATION;
                case DocumentKind.IdentityProof:
                    return KycDocumentType.IDENTITY_PROOF;
                case DocumentKind.RegistrationProof:
                    return KycDocumentType.REGISTRATION_PROOF;
                case DocumentKind.ShareholderProof:
                    return KycDocumentType.SHAREHOLDER_DECLARATION;
                default:
                    return KycDocumentType.NotSpecified;
            }
        }

        public static MangoPay.SDK.Entities.Address GetAddress(this Address address)
        {
            if (address == null)
                return null;

            return new MangoPay.SDK.Entities.Address
            {
                AddressLine1 = address.Line1,
                AddressLine2 = address.Line2,
                City = address.City,
                Country = address.Country.GetCountry(),
                PostalCode = address.Zipcode
            };
        }

        public static AddressDto GetAddress(this MangoPay.SDK.Entities.Address address)
        {
            if (address == null)
                return null;

            return new AddressDto
            {
                Line1 = address.AddressLine1,
                Line2 = address.AddressLine2,
                City = address.City,
                Country = address.Country.GetCountryCode(),
                Zipcode = address.PostalCode
            };
        }

        public static Birthplace GetBirthplace(this BirthAddress address)
        {
            if (address == null)
                return null;

            return new Birthplace
            {
                City = address.City,
                Country = address.Country.GetCountry()
            };
        }

        public static CountryIsoCode GetCountryCode(this CountryIso? countryCode)
        {
            return (CountryIsoCode) countryCode;
        }

        public static CountryIso GetCountry(this CountryIsoCode countryCode)
        {
            return (CountryIso) countryCode;
        }

        public static Sheaft.Domain.Enum.PaymentStatus GetPaymentStatus(
            this MangoPay.SDK.Core.Enumerations.PaymentStatus status)
        {
            return (Sheaft.Domain.Enum.PaymentStatus) status;
        }

        public static Sheaft.Domain.Enum.PreAuthorizationStatus GetPreAuthorizationStatus(
            this MangoPay.SDK.Core.Enumerations.PreAuthorizationStatus status)
        {
            return (Sheaft.Domain.Enum.PreAuthorizationStatus) status;
        }

        public static string GetOperationMessage(string code, string message)
        {
            switch (code)
            {
                case "001030":
                case "001033":
                    return
                        "La redirection vers la page de paiement ne s'est pas déroulée correctement et a expirée, veuillez renouveler votre demande.";
                case "001031":
                case "101002":
                    return
                        "Le processus de paiement a été annulé à votre initiative.";
                case "001034":
                case "101001":
                    return "La page de paiement a expirée.";
                case "101399":
                    return "Le mode 3DSecure n'est pas disponible, veuillez contacter notre support.";
                case "101304":
                    return "La validation 3DSecure a expirée.";
                case "101303":
                    return "Votre carte n'est pas compatible avec 3DSecure.";
                case "101302":
                    return "Votre carte ne supporte pas le mode 3DSecure.";
                case "101301":
                    return "L'authentification 3DSecure a échouée.";
                case "001999":
                    return "Un incident ou un problème de connexion a annulé l'opération.";
                case "001001":
                    return "Fond insuffisant dans le porte-monnaie numérique.";
                case "001002":
                    return "L'utilisateur renseigné, n'est pas le propriétaire du porte-monnaie numérique";
                case "001011":
                    return "Le montant de la transaction est trop elevé.";
                case "001012":
                    return "Le montant de la transaction est trop faible.";
                case "001013":
                    return "Le montant de la transaction est invalide.";
                case "001014":
                    return "Le montant doit être supérieur à 0€.";
                default:
                    return message;
            }
        }
    }
}