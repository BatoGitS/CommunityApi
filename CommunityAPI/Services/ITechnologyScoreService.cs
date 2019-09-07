using CommunityAPI.Contracts;
using CommunityAPI.Contracts.v1.Response;
using CommunityAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Services
{
    public interface ITechnologyScoreService
    {
        Task<List<UserTechnologyScoreResponse>> GetTechnologiesAsync(Guid userId);
        Task<bool> AddTechnologyAsync(UserTechnology technology);
        Task<bool> AddTechnologyScoreAsync(TechnologyScore technology);
        Task<UserTechnologyScoreResponse> GetTechnologyByIdAsync(Guid userId, Guid technologyId);
        Task<bool> DeleteTechnologyAsync(Guid userId, Guid technologyId);
    }
}
