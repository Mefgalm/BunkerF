using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Database.Entities
{
    public class CompanyPlayer
    {
        public int CompanyId { get; set; }
        
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        
        public int PlayerId { get; set; }
        
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }
        
        public bool IsOwner { get; set; }
    }
}