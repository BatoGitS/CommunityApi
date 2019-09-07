using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Domain
{
    public class Technology
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public String Name { get; set; }
        public String Description { get; set; }

    }
}
