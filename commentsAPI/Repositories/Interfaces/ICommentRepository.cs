using commentsAPI.Entities.DTOs;
using commentsAPI.Entities.Requests;
using commentsAPI.Entities.Shared;
using commentsAPI.Entities.Shared.Filters;

namespace commentsAPI.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<PaginatedResult<IEnumerable<CommentDTO>>> GetCommentsAsync(PaginationFilter filter);
        Task CreateCommentAsync(CommentRequest request);
    }
}
