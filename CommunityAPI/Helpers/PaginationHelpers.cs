using CommunityAPI.Contracts.v1.Request.Queries;
using CommunityAPI.Domain;
using CommunityAPI.Services;
using Contracts.v1.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Helpers
{
    public class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter pagination, List<T> response)
        {
            var nextPage = pagination.PageNumber >= 1
               ? uriService.GetAllUsersUri(new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize)).ToString()
               : null;

            var previousPage = pagination.PageNumber - 1 >= 1
                ? uriService.GetAllUsersUri(new PaginationQuery(pagination.PageNumber - 1, pagination.PageSize)).ToString()
                : null;

            return new PagedResponse<T>
            {
                Data = response,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                NextPage = response.Count == pagination.PageSize ? nextPage : null,
                PreviousPage = previousPage
            };
        }
    }
}
