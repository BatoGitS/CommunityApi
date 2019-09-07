using CommunityAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync(PaginationFilter paginationFilter = null, string search = null);
        Task<User> GetUserByIdAsync(String userId);
        Task<bool> UpdateUserAsync(User userToUpdate);
        Task<bool> DeleteUserAsync(String userId);
    }
}
