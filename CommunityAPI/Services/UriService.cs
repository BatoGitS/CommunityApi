using CommunityAPI.Contracts.v1;
using CommunityAPI.Contracts.v1.Request.Queries;
using System;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetUserUri(string userId)
        {
            return new Uri(_baseUri + Routes.User.Get.Replace("{userId}", userId));
        }

        public Uri GetAllUsersUri(PaginationQuery pagination = null)
        {
            var _uri = _baseUri + Routes.User.GetAll;
            var uri = new Uri(_uri);

            if (pagination == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_uri, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());

            return new Uri(modifiedUri);
        }
    }
}
