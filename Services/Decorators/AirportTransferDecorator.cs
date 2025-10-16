namespace QLKS.Services.Decorators
{
    /// <summary>
    /// Decorator adds airport transfer service
    /// Example: Add airport pickup/dropoff for 300,000 VND
    /// </summary>
    public class AirportTransferDecorator : BaseServiceDecorator
    {
        public AirportTransferDecorator(IServiceDecorator? wrappedService = null) 
            : base(wrappedService, "Đưa đón sân bay", 300000)
        {
        }

        public override decimal CalculatePrice(decimal basePrice)
        {
            // Add airport transfer cost
            return base.CalculatePrice(basePrice);
        }
    }
}
