namespace QLKS.Services.Decorators
{
    /// <summary>
    /// Base Decorator for Service pricing
    /// Implements Decorator Pattern to add additional services/features dynamically
    /// </summary>
    public abstract class BaseServiceDecorator : IServiceDecorator
    {
        protected readonly IServiceDecorator? _wrappedService;
        protected readonly string _description;
        protected readonly decimal _additionalPrice;

        protected BaseServiceDecorator(IServiceDecorator? wrappedService, string description, decimal additionalPrice)
        {
            _wrappedService = wrappedService;
            _description = description;
            _additionalPrice = additionalPrice;
        }

        public virtual decimal CalculatePrice(decimal basePrice)
        {
            decimal price = basePrice + _additionalPrice;
            if (_wrappedService != null)
            {
                price = _wrappedService.CalculatePrice(price);
            }
            return price;
        }

        public virtual string GetDescription()
        {
            string description = _description;
            if (_wrappedService != null)
            {
                description = $"{_wrappedService.GetDescription()}, {description}";
            }
            return description;
        }
    }
}
