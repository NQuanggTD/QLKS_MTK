using QLKS.Models;

namespace QLKS.Services.Decorators
{
    /// <summary>
    /// Base interface for Service Decorator Pattern
    /// Allows adding additional functionality to services without modifying their code
    /// </summary>
    public interface IServiceDecorator
    {
        decimal CalculatePrice(decimal basePrice);
        string GetDescription();
    }
}
