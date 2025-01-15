using System.Collections.Generic;
using api.Dtos.Comment;

namespace api.Dtos.Stock
{
    /// <summary>
    /// Represents the data transfer object for stock details.
    /// </summary>
    public class StockDto
    {
        /// <summary>
        /// Gets the unique identifier of the stock.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Gets the stock symbol.
        /// </summary>
        public string Symbol { get; init; } = string.Empty;

        /// <summary>
        /// Gets the company name associated with the stock.
        /// </summary>
        public string CompanyName { get; init; } = string.Empty;

        /// <summary>
        /// Gets the purchase price of the stock.
        /// </summary>
        public decimal Purchase { get; init; }

        /// <summary>
        /// Gets the last dividend value of the stock.
        /// </summary>
        public decimal LastDiv { get; init; }

        /// <summary>
        /// Gets the industry to which the stock belongs.
        /// </summary>
        public string Industry { get; init; } = string.Empty;

        /// <summary>
        /// Gets the market capitalization of the stock.
        /// </summary>
        public long MarketCap { get; init; }

        /// <summary>
        /// Gets the list of comments associated with the stock.
        /// </summary>
        public List<CommentDto> Comments { get; init; } = new();
    }
}
