using Microsoft.AspNetCore.Mvc;
using QLKS.Services;
using QLKS.Services.Strategies;
using QLKS.Utils;

namespace QLKS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricingController : ControllerBase
    {
        private readonly IPricingService _pricingService;

        public PricingController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        /// <summary>
        /// Calculate room price with additional services (Decorator Pattern)
        /// POST /api/pricing/calculate
        /// Body: { "basePrice": 1000000, "services": ["breakfast", "spa"] }
        /// </summary>
        [HttpPost("calculate")]
        public IActionResult CalculatePrice([FromBody] PriceCalculationRequest request)
        {
            try
            {
                var finalPrice = _pricingService.CalculateRoomPrice(request.BasePrice, request.Services);
                var description = _pricingService.GetServiceDescription(request.Services);

                return Ok(new
                {
                    success = true,
                    basePrice = request.BasePrice,
                    finalPrice = finalPrice,
                    services = request.Services,
                    description = description,
                    addedCost = finalPrice - request.BasePrice
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in CalculatePrice: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Apply discount strategy (Strategy Pattern)
        /// POST /api/pricing/discount
        /// Body: { "price": 1500000, "strategyType": "vip", "parameters": { "visitCount": 5 } }
        /// </summary>
        [HttpPost("discount")]
        public IActionResult ApplyDiscount([FromBody] DiscountRequest request)
        {
            try
            {
                IDiscountStrategy strategy = request.StrategyType.ToLower() switch
                {
                    "vip" => new VIPDiscountStrategy(),
                    "seasonal" => new SeasonalDiscountStrategy(),
                    "loyalty" => new LoyaltyDiscountStrategy(request.Parameters?.VisitCount ?? 0),
                    "earlybird" => new EarlyBirdDiscountStrategy(request.Parameters?.DaysInAdvance ?? 0),
                    _ => new NoDiscountStrategy()
                };

                var discountedPrice = _pricingService.ApplyDiscountStrategy(request.Price, strategy);

                return Ok(new
                {
                    success = true,
                    originalPrice = request.Price,
                    discountedPrice = discountedPrice,
                    savedAmount = request.Price - discountedPrice,
                    strategy = strategy.GetStrategyName(),
                    description = strategy.GetDiscountDescription()
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in ApplyDiscount: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get all available discount strategies
        /// GET /api/pricing/strategies
        /// </summary>
        [HttpGet("strategies")]
        public IActionResult GetStrategies()
        {
            var strategies = new[]
            {
                new { name = "none", displayName = "Không giảm giá", description = "Giá gốc" },
                new { name = "vip", displayName = "VIP", description = "Giảm 10% cho khách VIP" },
                new { name = "seasonal", displayName = "Theo mùa", description = "Giảm 15% mùa thấp điểm" },
                new { name = "loyalty", displayName = "Thân thiết", description = "Giảm 5-20% theo số lần đặt" },
                new { name = "earlybird", displayName = "Đặt sớm", description = "Giảm 10-20% khi đặt trước" }
            };

            return Ok(new { success = true, strategies });
        }

        /// <summary>
        /// Get all available additional services
        /// GET /api/pricing/services
        /// </summary>
        [HttpGet("services")]
        public IActionResult GetServices()
        {
            var services = new[]
            {
                new { name = "breakfast", displayName = "Bữa sáng", price = 100000 },
                new { name = "airport", displayName = "Đưa đón sân bay", price = 300000 },
                new { name = "spa", displayName = "Spa & Massage", price = 500000 },
                new { name = "laundry", displayName = "Giặt ủi", price = 50000 }
            };

            return Ok(new { success = true, services });
        }
    }

    // Request models
    public class PriceCalculationRequest
    {
        public decimal BasePrice { get; set; }
        public List<string> Services { get; set; } = new();
    }

    public class DiscountRequest
    {
        public decimal Price { get; set; }
        public string StrategyType { get; set; } = "none";
        public DiscountParameters? Parameters { get; set; }
    }

    public class DiscountParameters
    {
        public int VisitCount { get; set; }
        public int DaysInAdvance { get; set; }
    }
}
