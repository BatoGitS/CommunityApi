using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Domain
{
    public class User: IdentityUser
    {
        public String FullName { get; set; }
        public String About { get; set; }
        public String City { get; set; }
        public String BirthDay { get; set; }
        
        [NotMapped]
        public IEnumerable<Technology> technologies { get; set; }
    }
}
