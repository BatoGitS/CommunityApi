using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Domain
{
    public class TechnologyScore
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string SourceUserID { get; set; }
        [Required]
        public string TargetUserID { get; set; }
        [Required]
        public string TechnologyID { get; set; }
        [Required]
        public int Score { get; set; }

        
        [ForeignKey(nameof(SourceUserID))]
        public virtual User SourceUser { get; set; }

        [ForeignKey(nameof(TargetUserID))]
        public virtual User TargetUser { get; set; }

    }
}
