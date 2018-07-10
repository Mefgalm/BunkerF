using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Database.Entities
{
    public class PlayerTask
    {
        public int PlayerId { get; set; }
        
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }
        
        public int TaskId { get; set; }
        
        [ForeignKey(nameof(TaskId))]
        public Task Task { get; set; }
    }
}