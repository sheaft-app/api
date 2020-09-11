using Sheaft.Core;
using Sheaft.Domain.Models;
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
        Task<Result<string>> CreatePayoutAsync(PayoutTransaction transaction, CancellationToken token);
        Task<Result<string>> CreateTransferAsync(TransferTransaction transaction, CancellationToken token);
        Task<Result<string>> CreateUserAsync(User user, CancellationToken token);
        Task<Result<string>> CreateWalletAsync(Wallet wallet,  CancellationToken token);
        Task<Result<string>> CreateWebPayinAsync(PayinTransaction transaction, CancellationToken token);
        Task<Result<string>> RefundPayinAsync(RefundPayinTransaction transaction, CancellationToken token);
        Task<Result<string>> RefundTransferAsync(RefundTransferTransaction transaction, CancellationToken token);
        Task<Result<bool>> SubmitDocumentAsync(Document document, CancellationToken token);
        Task<Result<string>> ValidateCardAsync(Card payment, string registrationId, string registrationData, CancellationToken token);
    }
}