using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bunker.Database.Entities
{
    public class Company
    {
        public Company()
        {
            Challanges = new List<Challange>();
            Players    = new List<CompanyPlayer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Desciprion { get; set; }

        public ICollection<Challange> Challanges { get; set; }

        public ICollection<CompanyPlayer> Players { get; set; }
        
        public CompanyJoinInfo CompanyJoinInfo { get; set; }
    }
}