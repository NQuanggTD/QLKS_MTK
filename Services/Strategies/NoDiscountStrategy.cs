namespace QLKS.Services.Strategies
{
    /// <summary>
    /// No discount strategy - regular customer
    /// </summary>
    public class NoDiscountStrategy : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal originalPrice)
        {
            return originalPrice;
        }

        public string GetDiscountDescription()
        {
            return "Không giảm giá";
        }

        public string GetStrategyName()
        {
            return "Khách hàng thường";
        }
    }
}
