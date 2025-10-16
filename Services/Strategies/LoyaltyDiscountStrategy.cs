namespace QLKS.Services.Strategies
{
    /// <summary>
    /// Loyalty discount strategy - discount based on customer loyalty points or visits
    /// Example: 5% for 3-5 visits, 10% for 6-10 visits, 20% for 10+ visits
    /// </summary>
    public class LoyaltyDiscountStrategy : IDiscountStrategy
    {
        private readonly int _visitCount;

        public LoyaltyDiscountStrategy(int visitCount)
        {
            _visitCount = visitCount;
        }

        public decimal ApplyDiscount(decimal originalPrice)
        {
            decimal discountRate = CalculateDiscountRate();
            return originalPrice * (1 - discountRate);
        }

        public string GetDiscountDescription()
        {
            decimal discountRate = CalculateDiscountRate();
            return $"Giảm giá khách hàng thân thiết {discountRate * 100}% ({_visitCount} lần đặt phòng)";
        }

        public string GetStrategyName()
        {
            return "Khách hàng thân thiết";
        }

        private decimal CalculateDiscountRate()
        {
            if (_visitCount >= 10)
                return 0.20m; // 20% discount for 10+ visits
            else if (_visitCount >= 6)
                return 0.10m; // 10% discount for 6-9 visits
            else if (_visitCount >= 3)
                return 0.05m; // 5% discount for 3-5 visits
            else
                return 0m; // No discount for less than 3 visits
        }
    }
}
