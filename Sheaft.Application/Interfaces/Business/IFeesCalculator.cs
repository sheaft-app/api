namespace Sheaft.Application.Interfaces.Business
{
    public interface IFeesCalculator
    {
        decimal GetFees(decimal amount);
    }
}