using commentsAPI.Entities.Requests;
namespace commentsAPI.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task CreateCommentAsync(CommentRequest request);
    }
}
