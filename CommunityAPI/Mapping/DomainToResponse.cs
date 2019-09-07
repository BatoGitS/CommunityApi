using AutoMapper;
using CommunityAPI.Contracts.v1.Response;
using CommunityAPI.Domain;
using Contracts.v1.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Mapping
{
    public class DomainToResponse : Profile
    {
        public DomainToResponse()
        {
            CreateMap<User, UserResponse>();
            CreateMap<UserTechnology, UserTechnologyScoreResponse>();
            CreateMap<Technology, TechnologyResponse>();
        }
    }
}
