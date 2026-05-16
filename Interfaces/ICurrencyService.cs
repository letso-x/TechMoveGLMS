namespace TechMoveGLMS.Interfaces
{
    public interface ICurrencyService
    {
        Task<decimal> ConvertCurrency(decimal amount);
    }
}
