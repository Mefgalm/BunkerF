using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Database.Entities
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(2000)]
        public string Description { get; set; }
        
        public int Score { get; set; }
        
        [StringLength(200)]
        public string Answer { get; set; }
        
        public int ChallangeId { get; set; }
        
        [ForeignKey(nameof(ChallangeId))]
        public Challange Challange { get; set; }
    }
}