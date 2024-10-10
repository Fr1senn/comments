using commentsAPI.Entities.Models;
using commentsAPI.Entities.Requests;

namespace commentsAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetOrCreateUserAsync(UserRequest request);
    }
}
