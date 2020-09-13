using MangoPay.SDK;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using Sheaft.Models.Dto;
using Sheaft.Options;
using Sheaft.Services.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Services
{
    public class PspService : IPspService
    {
        private readonly MangoPayApi _api;
        private readonly PspOptions _pspOptions;
        private readonly ILogger<BlobService> _logger;

        public PspService(
            MangoPayApi mangoPayApi,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<BlobService> logger
            )
        {
            _api = mangoPayApi;
            _logger = logger;
            _pspOptions = pspOptions.Value;

        }

        public async Task<Result<string>> CreateConsumerAsync(ConsumerLegal consumerLegal, CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(consumerLegal.Consumer.Identifier))
                return new Result<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_User_User_Exists));

            try
            {
                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Users.CreateAsync(consumerLegal.Id.ToString("N"), 
                    new UserNaturalPostDTO(
                        consumerLegal.Owner.Email,
                        consumerLegal.Owner.FirstName,
                        consumerLegal.Owner.LastName,
                        consumerLegal.Owner.Birthdate.DateTime,
                        consumerLegal.Owner.Nationality.GetCountry(),
                        consumerLegal.Owner.CountryOfResidence.GetCountry())
                    {
                        Address = consumerLegal.Owner.Address.GetAddress()  
                    });
                return new Result<string>(result.Id);
            }
            catch (Exception e)
            {
                return new Result<string>(e);
            }
        }

        public async Task<Result<string>> CreateBusinessAsync(BusinessLegal businessLegal, CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(businessLegal.Business.Identifier))
                return new Result<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_User_User_Exists));

            try
            {
                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Users.CreateAsync(businessLegal.Id.ToString("N"),
                    new UserLegalPostDTO(
                        businessLegal.Email,
                        businessLegal.Business.Name,
                        businessLegal.Kind.GetLegalPersonType(),
                        businessLegal.Owner.FirstName,
                        businessLegal.Owner.LastName,
                        businessLegal.Owner.Birthdate.DateTime,
                        businessLegal.Owner.Nationality.GetCountry(),
                        businessLegal.Owner.CountryOfResidence.GetCountry())
                    {
                        CompanyNumber = businessLegal.Business.Siret,
                        HeadquartersAddress = businessLegal.Address.GetAddress(),
                        LegalRepresentativeAddress = businessLegal.Owner.Address.GetAddress(),
                        LegalRepresentativeEmail = businessLegal.Owner.Email
                    });
                return new Result<string>(result.Id);
            }
            catch (Exception e)
            {
                return new Result<string>(e);
            }
        }

        public async Task<Result<string>> CreateWalletAsync(Wallet wallet, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(wallet.User.Identifier))
                return new Result<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Wallet_User_Not_Exists));

            if (!string.IsNullOrWhiteSpace(wallet.Identifier))
                return new Result<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Wallet_Wallet_Exists));

            try
            {
                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Wallets.CreateAsync(wallet.Id.ToString("N"), new WalletPostDTO(new List<string> { wallet.User.Identifier }, wallet.Name, CurrencyIso.EUR));
                return new Result<string>(result.Id);
            }
            catch (Exception e)
            {
                return new Result<string>(e);
            }
        }

        public async Task<Result<string>> CreateBankIbanAsync(BankAccount payment, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(payment.User.Identifier))
                return new Result<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Transfer_User_Not_Exists));

            if (!string.IsNullOrWhiteSpace(payment.Identifier))
                return new Result<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Transfer_BankIBAN_Exists));

            try
            {
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

                return new Result<string>(result.Id);
            }
            catch (Exception e)
            {
                return new Result<string>(e);
            }
        }

        public async Task<Result<KeyValuePair<string, string>>> CreateCardAsync(Card payment, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(payment.User.Identifier))
                return new Result<KeyValuePair<string, string>>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Card_User_Not_Exists));

            if (!string.IsNullOrWhiteSpace(payment.Identifier))
                return new Result<KeyValuePair<string, string>>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Card_Card_Exists));

            try
            {
                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.CardRegistrations.CreateAsync(payment.Id.ToString("N"), new CardRegistrationPostDTO(payment.User.Identifier, CurrencyIso.EUR, CardType.CB_VISA_MASTERCARD));
                return new Result<KeyValuePair<string, string>>(new KeyValuePair<string, string>(result.Id, result.CardId));
            }
            catch (Exception e)
            {
                return new Result<KeyValuePair<string, string>>(e);
            }
        }

        public async Task<Result<string>> ValidateCardAsync(Card payment, string registrationId, string registrationData, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(payment.Identifier))
                return new Result<string>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotValidate_Card_Card_Not_Exists));

            try
            {
                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.CardRegistrations.UpdateAsync(new CardRegistrationPutDTO() { RegistrationData = registrationData }, registrationId);
                return new Result<string>(result.Id);
            }
            catch (Exception e)
            {
                return new Result<string>(e);
            }
        }

        public async Task<Result<PspDocumentResultDto>> CreateDocumentAsync(Document document, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(document.User.Identifier))
                return new Result<PspDocumentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Document_User_Not_Exists));

            if (!string.IsNullOrWhiteSpace(document.Identifier))
                return new Result<PspDocumentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Document_Document_Exists));

            try
            {
                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Users.CreateKycDocumentAsync(document.Id.ToString("N"), document.User.Identifier, document.Kind.GetDocumentType());

                return new Result<PspDocumentResultDto>(new PspDocumentResultDto
                {
                    Identifier = result.Id,
                    ProcessedOn = result.ProcessedDate,
                    ResultCode = result.RefusedReasonType,
                    ResultMessage = result.RefusedReasonMessage,
                    Status = result.Status.GetValidationStatus()
                });
            }
            catch (Exception e)
            {
                return new Result<PspDocumentResultDto>(e);
            }
        }

        public async Task<Result<bool>> AddPageToDocumentAsync(Page page, Document document, Stream data, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(document.Identifier))
                return new Result<bool>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_DocumentPage_Document_Not_Exists));

            try
            {
                await EnsureAccessTokenIsValidAsync(token);

                byte[] bytes = null;
                await _api.Users.CreateKycPageAsync(page.Id.ToString("N"), document.User.Identifier, document.Identifier, bytes);
                return new Result<bool>(true);
            }
            catch (Exception e)
            {
                return new Result<bool>(e);
            }
        }

        public async Task<Result<PspDocumentResultDto>> SubmitDocumentAsync(Document document, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(document.User.Identifier))
                return new Result<PspDocumentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotSubmit_Document_User_Not_Exists));

            if (string.IsNullOrWhiteSpace(document.Identifier))
                return new Result<PspDocumentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotSubmit_Document_Document_Not_Exists));

            try
            {
                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Users.UpdateKycDocumentAsync(document.User.Identifier, new KycDocumentPutDTO { Status = KycStatus.VALIDATION_ASKED }, document.Identifier);
                return new Result<PspDocumentResultDto>(new PspDocumentResultDto
                {
                    Identifier = result.Id,
                    ProcessedOn = result.ProcessedDate,
                    ResultCode = result.RefusedReasonType,
                    ResultMessage = result.RefusedReasonMessage,
                    Status = result.Status.GetValidationStatus()
                });
            }
            catch (Exception e)
            {
                return new Result<PspDocumentResultDto>(e);
            }
        }

        public async Task<Result<PspWebPaymentResultDto>> CreateWebPayinAsync(WebPayinTransaction transaction, Owner owner, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                return new Result<PspWebPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_WebPayin_Author_Not_Exists));

            if (string.IsNullOrWhiteSpace(transaction.CreditedWallet.Identifier))
                return new Result<PspWebPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_WebPayin_CreditedWallet_Not_Exists));

            try
            {
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
                        TemplateURLOptionsCard = new TemplateURLOptionsCard { PAYLINEV2 = _pspOptions.PaiementUrl }
                    });

                return new Result<PspWebPaymentResultDto>(new PspWebPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    RedirectUrl = result.RedirectURL,
                    ResultCode = result.ResultCode,
                    ResultMessage = result.ResultMessage,
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            }
            catch (Exception e)
            {
                return new Result<PspWebPaymentResultDto>(e);
            }
        }

        public async Task<Result<PspPaymentResultDto>> CreateCardPayinAsync(CardPayinTransaction transaction, Owner owner, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                return new Result<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_WebPayin_Author_Not_Exists));

            if (string.IsNullOrWhiteSpace(transaction.CreditedWallet.Identifier))
                return new Result<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_WebPayin_CreditedWallet_Not_Exists));

            try
            {
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

                return new Result<PspPaymentResultDto>(new PspPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = result.ResultMessage,
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            }
            catch (Exception e)
            {
                return new Result<PspPaymentResultDto>(e);
            }
        }

        public async Task<Result<PspPaymentResultDto>> CreateTransferAsync(TransferTransaction transaction, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                return new Result<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Transfer_Author_Not_Exists));

            if (string.IsNullOrWhiteSpace(transaction.CreditedWallet.Identifier))
                return new Result<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Transfer_CreditedWallet_Not_Exists));

            if (string.IsNullOrWhiteSpace(transaction.DebitedWallet.Identifier))
                return new Result<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Transfer_DebitedWallet_Not_Exists));

            try
            {
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

                return new Result<PspPaymentResultDto>(new PspPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = result.ResultMessage,
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            }
            catch (Exception e)
            {
                return new Result<PspPaymentResultDto>(e);
            }
        }

        public async Task<Result<PspPaymentResultDto>> CreatePayoutAsync(PayoutTransaction transaction, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                return new Result<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Payout_Author_Not_Exists));

            if (string.IsNullOrWhiteSpace(transaction.DebitedWallet.Identifier))
                return new Result<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotCreate_Payout_DebitedWallet_Not_Exists));

            try
            {
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

                return new Result<PspPaymentResultDto>(new PspPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = result.ResultMessage,
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            }
            catch (Exception e)
            {
                return new Result<PspPaymentResultDto>(e);
            }
        }

        public async Task<Result<PspPaymentResultDto>> RefundPayinAsync(RefundPayinTransaction transaction, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                return new Result<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotRefund_Payin_Author_Not_Exists));

            if (string.IsNullOrWhiteSpace(transaction.TransactionToRefundIdentifier))
                return new Result<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotRefund_Payin_PayinIdentifier_Missing));

            try
            {
                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.PayIns.CreateRefundAsync(transaction.Id.ToString("N"), transaction.TransactionToRefundIdentifier,
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

                return new Result<PspPaymentResultDto>(new PspPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = result.ResultMessage,
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            }
            catch (Exception e)
            {
                return new Result<PspPaymentResultDto>(e);
            }
        }

        public async Task<Result<PspPaymentResultDto>> RefundTransferAsync(RefundTransferTransaction transaction, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(transaction.Author.Identifier))
                return new Result<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotRefund_Transfer_Author_Not_Exists));

            if (string.IsNullOrWhiteSpace(transaction.TransactionToRefundIdentifier))
                return new Result<PspPaymentResultDto>(new SheaftException(ExceptionKind.BadRequest, MessageKind.PsP_CannotRefund_Transfer_TransferIdentifier_Missing));

            try
            {
                await EnsureAccessTokenIsValidAsync(token);

                var result = await _api.Transfers.CreateRefundAsync(transaction.Id.ToString("N"), transaction.TransactionToRefundIdentifier, new RefundTransferPostDTO(transaction.Author.Identifier));
                return new Result<PspPaymentResultDto>(new PspPaymentResultDto
                {
                    Credited = result.CreditedFunds.Amount.GetAmount(),
                    ExecutedOn = result.ExecutionDate,
                    Identifier = result.Id,
                    ResultCode = result.ResultCode,
                    ResultMessage = result.ResultMessage,
                    Debited = result.DebitedFunds.Amount.GetAmount(),
                    Fees = result.Fees.Amount.GetAmount(),
                    Status = result.Status.GetTransactionStatus()
                });
            }
            catch (Exception e)
            {
                return new Result<PspPaymentResultDto>(e);
            }
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

        public static ValidationStatus GetValidationStatus(this KycStatus status)
        {
            return (ValidationStatus)status;
        }

        public static Sheaft.Interop.Enums.TransactionStatus GetTransactionStatus(this MangoPay.SDK.Core.Enumerations.TransactionStatus status)
        {
            return (Sheaft.Interop.Enums.TransactionStatus)status;
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
        public static CountryIso GetCountry(this CountryIsoCode countryCode)
        {
            return (CountryIso)countryCode;
        }
    }
}
