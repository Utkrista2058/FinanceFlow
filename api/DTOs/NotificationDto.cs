//namespace SmartBudgetTracker.DTOs
//{
//    public class NotificationDto
//    {
//        public int Id { get; set; }
//        public string Title { get; set; }
//        public string Message { get; set; }
//        public string Type { get; set; }
//        public bool IsRead { get; set; }
//        public string? Link { get; set; }
//        public DateTime CreatedAt { get; set; }
//    }

//    public class CreateNotificationDto
//    {
//        public int UserId { get; set; }
//        public string Title { get; set; }
//        public string Message { get; set; }
//        public string Type { get; set; }
//        public string? Link { get; set; }
//    }
//}


using System.Collections.Generic;

namespace SmartBudgetTracker.DTOs
{
    public class RegisterTokenRequest
    {
        public int UserId { get; set; }
        public string FcmToken { get; set; }
        public string Email { get; set; }
    }

    public class SendNotificationRequest
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}