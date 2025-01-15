using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock
{
    /// <summary>
    /// Represents a request DTO for creating a stock record.
    /// </summary>
    public class CreateStockRequestDto
    {
        /// <summary>
        /// Gets the stock symbol. Must be non-empty and not exceed 10 characters.
        /// </summary>
        [Required(ErrorMessage = "Symbol is required.")]
        [StringLength(10, ErrorMessage = "Symbol must not exceed 10 characters.")]
        public required string Symbol { get; init; }

        /// <summary>
        /// Gets the company name. Must be non-empty and not exceed 100 characters.
        /// </summary>
        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100, ErrorMessage = "Company name must not exceed 100 characters.")]
        public required string CompanyName { get; init; }

        /// <summary>
        /// Gets the purchase price of the stock. Must be greater than 0.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase value must be greater than 0.")]
        public required decimal Purchase { get; init; }

        /// <summary>
        /// Gets the last dividend value. Cannot be negative.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "LastDiv cannot be negative.")]
        public required decimal LastDiv { get; init; }

        /// <summary>
        /// Gets the industry name. Must be non-empty and not exceed 50 characters.
        /// </summary>
        [Required(ErrorMessage = "Industry is required.")]
        [StringLength(50, ErrorMessage = "Industry name must not exceed 50 characters.")]
        public required string Industry { get; init; }

        /// <summary>
        /// Gets the market capitalization of the stock. Cannot be negative.
        /// </summary>
        [Range(0, long.MaxValue, ErrorMessage = "MarketCap cannot be negative.")]
        public required long MarketCap { get; init; }
    }
}
