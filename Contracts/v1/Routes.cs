using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Contracts.v1
{
    public class Routes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;
        public static class Identity
        {
            public const string Login = Base + "/auth/login";
            public const string Register = Base + "/auth/register";
            public const string Refresh = Base + "/auth/refresh";
            public const string Logout = Base + "/auth/logout";
            public const string test = Base + "/auth";
        }
        public static class Technology
        {
            public const string GetAll = Base + "/technology";
            public const string Create = Base + "/technology";
            public const string Get = Base + "/technology/{technologyId}";
            public const string Delete = Base + "/technology/{technologyId}";
            public const string Update = Base + "/technology/{technologyId}";
        }
        public static class User
        {
            public const string GetAll = Base + "/user";
            public const string Create = Base + "/user";
            public const string Get = Base + "/user/{userId}";
            public const string Delete = Base + "/user/{userId}";
            public const string Update = Base + "/user/{userId}";

            public static class Technology
            {
                public const string GetAll = Base + "/user/{userId}/technology";
                public const string Add = Base + "/user/{userId}/technology";
                public const string Get = Base + "/user/{userId}/technology/{technologyId}";
                public const string Delete = Base + "/user/{userId}/technology/{technologyId}";
                public const string Set = Base + "/user/{userId}/technology/{technologyId}/{score}";
            }
        }

    }
}
