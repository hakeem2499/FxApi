namespace api.Helpers
{
    /// <summary>
    /// Represents a query object for filtering and paginating data requests.
    /// </summary>
    public class QueryObject
    {
        /// <summary>
        /// Gets or sets the stock symbol for filtering results.
        /// </summary>
        public string? Symbol { get; init; }

        /// <summary>
        /// Gets or sets the company name for filtering results.
        /// </summary>
        public string? CompanyName { get; init; }

        /// <summary>
        /// Gets or sets the field to sort the results by.
        /// </summary>
        public string? SortBy { get; init; }

        /// <summary>
        /// Gets or sets whether the results should be sorted in descending order.
        /// </summary>
        public bool IsDescending { get; init; } = false;

        /// <summary>
        /// Gets or sets the page number for pagination.
        /// Defaults to 1.
        /// </summary>
        public int PageNumber { get; init; } = 1;

        /// <summary>
        /// Gets or sets the page size for pagination.
        /// Defaults to 20.
        /// </summary>
        public int PageSize { get; init; } = 20;
    }
}
