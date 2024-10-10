namespace commentsAPI.Entities.Shared
{
    public class PaginatedResult<T>
    {
        public T? Result { get; set; }
        public int TotalCount { get; set; }
    }
}
