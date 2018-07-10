using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Database.Entities
{
    public class Role
    {
        public Role()
        {
            Players = new List<PlayerRole>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public ICollection<PlayerRole> Players { get; set; }
    }
}