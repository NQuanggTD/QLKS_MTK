namespace QLKS.Services.Strategies
{
    /// <summary>
    /// Strategy Pattern interface for different discount calculation strategies
    /// Allows switching between different discount algorithms at runtime
    /// </summary>
    public interface IDiscountStrategy
    {
        decimal ApplyDiscount(decimal originalPrice);
        string GetDiscountDescription();
        string GetStrategyName();
    }
}
