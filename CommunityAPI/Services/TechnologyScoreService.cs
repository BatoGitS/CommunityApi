using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommunityAPI.Contracts;
using CommunityAPI.Contracts.v1.Response;
using CommunityAPI.Data;
using CommunityAPI.Domain;
using CommunityAPI.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CommunityAPI.Services
{
    public class TechnologyScoreService : ITechnologyScoreService
    {
        private readonly ModelContext _dataContext;
        private readonly IMapper _mapper;

        public TechnologyScoreService(ModelContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<List<UserTechnologyScoreResponse>> GetTechnologiesAsync(Guid userId)
        {
            return await _dataContext.UserTechnology.Where(x => x.UserID == userId.ToString())
                .Include(x => x.Technology)
                .GroupJoin(_dataContext.TechnologyScore,
                    p => new { UID = p.UserID, TID = p.TechnologyID },
                    c => new { UID = c.TargetUserID, TID = c.TechnologyID },
                    (p, c) => new UserTechnologyScoreResponse
                    {
                        Technology = _mapper.Map<TechnologyResponse>(p.Technology),
                        AvgScore = c.DefaultIfEmpty().Average(x => x.Score)
                    }
                ).OrderByDescending(o => o.AvgScore).ThenBy(o => o.Technology.Name)
                .ToListAsync();
        }
        public async Task<bool> AddTechnologyScoreAsync(TechnologyScore technology)
        {
            var exist = await _dataContext.TechnologyScore
                .AsNoTracking()
                .SingleOrDefaultAsync(c =>
                    c.SourceUserID == technology.SourceUserID &&
                    c.TargetUserID == technology.TargetUserID &&
                    c.TechnologyID == technology.TechnologyID);
            if (exist != null)
            {
                technology.Id = exist.Id;
                _dataContext.TechnologyScore.Update(technology);
            } else
            {
                _dataContext.TechnologyScore.Add(technology);
            }
            var added = await _dataContext.SaveChangesAsync();
            return added > 0;
        }

        public async Task<bool> AddTechnologyAsync(UserTechnology technology)
        {
            try
            {
                _dataContext.UserTechnology.Add(technology);

                var added = await _dataContext.SaveChangesAsync();
                return added > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<UserTechnologyScoreResponse> GetTechnologyByIdAsync(Guid userId, Guid technologyId)
        {
            return await _dataContext.UserTechnology.Where(x => x.UserID == userId.ToString() && x.TechnologyID == technologyId.ToString())
                .Include(x => x.Technology)
                .GroupJoin(_dataContext.TechnologyScore,
                    p => new { UID = p.UserID, TID = p.TechnologyID },
                    c => new { UID = c.TargetUserID, TID = c.TechnologyID },
                    (p, c) => new UserTechnologyScoreResponse
                    {
                        Technology = _mapper.Map<TechnologyResponse>(p.Technology),
                        AvgScore = c.DefaultIfEmpty().Average(x => x.Score)
                    }
                )
                .SingleOrDefaultAsync();
        }

        public async Task<bool> DeleteTechnologyAsync(Guid userId, Guid technologyId)
        {
            var technology = await _dataContext.UserTechnology
                .Where(x => x.UserID == userId.ToString() && x.TechnologyID == technologyId.ToString()).SingleOrDefaultAsync();

            if (technology == null)
                return false;

            _dataContext.UserTechnology.Remove(technology);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }
    }
}
