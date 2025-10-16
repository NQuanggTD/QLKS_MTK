namespace QLKS.Services.Decorators
{
    /// <summary>
    /// Basic service without any decorations
    /// Starting point for decorator chain
    /// </summary>
    public class BasicServiceDecorator : IServiceDecorator
    {
        private readonly string _baseDescription;

        public BasicServiceDecorator(string baseDescription = "Dịch vụ cơ bản")
        {
            _baseDescription = baseDescription;
        }

        public decimal CalculatePrice(decimal basePrice)
        {
            return basePrice;
        }

        public string GetDescription()
        {
            return _baseDescription;
        }
    }
}
