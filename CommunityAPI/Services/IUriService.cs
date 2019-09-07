using CommunityAPI.Contracts.v1.Request.Queries;
using System;

namespace CommunityAPI.Services
{
    public interface IUriService
    {
        Uri GetUserUri(string userId);

        Uri GetAllUsersUri(PaginationQuery pagination = null);
    }
}
