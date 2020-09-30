using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface IPspService
    {
        Task<Result<bool>> AddPageToDocumentAsync(Page page, Document document, string userIdentifier, byte[] bytes, CancellationToken token);
        Task<Result<string>> CreateBankIbanAsync(BankAccount payment, CancellationToken token);
        Task<Result<KeyValuePair<string, string>>> CreateCardAsync(Card payment, CancellationToken token);
        Task<Result<PspDocumentResultDto>> CreateDocumentAsync(Document document, string userIdentifier, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreatePayoutAsync(Payout transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateTransferAsync(Transfer transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateDonationAsync(Donation transaction, CancellationToken token);
        Task<Result<string>> CreateConsumerAsync(ConsumerLegal user, CancellationToken token);
        Task<Result<string>> CreateBusinessAsync(BusinessLegal user, CancellationToken token);
        Task<Result<string>> CreateWalletAsync(Wallet wallet,  CancellationToken token);
        Task<Result<PspWebPaymentResultDto>> CreateWebPayinAsync(WebPayin transaction, Owner owner, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateCardPayinAsync(CardPayin transaction, Owner owner, CancellationToken token);
        Task<Result<PspPaymentResultDto>> RefundPayinAsync(PayinRefund transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> RefundTransferAsync(TransferRefund transaction, CancellationToken token);
        Task<Result<PspDocumentResultDto>> SubmitDocumentAsync(Document document, string userIdentifier, CancellationToken token);
        Task<Result<string>> ValidateCardAsync(Card payment, string registrationId, string registrationData, CancellationToken token);
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
    }
}