using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityAPI.Contracts.v1.Response
{
    public class UserResponse
    {
        public String Id { get; set; }
        public String Email { get; set; }
        public String FullName { get; set; }
        public String About { get; set; }
        public String City { get; set; }
        public String BirthDay { get; set; }

        IEnumerable<TechnologyResponse> technologies { get; set; }

    }
}
