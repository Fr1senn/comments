using commentsAPI.Entities.Models;
using commentsAPI.Entities.Requests;
using commentsAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace commentsAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CommentsContext _context;

        public UserRepository(CommentsContext context)
        {
            _context = context;
        }

        public async Task<User> GetOrCreateUserAsync(UserRequest request)
        {
            var dbUser = await _context.Users.SingleOrDefaultAsync(u => u.UserName == request.UserName && u.Email == request.Email);

            if (dbUser != null)
            {
                return dbUser;
            }

            var newUser = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                HomePage = request.HomePage,
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }
    }
}
