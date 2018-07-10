using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Database.Entities
{
    public class Challange
    {
        public Challange()
        {
            Tasks = new List<Task>();
            Teams = new List<ChallangeTeam>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(10000)]
        public string Desciprion { get; set; }

        public ICollection<Task> Tasks { get; set; }

        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        
        public int PlayerOwnerId { get; set; }
        
        [ForeignKey(nameof(PlayerOwnerId))]
        public Player PlayerOwner { get; set; }
        
        public ICollection<ChallangeTeam> Teams { get; set; }
    }
}