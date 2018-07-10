using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Database.Entities
{
    public class PlayerTeam
    {
        public int PlayerId { get; set; }
        
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }
        
        public int TeamId { get; set; }
        
        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }
        
        public bool IsOwner { get; set; }
    }
}