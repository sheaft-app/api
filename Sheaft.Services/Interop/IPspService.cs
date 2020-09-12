using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Services.Interop
{
    public interface IPspService
    {
        Task<Result<bool>> AddPageToDocumentAsync(Document document, int pageNumber, Stream page, CancellationToken token);
        Task<Result<string>> CreateBankIbanAsync(BankAccount payment, CancellationToken token);
        Task<Result<KeyValuePair<string, string>>> CreateCardAsync(Card payment, CancellationToken token);
        Task<Result<string>> CreateDocumentAsync(Document document, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreatePayoutAsync(PayoutTransaction transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateTransferAsync(TransferTransaction transaction, CancellationToken token);
        Task<Result<string>> CreateUserAsync(User user, CancellationToken token);
        Task<Result<string>> CreateWalletAsync(Wallet wallet,  CancellationToken token);
        Task<Result<PspWebPaymentResultDto>> CreateWebPayinAsync(WebPayinTransaction transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateCardPayinAsync(CardPayinTransaction transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> RefundPayinAsync(RefundPayinTransaction transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> RefundTransferAsync(RefundTransferTransaction transaction, CancellationToken token);
        Task<Result<bool>> SubmitDocumentAsync(Document document, CancellationToken token);
        Task<Result<string>> ValidateCardAsync(Card payment, string registrationId, string registrationData, CancellationToken token);
    }
}