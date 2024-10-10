using System.ComponentModel.DataAnnotations;

namespace commentsAPI.Entities.Requests
{
    public class CommentRequest
    {
        public Guid? ParentCommentId { get; set; }

        [Required]
        public string CommentText { get; set; } = null!;

        [Required]
        public UserRequest User { get; set; } = null!;
    }
}
