namespace commentsAPI.Entities.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
