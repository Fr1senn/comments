using commentsAPI.Entities.Models;
using commentsAPI.Entities.Requests;
using commentsAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace commentsAPI.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommentsContext _context;
        private readonly IUserRepository _userRepository;

        public CommentRepository(CommentsContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task CreateCommentAsync(CommentRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                User dbUser = await _userRepository.GetOrCreateUserAsync(request.User);

                if (request.ParentCommentId != null)
                {
                    var dbComment = await _context.Comments.SingleOrDefaultAsync(c => c.PublicId == request.ParentCommentId);

                    if (dbComment is null)
                    {
                        throw new ApplicationException("Comment does not exist");
                    }

                    _context.Comments.Add(new Comment
                    {
                        ParentCommentId = dbComment.Id,
                        CommentText = request.CommentText,
                        UserId = dbUser.Id,
                        CreatedAt = DateTime.UtcNow,
                    });
                }
                else
                {
                    _context.Comments.Add(new Comment
                    {
                        CommentText = request.CommentText,
                        UserId = dbUser.Id,
                        CreatedAt = DateTime.UtcNow,
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (ApplicationException)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
