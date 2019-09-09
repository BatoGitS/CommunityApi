using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Domain
{
    public class UserTechnology
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public string TechnologyID { get; set; }

        public virtual IEnumerable<TechnologyScore> Scores { get; set; }

        [ForeignKey(nameof(TechnologyID))]
        public virtual Technology Technology { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }
    }
}
