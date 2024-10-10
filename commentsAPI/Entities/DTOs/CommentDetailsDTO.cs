using commentsAPI.Entities.Shared;

namespace commentsAPI.Entities.DTOs
{
    public class CommentDetailsDTO
    {
        public Guid Id { get; set; }
        public string CommentText { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public UserDTO? User { get; set; }
        public PaginatedResult<IEnumerable<CommentDTO>> ChildComments { get; set; } = new PaginatedResult<IEnumerable<CommentDTO>>();
    }
}
