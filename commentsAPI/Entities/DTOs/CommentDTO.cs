namespace commentsAPI.Entities.DTOs
{
    public class CommentDTO
    {
        public Guid PublicId { get; set; }
        public string CommentText { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public UserDTO User { get; set; } = null!;
    }
}
