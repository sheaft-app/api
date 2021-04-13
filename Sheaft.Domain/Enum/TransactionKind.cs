namespace Sheaft.Domain.Enum
{
    public enum TransactionKind
    {
        WebPayin = 0,
        PreAuthorizedPayin,
        Transfer = 100,
        Payout = 200,
        RefundPayin = 300,
        RefundTransfer,
        RefundPayout,
        Repudiation = 400,
        Settlement = 500,
        Donation = 600,
        Commission = 700,
        Withholding = 800
    }
}