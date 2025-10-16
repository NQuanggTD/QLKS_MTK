namespace QLKS.Services.Decorators
{
    /// <summary>
    /// Decorator adds laundry service
    /// Example: Add laundry service for 50,000 VND per day
    /// </summary>
    public class LaundryServiceDecorator : BaseServiceDecorator
    {
        public LaundryServiceDecorator(IServiceDecorator? wrappedService = null) 
            : base(wrappedService, "Giặt ủi miễn phí", 50000)
        {
        }

        public override decimal CalculatePrice(decimal basePrice)
        {
            // Add laundry service cost
            return base.CalculatePrice(basePrice);
        }
    }
}
