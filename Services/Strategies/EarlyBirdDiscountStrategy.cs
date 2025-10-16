namespace QLKS.Services.Strategies
{
    /// <summary>
    /// Early bird discount strategy - discount for booking in advance
    /// Example: 20% discount for booking 30+ days in advance
    /// </summary>
    public class EarlyBirdDiscountStrategy : IDiscountStrategy
    {
        private readonly int _daysInAdvance;

        public EarlyBirdDiscountStrategy(int daysInAdvance)
        {
            _daysInAdvance = daysInAdvance;
        }

        public decimal ApplyDiscount(decimal originalPrice)
        {
            decimal discountRate = CalculateDiscountRate();
            return originalPrice * (1 - discountRate);
        }

        public string GetDiscountDescription()
        {
            decimal discountRate = CalculateDiscountRate();
            return $"Giảm giá đặt trước {discountRate * 100}% (Đặt trước {_daysInAdvance} ngày)";
        }

        public string GetStrategyName()
        {
            return "Early Bird - Đặt phòng sớm";
        }

        private decimal CalculateDiscountRate()
        {
            if (_daysInAdvance >= 30)
                return 0.20m; // 20% discount for 30+ days in advance
            else if (_daysInAdvance >= 14)
                return 0.15m; // 15% discount for 14-29 days in advance
            else if (_daysInAdvance >= 7)
                return 0.10m; // 10% discount for 7-13 days in advance
            else
                return 0m; // No discount for less than 7 days
        }
    }
}
