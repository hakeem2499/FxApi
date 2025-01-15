using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for creating a comment.
    /// </summary>
    public class CreateCommentDto
    {
        /// <summary>
        /// Gets or sets the title of the comment. Required and must not exceed 100 characters.
        /// </summary>
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title must not exceed 100 characters.")]
        public string Title { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the content of the comment. Required and must not exceed 1000 characters.
        /// </summary>
        [Required(ErrorMessage = "Content is required.")]
        [StringLength(1000, ErrorMessage = "Content must not exceed 1000 characters.")]
        public string Content { get; init; } = string.Empty;
    }
}
