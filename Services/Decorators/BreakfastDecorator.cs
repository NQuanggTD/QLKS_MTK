namespace QLKS.Services.Decorators
{
    /// <summary>
    /// Decorator adds breakfast service
    /// Example: Add breakfast for 100,000 VND per day
    /// </summary>
    public class BreakfastDecorator : BaseServiceDecorator
    {
        public BreakfastDecorator(IServiceDecorator? wrappedService = null) 
            : base(wrappedService, "Bao gồm bữa sáng", 100000)
        {
        }

        public override decimal CalculatePrice(decimal basePrice)
        {
            // Add breakfast cost
            return base.CalculatePrice(basePrice);
        }
    }
}
