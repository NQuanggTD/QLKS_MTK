namespace QLKS.Services.Decorators
{
    /// <summary>
    /// Decorator adds spa/massage service
    /// Example: Add spa package for 500,000 VND
    /// </summary>
    public class SpaServiceDecorator : BaseServiceDecorator
    {
        public SpaServiceDecorator(IServiceDecorator? wrappedService = null) 
            : base(wrappedService, "Gói spa & massage", 500000)
        {
        }

        public override decimal CalculatePrice(decimal basePrice)
        {
            // Add spa service cost
            return base.CalculatePrice(basePrice);
        }
    }
}
