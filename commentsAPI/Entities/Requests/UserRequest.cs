using System.ComponentModel.DataAnnotations;

namespace commentsAPI.Entities.Requests
{
    public class UserRequest
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Username must contain only alphanumeric characters (latin letters and numbers).")]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string? HomePage { get; set; }
    }
}
