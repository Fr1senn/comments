using commentsAPI.Entities.DTOs;
using commentsAPI.Entities.Models;
using commentsAPI.Entities.Requests;
using commentsAPI.Entities.Shared;
using commentsAPI.Entities.Shared.Filters;
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

        public async Task<PaginatedResult<IEnumerable<CommentDTO>>> GetCommentsAsync(PaginationFilter filter)
        {
            var totalCount = await _context.Comments.CountAsync();
            var query = _context.Comments
                .AsNoTracking()
                .Include(c => c.User)
                .Select(c => new CommentDTO
                {
                    PublicId = c.PublicId,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt,
                    User = new UserDTO
                    {
                        Id = c.User.Id,
                        UserName = c.User.UserName,
                        Email = c.User.Email
                    }
                })
                .OrderByDescending(c => c.CreatedAt)
                .Skip(filter.Skip * filter.Take)
                .Take(filter.Take);

            var comments = await query.ToListAsync();

            return new PaginatedResult<IEnumerable<CommentDTO>>
            {
                Result = comments,
                TotalCount = totalCount
            };
        }

        public async Task<CommentDetailsDTO> GetCommentById(Guid id, PaginationFilter filter)
        {
            var comment = await _context.Comments
                .AsNoTracking()
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.PublicId == id);

            if (comment == null)
            {
                throw new ApplicationException("Comment does not exist");
            }

            var childCommentsQuery = _context.Comments
                .AsNoTracking()
                .Include(c => c.User)
                .Where(c => c.ParentCommentId == comment.Id);

            var totalCount = await childCommentsQuery.CountAsync();

            var childComments = await childCommentsQuery
                .Select(c => new CommentDTO
                {
                    PublicId = c.PublicId,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt,
                    User = new UserDTO
                    {
                        Id = c.User.Id,
                        UserName = c.User.UserName,
                        Email = c.User.Email
                    }
                })
                .OrderByDescending(c => c.CreatedAt)
                .Skip(filter.Skip * filter.Take)
                .Take(filter.Take)
                .ToListAsync();

            var commentDetailsDTO = new CommentDetailsDTO
            {
                Id = comment.PublicId,
                CommentText = comment.CommentText,
                CreatedAt = comment.CreatedAt,
                User = new UserDTO
                {
                    Id = comment.User.Id,
                    UserName = comment.User.UserName,
                    Email = comment.User.Email
                },
                ChildComments = new PaginatedResult<IEnumerable<CommentDTO>>
                {
                    Result = childComments,
                    TotalCount = totalCount
                }
            };

            return commentDetailsDTO;
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
