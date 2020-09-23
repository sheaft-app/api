using MangoPay.SDK;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;
using Sheaft.Options;
using Sheaft.Application.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Infrastructure.Services
{
    public class PspService : ResultsHandler, IPspService
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

        public async Task<Result<string>> CreateConsumerAsync(ConsumerLegal consumerLegal, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (!string.IsNullOrWhiteSpace(consumerLegal.Consumer.Identifier))
                    return Failed<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_User_User_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Users.CreateAsync(consumerLegal.Id.ToString("N"),
                    new UserNaturalPostDTO(
                        consumerLegal.Owner.Email,
                        consumerLegal.Owner.FirstName,
                        consumerLegal.Owner.LastName,
                        consumerLegal.Owner.BirthDate.DateTime,
                        consumerLegal.Owner.Nationality.GetCountry(),
                        consumerLegal.Owner.CountryOfResidence.GetCountry())
                    {
                        Address = consumerLegal.Owner.Address.GetAddress()
                    });

                return Ok(result.Id);
            });
        }

        public async Task<Result<string>> CreateBusinessAsync(BusinessLegal businessLegal, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (!string.IsNullOrWhiteSpace(businessLegal.Business.Identifier))
                    return Failed<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_User_User_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Users.CreateAsync(businessLegal.Id.ToString("N"),
                    new UserLegalPostDTO(
                        businessLegal.Email,
                        businessLegal.Business.Name,
                        businessLegal.Kind.GetLegalPersonType(),
                        businessLegal.Owner.FirstName,
                        businessLegal.Owner.LastName,
                        businessLegal.Owner.BirthDate.DateTime,
                        businessLegal.Owner.Nationality.GetCountry(),
                        businessLegal.Owner.CountryOfResidence.GetCountry())
                    {
                        CompanyNumber = businessLegal.Siret,
                        HeadquartersAddress = businessLegal.Address.GetAddress(),
                        LegalRepresentativeAddress = businessLegal.Owner.Address.GetAddress(),
                        LegalRepresentativeEmail = businessLegal.Owner.Email
                    });

                return Ok(result.Id);
            });
        }

        public async Task<Result<string>> CreateWalletAsync(Wallet wallet, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(wallet.User.Identifier))
                    return Failed<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Wallet_User_Not_Exists));

                if (!string.IsNullOrWhiteSpace(wallet.Identifier))
                    return Failed<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Wallet_Wallet_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Wallets.CreateAsync(
                    wallet.Id.ToString("N"),
                    new WalletPostDTO(new List<string> { wallet.User.Identifier },
                    wallet.Name,
                    CurrencyIso.EUR));

                return Ok(result.Id);
            });
        }

        public async Task<Result<string>> CreateBankIbanAsync(BankAccount payment, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(payment.User.Identifier))
                    return Failed<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Transfer_User_Not_Exists));

                if (!string.IsNullOrWhiteSpace(payment.Identifier))
                    return Failed<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Transfer_BankIBAN_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Users.CreateBankAccountIbanAsync(payment.Id.ToString("N"),
                    new BankAccountIbanPostDTO(
                        payment.Owner,
                        new Address
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
                        UserId = payment.User.Identifier
                    });

                return Ok(result.Id);
            });
        }

        public async Task<Result<KeyValuePair<string, string>>> CreateCardAsync(Card payment, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(payment.User.Identifier))
                    return Failed<KeyValuePair<string, string>>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Card_User_Not_Exists));

                if (!string.IsNullOrWhiteSpace(payment.Identifier))
                    return Failed<KeyValuePair<string, string>>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Card_Card_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.CardRegistrations.CreateAsync(
                    payment.Id.ToString("N"),
                    new CardRegistrationPostDTO(payment.User.Identifier, CurrencyIso.EUR, CardType.CB_VISA_MASTERCARD));

                return Ok(new KeyValuePair<string, string>(result.Id, result.CardId));
            });
        }

        public async Task<Result<string>> ValidateCardAsync(Card payment, string registrationId, string registrationData, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(payment.Identifier))
                    return Failed<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotValidate_Card_Card_Not_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.CardRegistrations.UpdateAsync(new CardRegistrationPutDTO() { RegistrationData = registrationData }, registrationId);
                return Ok(result.Id);
            });
        }

        public async Task<Result<PspDocumentResultDto>> CreateDocumentAsync(Document document, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(document.User.Identifier))
                    return Failed<PspDocumentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Document_User_Not_Exists));

                if (!string.IsNullOrWhiteSpace(document.Identifier))
                    return Failed<PspDocumentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Document_Document_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Users.CreateKycDocumentAsync(document.Id.ToString("N"), document.User.Identifier, document.Kind.GetDocumentType());

                return Ok(new PspDocumentResultDto
                {
                    Identifier = result.Id,
                    ProcessedOn = result.ProcessedDate,
                    ResultCode = result.RefusedReasonType,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.RefusedReasonType, result.RefusedReasonMessage),
                    Status = result.Status.GetValidationStatus()
                });
            });
        }

        public async Task<Result<bool>> AddPageToDocumentAsync(Page page, Document document, Stream data, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(document.Identifier))
                    return Failed<bool>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_DocumentPage_Document_Not_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                byte[] bytes = null;
                await _api.Users.CreateKycPageAsync(page.Id.ToString("N"), document.User.Identifier, document.Identifier, bytes);
                return Ok(true);
            });
        }

        public async Task<Result<PspDocumentResultDto>> SubmitDocumentAsync(Document document, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(document.User.Identifier))
                    return Failed<PspDocumentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotSubmit_Document_User_Not_Exists));

                if (string.IsNullOrWhiteSpace(document.Identifier))
                    return Failed<PspDocumentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotSubmit_Document_Document_Not_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Users.UpdateKycDocumentAsync(
                    document.User.Identifier,
                    new KycDocumentPutDTO { Status = KycStatus.VALIDATION_ASKED },
                    document.Identifier);

                return Ok(new PspDocumentResultDto
                {
                    Identifier = result.Id,
                    ProcessedOn = result.ProcessedDate,
                    ResultCode = result.RefusedReasonType,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.RefusedReasonType, result.RefusedReasonMessage),
                    Status = result.Status.GetValidationStatus()
                });
            });
        }

        public async Task<Result<PspDeclarationResultDto>> CreateUboDeclarationAsync(UboDeclaration declaration, Business business, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(business.Identifier))
                    return Failed<PspDeclarationResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Declaration_User_Not_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.UboDeclarations.CreateUboDeclarationAsync(declaration.Id.ToString("N"), business.Identifier);
                return Ok(new PspDeclarationResultDto
                {
                    Identifier = result.Id,
                    Status = result.Status.GetDeclarationStatus(),
                    ResultMessage = MangoExtensions.GetOperationMessage(result.Reason.ToString("G"), result.Message),
                    ProcessedOn = result.ProcessedDate,
                    ResultCode = result.Reason.ToString("G")
                });
            });
        }

        public async Task<Result<PspDeclarationResultDto>> SubmitUboDeclarationAsync(UboDeclaration declaration, Business business, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(declaration.Identifier))
                    return Failed<PspDeclarationResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotSubmit_Declaration_Not_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.UboDeclarations.UpdateUboDeclarationAsync(
                    new UboDeclarationPutDTO(
                        declaration.Ubos.Select(u => new UboDTO {
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
                return Ok(new PspDeclarationResultDto
                {
                    Identifier = result.Id,
                    Status = result.Status.GetDeclarationStatus(),
                    ResultMessage = MangoExtensions.GetOperationMessage(result.Reason.ToString("G"), result.Message),
                    ProcessedOn = result.ProcessedDate,
                    ResultCode = result.Reason.ToString("G")
                });
            });
        }

        public async Task<Result<string>> CreateUboAsync(Ubo ubo, UboDeclaration declaration, Business business, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(declaration.Identifier))
                    return Failed<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotAddUbo_Declaration_Not_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.UboDeclarations.CreateUboAsync(ubo.Id.ToString("N"),
                    new UboPostDTO(ubo.FirstName,
                        ubo.LastName,
                        ubo.Address.GetAddress(),
                        ubo.Nationality.GetCountry(),
                        ubo.BirthDate.DateTime,
                        ubo.BirthPlace.GetBirthplace()
                    ),
                    business.Identifier,
                    declaration.Identifier);

                return Ok(result.Id);
            });
        }

        public async Task<Result<bool>> UpdateUboAsync(Ubo ubo, UboDeclaration declaration, Business business, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(declaration.Identifier))
                    return Failed<bool>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotUpdateUbo_Declaration_Not_Exists));

                if (string.IsNullOrWhiteSpace(ubo.Identifier))
                    return Failed<bool>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotUpdateUbo_Ubo_Not_Exists));

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

                return Ok(true);
            });
        }

        public async Task<Result<PspWebPaymentResultDto>> CreateWebPayinAsync(WebPayinTransaction transaction, Owner owner, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                    return Failed<PspWebPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_WebPayin_Author_Not_Exists));

                if (string.IsNullOrWhiteSpace(transaction.CreditedWallet.Identifier))
                    return Failed<PspWebPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_WebPayin_CreditedWallet_Not_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.PayIns.CreateCardWebAsync(transaction.Id.ToString("N"),
                    new PayInCardWebPostDTO(
                        transaction.Author.Identifier,
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
                        transaction.CreditedWallet.Identifier,
                        _pspOptions.ReturnUrl,
                        CultureCode.FR,
                        CardType.CB_VISA_MASTERCARD,
                        transaction.Reference)
                    {
                        SecureMode = SecureMode.DEFAULT,
                        TemplateURLOptionsCard = new TemplateURLOptionsCard {
                            PAYLINEV2 = _pspOptions.PaymentUrl 
                        }
                    });

                return Ok(new PspWebPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    RedirectUrl = result.RedirectURL,
                    ResultCode = result.ResultCode,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            });
        }

        public async Task<Result<PspPaymentResultDto>> CreateCardPayinAsync(CardPayinTransaction transaction, Owner owner, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                    return Failed<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_WebPayin_Author_Not_Exists));

                if (string.IsNullOrWhiteSpace(transaction.CreditedWallet.Identifier))
                    return Failed<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_WebPayin_CreditedWallet_Not_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.PayIns.CreateCardDirectAsync(transaction.Id.ToString("N"),
                    new PayInCardDirectPostDTO(
                        transaction.Author.Identifier,
                        transaction.CreditedWallet.User.Identifier,
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
                        transaction.CreditedWallet.Identifier,
                        _pspOptions.ReturnUrl,
                        transaction.Card.Identifier,
                        transaction.Reference,
                        new Billing
                        {
                            Address = owner.Address.GetAddress()
                        })
                    {
                        SecureMode = SecureMode.DEFAULT
                    });

                return Ok(new PspPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            });
        }

        public async Task<Result<PspPaymentResultDto>> CreateTransferAsync(TransferTransaction transaction, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                    return Failed<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Transfer_Author_Not_Exists));

                if (string.IsNullOrWhiteSpace(transaction.CreditedWallet.Identifier))
                    return Failed<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Transfer_CreditedWallet_Not_Exists));

                if (string.IsNullOrWhiteSpace(transaction.DebitedWallet.Identifier))
                    return Failed<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Transfer_DebitedWallet_Not_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Transfers.CreateAsync(transaction.Id.ToString("N"),
                    new TransferPostDTO(
                        transaction.Author.Identifier,
                        transaction.CreditedWallet.User.Identifier,
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
                        transaction.DebitedWallet.Identifier,
                        transaction.CreditedWallet.Identifier));

                return Ok(new PspPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            });
        }

        public async Task<Result<PspPaymentResultDto>> CreatePayoutAsync(PayoutTransaction transaction, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                    return Failed<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Payout_Author_Not_Exists));

                if (string.IsNullOrWhiteSpace(transaction.DebitedWallet.Identifier))
                    return Failed<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Payout_DebitedWallet_Not_Exists));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.PayOuts.CreateBankWireAsync(transaction.Id.ToString("N"),
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
                        transaction.Reference));

                return Ok(new PspPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            });
        }

        public async Task<Result<PspPaymentResultDto>> RefundPayinAsync(RefundPayinTransaction transaction, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                    return Failed<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotRefund_Payin_Author_Not_Exists));

                if (string.IsNullOrWhiteSpace(transaction.PayinTransaction.Identifier))
                    return Failed<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotRefund_Payin_PayinIdentifier_Missing));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.PayIns.CreateRefundAsync(transaction.Id.ToString("N"), transaction.PayinTransaction.Identifier,
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
                        }));

                return Ok(new PspPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            });
        }

        public async Task<Result<PspPaymentResultDto>> RefundTransferAsync(RefundTransferTransaction transaction, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                    return Failed<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotRefund_Transfer_Author_Not_Exists));

                if (string.IsNullOrWhiteSpace(transaction.TransferTransaction.Identifier))
                    return Failed<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotRefund_Transfer_TransferIdentifier_Missing));

                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Transfers.CreateRefundAsync(
                    transaction.Id.ToString("N"),
                    transaction.TransferTransaction.Identifier,
                    new RefundTransferPostDTO(transaction.Author.Identifier));

                return Ok(new PspPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            });
        }

        public async Task<Result<PspDocumentResultDto>> GetDocumentAsync(string identifier, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                await EnsureAccessTokenIsValidAsync(token);
                var result = await _api.Kyc.GetAsync(identifier);

                return Ok(new PspDocumentResultDto
                {
                    Identifier = result.Id,
                    ResultCode = result.RefusedReasonType,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.RefusedReasonType, result.RefusedReasonMessage),
                    Status = result.Status.GetValidationStatus(),
                    ProcessedOn = result.ProcessedDate
                });
            });
        }

        public async Task<Result<PspDeclarationResultDto>> GetDeclarationAsync(string identifier, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                await EnsureAccessTokenIsValidAsync(token);
                var result = await _api.UboDeclarations.GetUboDeclarationByIdAsync(identifier);

                return Ok(new PspDeclarationResultDto
                {
                    Identifier = result.Id,
                    ResultCode = result.Reason.ToString("G"),
                    ResultMessage = MangoExtensions.GetOperationMessage(result.Reason.ToString("G"), result.Message),
                    Status = result.Status.GetDeclarationStatus(),
                    ProcessedOn = result.ProcessedDate
                });
            });
        }

        public async Task<Result<PspTransactionResultDto>> GetPayinAsync(string identifier, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                await EnsureAccessTokenIsValidAsync(token);
                var result = await _api.PayIns.GetAsync(identifier);

                return Ok(new PspTransactionResultDto
                {
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                    Status = result.Status.GetTransactionStatus(),
                    ProcessedOn = result.ExecutionDate
                });
            });
        }

        public async Task<Result<PspTransactionResultDto>> GetTransferAsync(string identifier, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                await EnsureAccessTokenIsValidAsync(token);
                var result = await _api.Transfers.GetAsync(identifier);

                return Ok(new PspTransactionResultDto
                {
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                    Status = result.Status.GetTransactionStatus(),
                    ProcessedOn = result.ExecutionDate
                });
            });
        }

        public async Task<Result<PspTransactionResultDto>> GetRefundAsync(string identifier, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                await EnsureAccessTokenIsValidAsync(token);
                var result = await _api.Refunds.GetAsync(identifier);

                return Ok(new PspTransactionResultDto
                {
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                    Status = result.Status.GetTransactionStatus(),
                    ProcessedOn = result.ExecutionDate
                });
            });
        }

        public async Task<Result<PspTransactionResultDto>> GetPayoutAsync(string identifier, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                await EnsureAccessTokenIsValidAsync(token);
                var result = await _api.PayOuts.GetAsync(identifier);

                return Ok(new PspTransactionResultDto
                {
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = MangoExtensions.GetOperationMessage(result.ResultCode, result.ResultMessage),
                    Status = result.Status.GetTransactionStatus(),
                    ProcessedOn = result.ExecutionDate
                });
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
    }

    internal static class MangoExtensions
    {
        public static long GetAmount(this decimal amount)
        {
            return (long)(amount * 100);
        }

        public static decimal GetAmount(this long amount)
        {
            return (decimal)(amount / 100.00);
        }

        public static DocumentStatus GetValidationStatus(this KycStatus status)
        {
            return (DocumentStatus)status;
        }

        public static DeclarationStatus GetDeclarationStatus(this UboDeclarationType status)
        {
            return (DeclarationStatus)status;
        }

        public static Sheaft.Domain.Enums.TransactionStatus GetTransactionStatus(this MangoPay.SDK.Core.Enumerations.TransactionStatus status)
        {
            return (Sheaft.Domain.Enums.TransactionStatus)status;
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
        public static Address GetAddress(this BaseAddress address)
        {
            return new Address
            {
                AddressLine1 = address.Line1,
                AddressLine2 = address.Line2,
                City = address.City,
                Country = address.Country.GetCountry(),
                PostalCode = address.Zipcode
            };
        }
        public static Birthplace GetBirthplace(this BirthAddress address)
        {
            return new Birthplace
            {
                City = address.City,
                Country = address.Country.GetCountry()
            };
        }
        public static CountryIso GetCountry(this CountryIsoCode countryCode)
        {
            return (CountryIso)countryCode;
        }

        public static string GetOperationMessage(string code, string message)
        {
            switch (code)
            {
                case "001030":
                case "001033":
                    return "La redirection vers la page de paiement ne s'est pas déroulée correctement et a expirée, veuillez renouveler votre demande.";
                case "001031":
                case "101002":
                    return "Le processus de paiement a été annulé à votre initiative, vous pouvez renouveler votre demande.";
                case "001034":
                case "101001":
                    return "La page de paiement a expirée, veuillez renouveler votre demande.";
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
                    return "Le montant crédité doit être supérieur à 0.";
                default:
                    return message;
            }
        }
    }
}
