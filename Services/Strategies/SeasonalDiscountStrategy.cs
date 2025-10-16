namespace QLKS.Services.Strategies
{
    /// <summary>
    /// Seasonal discount strategy - discount based on season (low/peak season)
    /// Example: 15% discount in low season
    /// </summary>
    public class SeasonalDiscountStrategy : IDiscountStrategy
    {
        private readonly decimal _discountRate;
        private readonly string _seasonName;

        public SeasonalDiscountStrategy(decimal discountRate = 0.15m, string seasonName = "Mùa thấp điểm")
        {
            _discountRate = discountRate;
            _seasonName = seasonName;
        }

        public decimal ApplyDiscount(decimal originalPrice)
        {
            return originalPrice * (1 - _discountRate);
        }

        public string GetDiscountDescription()
        {
            return $"Giảm giá {_seasonName} {_discountRate * 100}%";
        }

        public string GetStrategyName()
        {
            return $"Khuyến mãi theo mùa - {_seasonName}";
        }
    }
}
