using CommunityAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Services
{
    public interface ITechnologyService
    {
        Task<List<Technology>> GetTechnologiesAsync();
        Task<bool> CreateTechnologyAsync(Technology technology);
        Task<Technology> GetTechnologyByIdAsync(Guid technologyId);
        Task<bool> UpdateTechnologyAsync(Technology technologyToUpdate);
        Task<bool> DeleteTechnologyAsync(Guid technologyId);
    }
}
