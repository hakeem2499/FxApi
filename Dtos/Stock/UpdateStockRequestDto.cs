using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock
{
    /// <summary>
    /// Represents a request DTO for updating stock details.
    /// </summary>
    public class UpdateStockRequestDto
    {
        /// <summary>
        /// Gets the stock symbol. Must not exceed 10 characters.
        /// </summary>
        [StringLength(10, ErrorMessage = "Symbol must not exceed 10 characters.")]
        public string Symbol { get; init; } = string.Empty;

        /// <summary>
        /// Gets the company name. Must not exceed 100 characters.
        /// </summary>
        [StringLength(100, ErrorMessage = "Company name must not exceed 100 characters.")]
        public string CompanyName { get; init; } = string.Empty;

        /// <summary>
        /// Gets the purchase price of the stock. Must be greater than or equal to 0.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Purchase value cannot be negative.")]
        public decimal Purchase { get; init; }

        /// <summary>
        /// Gets the last dividend value of the stock. Cannot be negative.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "LastDiv cannot be negative.")]
        public decimal LastDiv { get; init; }

        /// <summary>
        /// Gets the industry name. Must not exceed 50 characters.
        /// </summary>
        [StringLength(50, ErrorMessage = "Industry name must not exceed 50 characters.")]
        public string Industry { get; init; } = string.Empty;

        /// <summary>
        /// Gets the market capitalization of the stock. Cannot be negative.
        /// </summary>
        [Range(0, long.MaxValue, ErrorMessage = "MarketCap cannot be negative.")]
        public long MarketCap { get; init; }
    }
}
