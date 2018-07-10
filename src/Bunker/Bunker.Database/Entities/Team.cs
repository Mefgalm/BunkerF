using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Database.Entities
{
    public class Team
    {
        public Team()
        {
            Players = new List<PlayerTeam>();
            Challanges = new List<ChallangeTeam>();
        }
        
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        public int CompanyId { get; set; }
        
        public TeamJoinInfo TeamJoinInfo { get; set; }
        
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        
        public ICollection<PlayerTeam> Players { get; set; }
        
        public ICollection<ChallangeTeam> Challanges { get; set; }
    }
}