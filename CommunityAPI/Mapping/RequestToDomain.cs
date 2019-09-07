using AutoMapper;
using CommunityAPI.Contracts.v1.Request.Queries;
using CommunityAPI.Contracts.v1.Response;
using CommunityAPI.Domain;

namespace CommunityAPI.Mapping
{
    public class RequestToDomain : Profile
    {
        public RequestToDomain()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<TechnologyResponse, Technology>();
        }
    }
}
