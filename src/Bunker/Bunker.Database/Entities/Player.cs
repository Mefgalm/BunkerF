using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bunker.Database.Entities
{
    public class Player
    {
        public Player()
        {
            Companies = new List<CompanyPlayer>();
            Tasks     = new List<Task>();
            Teams     = new List<PlayerTeam>();
            Roles     = new List<PlayerRole>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(16)]
        public string NickName { get; set; }

        [Required]
        [StringLength(16)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(16)]
        public string LastName { get; set; }

        [Required]
        [StringLength(60)]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public ICollection<CompanyPlayer> Companies { get; set; }

        public ICollection<Task> Tasks { get; set; }

        public ICollection<PlayerTeam> Teams { get; set; }

        public ICollection<PlayerRole> Roles { get; set; }
    }
}