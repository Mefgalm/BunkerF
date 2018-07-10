using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Database.Entities
{
    public class TeamJoinInfo
    {
        [Key]
        public int TeamId { get; set; }
        
        [Required]
        public string Key { get; set; }
        
        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }
    }
}