using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface IPspService
    {
        Task<Result<bool>> AddPageToDocumentAsync(Page page, Document document, string userIdentifier, byte[] bytes, CancellationToken token);
        Task<Result<string>> CreateBankIbanAsync(BankAccount payment, CancellationToken token);
        Task<Result<bool>> UpdateBankIbanAsync(BankAccount payment, bool isActive, CancellationToken token);
        Task<Result<PspCardRegistrationResultDto>> CreateCardRegistrationAsync(User user, CancellationToken token);
        Task<Result<PspDocumentResultDto>> CreateDocumentAsync(Document document, string userIdentifier, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreatePayoutAsync(Payout transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateTransferAsync(Transfer transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateDonationAsync(Donation transaction, CancellationToken token);
        Task<Result<string>> CreateConsumerAsync(ConsumerLegal user, CancellationToken token);
        Task<Result<string>> UpdateConsumerAsync(ConsumerLegal consumerLegal, CancellationToken token);
        Task<Result<string>> CreateBusinessAsync(BusinessLegal user, CancellationToken token);
        Task<Result<string>> UpdateBusinessAsync(BusinessLegal businessLegal, CancellationToken token);
        Task<Result<string>> CreateWalletAsync(Wallet wallet,  CancellationToken token);
        Task<Result<PspWebPaymentResultDto>> CreateWebPayinAsync(WebPayin transaction, Owner owner, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateCardPayinAsync(CardPayin transaction, Owner owner, CancellationToken token);
        Task<Result<PspPaymentResultDto>> RefundPayinAsync(WebPayinRefund transaction, CancellationToken token);
        Task<Result<PspDocumentResultDto>> SubmitDocumentAsync(Document document, string userIdentifier, CancellationToken token);
        Task<Result<PspDeclarationResultDto>> CreateUboDeclarationAsync(Declaration declaration, User business, CancellationToken token);
        Task<Result<PspDeclarationResultDto>> SubmitUboDeclarationAsync(Declaration declaration, User business, CancellationToken token);
        Task<Result<string>> CreateUboAsync(Ubo ubo, Declaration declaration, User business, CancellationToken token);
        Task<Result<bool>> UpdateUboAsync(Ubo ubo, Declaration declaration, User business, CancellationToken token);
        Task<Result<PspDocumentResultDto>> GetDocumentAsync(string identifier, CancellationToken token);
        Task<Result<PspDeclarationResultDto>> GetDeclarationAsync(string identifier, CancellationToken token);
        Task<Result<PspTransactionResultDto>> GetPayinAsync(string identifier, CancellationToken token);
        Task<Result<PspTransactionResultDto>> GetTransferAsync(string identifier, CancellationToken token);
        Task<Result<PspTransactionResultDto>> GetPayoutAsync(string identifier, CancellationToken token);
        Task<Result<PspTransactionResultDto>> GetRefundAsync(string identifier, CancellationToken token);
        Task<Result<PspUserLegalDto>> GetCompanyAsync(string identifier, CancellationToken token);
        Task<Result<PspUserNormalDto>> GetConsumerAsync(string identifier, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateWithholdingAsync(Withholding transaction, CancellationToken token);
        Task<Result<PspPreAuthorizationResultDto>> CreatePreAuthorizationAsync(PreAuthorization preAuthorization, CancellationToken token);
        Task<Result<PspPreAuthorizationResultDto>> GetPreAuthorizationAsync(string identifier, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreatePreAuthorizedPayinAsync(PreAuthorizedPurchaseOrderPayin preAuthorizedPayin, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreatePreAuthorizedPayinAsync(PreAuthorizedDonationPayin preAuthorizedPayin, CancellationToken token);
    }
}