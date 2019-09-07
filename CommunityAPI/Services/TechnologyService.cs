using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityAPI.Data;
using CommunityAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace CommunityAPI.Services
{
    public class TechnologyService : ITechnologyService
    {
        private readonly ModelContext _dataContext;

        public TechnologyService(ModelContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> CreateTechnologyAsync(Technology technology)
        {
            try
            {
                await _dataContext.Technology.AddAsync(technology);
                var created = await _dataContext.SaveChangesAsync();
                return created > 0;
            }
            catch {
                return false;
            }
        }

        public async Task<bool> DeleteTechnologyAsync(Guid technologyId)
        {
            var post = await GetTechnologyByIdAsync(technologyId);

            if (post == null)
                return false;

            _dataContext.Technology.Remove(post);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<List<Technology>> GetTechnologiesAsync()
        {
            return await _dataContext.Technology.OrderBy(ord => ord.Name).ToListAsync();
        }

        public async Task<Technology> GetTechnologyByIdAsync(Guid technologyId)
        {
            return await _dataContext.Technology.SingleOrDefaultAsync(x => x.Id == technologyId.ToString());
        }

        public async Task<bool> UpdateTechnologyAsync(Technology technologyToUpdate)
        {
            _dataContext.Technology.Update(technologyToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }
    }
}
