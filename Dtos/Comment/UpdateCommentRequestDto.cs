using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for updating a comment.
    /// </summary>
    public class UpdateCommentRequestDto
    {
        /// <summary>
        /// Gets or sets the title of the comment. Optional but must not exceed 100 characters if provided.
        /// </summary>
        [StringLength(100, ErrorMessage = "Title must not exceed 100 characters.")]
        public string Title { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the content of the comment. Optional but must not exceed 1000 characters if provided.
        /// </summary>
        [StringLength(1000, ErrorMessage = "Content must not exceed 1000 characters.")]
        public string Content { get; init; } = string.Empty;
    }
}
