using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Infrastructure
{
    public interface IPspService
    {
        Task<Result<bool>> AddPageToDocumentAsync(Domain.Page page, Domain.Document document, string userIdentifier, byte[] bytes, CancellationToken token);
        Task<Result<string>> CreateBankIbanAsync(BankAccount payment, CancellationToken token);
        Task<Result<bool>> UpdateBankIbanAsync(BankAccount payment, bool isActive, CancellationToken token);
        Task<Result<PspCardRegistrationResultDto>> CreateCardRegistrationAsync(User user, CancellationToken token);
        Task<Result<PspDocumentResultDto>> CreateDocumentAsync(Domain.Document document, string userIdentifier, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreatePayoutAsync(Domain.Payout transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateTransferAsync(Domain.Transfer transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateDonationAsync(Domain.Donation transaction, CancellationToken token);
        Task<Result<string>> CreateConsumerAsync(ConsumerLegal user, CancellationToken token);
        Task<Result<string>> UpdateConsumerAsync(ConsumerLegal consumerLegal, CancellationToken token);
        Task<Result<string>> CreateBusinessAsync(BusinessLegal user, CancellationToken token);
        Task<Result<string>> UpdateBusinessAsync(BusinessLegal businessLegal, CancellationToken token);
        Task<Result<string>> CreateWalletAsync(Domain.Wallet wallet,  CancellationToken token);
        Task<Result<PspWebPaymentResultDto>> CreateWebPayinAsync(WebPayin transaction, Owner owner, CancellationToken token);
        Task<Result<PspPaymentResultDto>> RefundPayinAsync(Domain.PayinRefund transaction, CancellationToken token);
        Task<Result<PspDocumentResultDto>> SubmitDocumentAsync(Domain.Document document, string userIdentifier, CancellationToken token);
        Task<Result<PspDeclarationResultDto>> CreateUboDeclarationAsync(Domain.Declaration declaration, Domain.User business, CancellationToken token);
        Task<Result<PspDeclarationResultDto>> SubmitUboDeclarationAsync(Domain.Declaration declaration, Domain.User business, CancellationToken token);
        Task<Result<string>> CreateUboAsync(Domain.Ubo ubo, Domain.Declaration declaration, Domain.User business, CancellationToken token);
        Task<Result<bool>> UpdateUboAsync(Domain.Ubo ubo, Domain.Declaration declaration, Domain.User business, CancellationToken token);
        Task<Result<PspDocumentResultDto>> GetDocumentAsync(string identifier, CancellationToken token);
        Task<Result<PspDeclarationResultDto>> GetDeclarationAsync(string identifier, CancellationToken token);
        Task<Result<PspTransactionResultDto>> GetPayinAsync(string identifier, CancellationToken token);
        Task<Result<PspTransactionResultDto>> GetTransferAsync(string identifier, CancellationToken token);
        Task<Result<PspTransactionResultDto>> GetPayoutAsync(string identifier, CancellationToken token);
        Task<Result<PspTransactionResultDto>> GetRefundAsync(string identifier, CancellationToken token);
        Task<Result<PspUserLegalDto>> GetCompanyAsync(string identifier, CancellationToken token);
        Task<Result<PspUserNormalDto>> GetConsumerAsync(string identifier, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateWithholdingAsync(Domain.Withholding transaction, CancellationToken token);
        Task<Result<PspPreAuthorizationResultDto>> CreatePreAuthorizationAsync(PreAuthorization preAuthorization, CancellationToken token);
        Task<Result<PspPreAuthorizationResultDto>> GetPreAuthorizationAsync(string identifier, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreatePreAuthorizedPayinAsync(PreAuthorization preAuthorization, CancellationToken token);
    }
}