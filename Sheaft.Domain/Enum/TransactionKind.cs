namespace Sheaft.Domain.Enum
{
    public enum TransactionKind
    {
        PayinWeb = 0,
        PayinCard,
        PayinMoney,
        PayinCheck,
        PayinExternal,
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