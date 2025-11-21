namespace SmartBudgetTracker.Models
{
    public class DeviceToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FcmToken { get; set; }
        public string? DeviceType { get; set; } // "web", "android", "ios"
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUsedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public User User { get; set; } = null!;
    }
}