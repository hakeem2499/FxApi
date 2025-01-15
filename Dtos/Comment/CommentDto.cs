using System;

namespace api.Dtos.Comment
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for comments.
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the comment.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Gets or sets the title of the comment.
        /// </summary>
        public string Title { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        public string Content { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the comment was created. Defaults to the current time.
        /// </summary>
        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the username or identifier of the user who created the comment.
        /// </summary>
        public string CreatedBy { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the associated stock identifier, if applicable.
        /// </summary>
        public int? StockId { get; init; }
    }
}
