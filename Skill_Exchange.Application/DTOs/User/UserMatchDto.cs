namespace Skill_Exchange.Application.DTOs.User
{
    public class UserMatchDTO
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public double MatchScore { get; set; }
        public List<string> Skills { get; set; } = new();
        public string? ImageUrl { get; set; }
        public DateTime LastActiveAt { get; set; }
    }
}
