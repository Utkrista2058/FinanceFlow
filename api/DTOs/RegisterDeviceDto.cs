namespace SmartBudgetTracker.DTOs
{
    public class RegisterDeviceDto
    {
        public int UserId { get; set; }
        public string FcmToken { get; set; }
        public string? DeviceType { get; set; } // optional
    }
}
