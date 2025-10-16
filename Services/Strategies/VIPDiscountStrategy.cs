namespace QLKS.Services.Strategies
{
    /// <summary>
    /// VIP discount strategy - 10% discount
    /// </summary>
    public class VIPDiscountStrategy : IDiscountStrategy
    {
        private readonly decimal _discountRate;

        public VIPDiscountStrategy(decimal discountRate = 0.10m)
        {
            _discountRate = discountRate;
        }

        public decimal ApplyDiscount(decimal originalPrice)
        {
            return originalPrice * (1 - _discountRate);
        }

        public string GetDiscountDescription()
        {
            return $"Giảm giá VIP {_discountRate * 100}%";
        }

        public string GetStrategyName()
        {
            return "Khách hàng VIP";
        }
    }
}
