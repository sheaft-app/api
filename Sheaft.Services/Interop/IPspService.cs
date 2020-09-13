﻿using Sheaft.Core;
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
        Task<Result<bool>> AddPageToDocumentAsync(Page page, Document document, Stream data, CancellationToken token);
        Task<Result<string>> CreateBankIbanAsync(BankAccount payment, CancellationToken token);
        Task<Result<KeyValuePair<string, string>>> CreateCardAsync(Card payment, CancellationToken token);
        Task<Result<PspDocumentResultDto>> CreateDocumentAsync(Document document, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreatePayoutAsync(PayoutTransaction transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateTransferAsync(TransferTransaction transaction, CancellationToken token);
        Task<Result<string>> CreateConsumerAsync(ConsumerLegal user, CancellationToken token);
        Task<Result<string>> CreateBusinessAsync(BusinessLegal user, CancellationToken token);
        Task<Result<string>> CreateWalletAsync(Wallet wallet,  CancellationToken token);
        Task<Result<PspWebPaymentResultDto>> CreateWebPayinAsync(WebPayinTransaction transaction, Owner owner, CancellationToken token);
        Task<Result<PspPaymentResultDto>> CreateCardPayinAsync(CardPayinTransaction transaction, Owner owner, CancellationToken token);
        Task<Result<PspPaymentResultDto>> RefundPayinAsync(RefundPayinTransaction transaction, CancellationToken token);
        Task<Result<PspPaymentResultDto>> RefundTransferAsync(RefundTransferTransaction transaction, CancellationToken token);
        Task<Result<PspDocumentResultDto>> SubmitDocumentAsync(Document document, CancellationToken token);
        Task<Result<string>> ValidateCardAsync(Card payment, string registrationId, string registrationData, CancellationToken token);
    }
}