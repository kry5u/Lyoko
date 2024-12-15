using Microsoft.AspNetCore.Identity;

namespace Lyoko.Models
{
    public class Replay
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int Input { get; set; }
        public int Score { get; set; }
        public Level Level { get; set; }
    }
}
