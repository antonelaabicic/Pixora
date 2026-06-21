namespace Pixora.DAL.Models
{
    public class UserActionLog
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public UserActionType ActionType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Details { get; set; } = string.Empty;
    }
}