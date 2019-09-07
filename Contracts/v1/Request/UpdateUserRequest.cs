using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityAPI.Contracts.v1.Request
{
    public class UpdateUserRequest
    {
        public String FullName { get; set; }
        public String About { get; set; }
        public String City { get; set; }
        public String BirthDay { get; set; }
    }
}
