using QLKS.Models;
using QLKS.Services.Decorators;
using QLKS.Services.Strategies;
using QLKS.Utils;

namespace QLKS.Services
{
    /// <summary>
    /// Service for calculating room prices using Decorator and Strategy patterns
    /// Demonstrates how to combine multiple design patterns
    /// </summary>
    public interface IPricingService
    {
        decimal CalculateRoomPrice(decimal basePrice, List<string> additionalServices);
        decimal ApplyDiscountStrategy(decimal price, IDiscountStrategy strategy);
        string GetServiceDescription(List<string> additionalServices);
    }

    public class PricingService : IPricingService
    {
        /// <summary>
        /// Calculate room price with additional services using Decorator Pattern
        /// </summary>
        public decimal CalculateRoomPrice(decimal basePrice, List<string> additionalServices)
        {
            try
            {
                IServiceDecorator service = new BasicServiceDecorator("Phòng nghỉ");

                // Apply decorators based on selected services
                foreach (var serviceName in additionalServices)
                {
                    service = serviceName.ToLower() switch
                    {
                        "breakfast" or "bữa sáng" => new BreakfastDecorator(service),
                        "airport" or "sân bay" => new AirportTransferDecorator(service),
                        "spa" => new SpaServiceDecorator(service),
                        "laundry" or "giặt ủi" => new LaundryServiceDecorator(service),
                        _ => service
                    };
                }

                decimal finalPrice = service.CalculatePrice(basePrice);
                Logger.Info($"Calculated room price: {finalPrice} with services: {string.Join(", ", additionalServices)}");
                
                return finalPrice;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error calculating room price: {ex.Message}");
                return basePrice;
            }
        }

        /// <summary>
        /// Apply discount using Strategy Pattern
        /// </summary>
        public decimal ApplyDiscountStrategy(decimal price, IDiscountStrategy strategy)
        {
            try
            {
                decimal discountedPrice = strategy.ApplyDiscount(price);
                Logger.Info($"Applied discount strategy '{strategy.GetStrategyName()}': {price} -> {discountedPrice}");
                return discountedPrice;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error applying discount strategy: {ex.Message}");
                return price;
            }
        }

        /// <summary>
        /// Get description of all services using Decorator Pattern
        /// </summary>
        public string GetServiceDescription(List<string> additionalServices)
        {
            IServiceDecorator service = new BasicServiceDecorator("Phòng nghỉ");

            foreach (var serviceName in additionalServices)
            {
                service = serviceName.ToLower() switch
                {
                    "breakfast" or "bữa sáng" => new BreakfastDecorator(service),
                    "airport" or "sân bay" => new AirportTransferDecorator(service),
                    "spa" => new SpaServiceDecorator(service),
                    "laundry" or "giặt ủi" => new LaundryServiceDecorator(service),
                    _ => service
                };
            }

            return service.GetDescription();
        }
    }
}
