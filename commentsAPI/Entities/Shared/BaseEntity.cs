namespace commentsAPI.Entities.Shared
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
