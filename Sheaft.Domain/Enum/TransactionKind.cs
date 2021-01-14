namespace Sheaft.Domain.Enums
{
    public enum TransactionKind
    {
        WebPayin = 0,
        CardPayin,
        PreAuthorizedPayin,
        Transfer = 100,
        Payout = 200,
        RefundPayin = 300,
        RefundTransfer,
        RefundPayout,
        Repudiation = 400,
        Settlement = 500
    }
}