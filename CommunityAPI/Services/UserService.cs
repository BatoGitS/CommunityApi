using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityAPI.Data;
using CommunityAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace CommunityAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ModelContext _dataContext;

        public UserService(ModelContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);

            if (user == null)
                return false;

            _dataContext.Users.Remove(user);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<List<User>> GetAllUsersAsync(PaginationFilter paginationFilter = null, string search = null)
        {
            if (paginationFilter == null && search == null)
            {
                return await _dataContext.Users.OrderBy(ord => ord.FullName).ToListAsync();
            }

            var searchArr = search?.Split(';');
            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            if (search != null && paginationFilter == null)
            {
                return await _dataContext.Users.GroupJoin(_dataContext.UserTechnology, users => users.Id, technologies => technologies.UserID,
                    (user, technology) => new
                    {
                        User = user,
                        Technology = technology.Join(_dataContext.Technology, i => i.TechnologyID, t => t.Id,
                            (i, t) => t.Name
                        )
                    }).Where(s => searchArr.Any(w =>
                            s.User.FullName.Contains(w, StringComparison.InvariantCultureIgnoreCase)
                            || s.Technology.Any(name => name.Contains(w, StringComparison.InvariantCultureIgnoreCase))
                            )
                    ).Select(user => user.User).Distinct().OrderBy(ord => ord.FullName).ToListAsync();
            }
            if (search != null && paginationFilter != null)
            {
                return await _dataContext.Users.GroupJoin(_dataContext.UserTechnology, users => users.Id, technologies => technologies.UserID,
                    (user, technology) => new
                    {
                        User = user,
                        Technology = technology.Join(_dataContext.Technology, i => i.TechnologyID, t => t.Id,
                            (i, t) => t.Name
                        )
                    }).Where(s => searchArr.Any(w =>
                            s.User.FullName.Contains(w, StringComparison.InvariantCultureIgnoreCase)
                            || s.Technology.Any(name => name.Contains(w, StringComparison.InvariantCultureIgnoreCase))
                            )
                    ).Select(user => user.User).Distinct().OrderBy(ord => ord.FullName).Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
            }
            
            return await _dataContext.Users.OrderBy(ord => ord.FullName)
                    .Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _dataContext.Users.Where(u=>u.Id == userId).SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateUserAsync(User userToUpdate)
        {
            _dataContext.Users.Update(userToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }
    }
}
