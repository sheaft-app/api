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
        Task<Result<string>> CreateBankIbanForUserAsync(Transfer payment, User user, CancellationToken token);
        Task<Result<KeyValuePair<string, string>>> CreateCardForUserAsync(Card payment, User user, CancellationToken token);
        Task<Result<string>> CreateDocumentForUserAsync(Document document, User user, CancellationToken token);
        Task<Result<string>> CreatePayout(Transaction transaction, Transfer bankAccount, CancellationToken token);
        Task<Result<string>> CreateTransferForUser(Transaction transaction, User user, CancellationToken token);
        Task<Result<string>> CreateUserAsync(User user, CancellationToken token);
        Task<Result<string>> CreateWalletForUserAsync(Wallet wallet, User user, CancellationToken token);
        Task<Result<string>> CreateWebPayin(Transaction transaction, CancellationToken token);
        Task<Result<string>> RefundPayin(string payinIdentifier, Transaction transaction, CancellationToken token);
        Task<Result<string>> RefundTransfer(string transferIdentifier, Transaction transaction, CancellationToken token);
        Task<Result<bool>> SubmitUserDocumentAsync(User user, Document document, CancellationToken token);
        Task<Result<string>> ValidateCardAsync(Card payment, string registrationId, string registrationData, CancellationToken token);
    }
}