using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Database.Entities
{
    public class PlayerRole
    {
        public int PlayerId { get; set; }
        
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }
        
        public int RoleId { get; set; }
        
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
    }
}