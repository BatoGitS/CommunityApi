using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }
            
            return httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
        public static bool IsAdmin(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return false;
            }
            
            return httpContext.User.HasClaim("Admin", "true");
        }
        public static string GetLocationURI(this HttpContext httpContext, String endpoint)
        {
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}";

            return baseUrl + "/" + endpoint;
        }
    }
}
