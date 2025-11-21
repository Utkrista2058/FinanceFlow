//using SmartBudgetTracker.Models;

//namespace SmartBudgetTracker.Repositories.Interfaces
//{
//    public interface INotificationRepository
//    {
//        Task<List<Notification>> GetUserNotificationsAsync(int userId, int limit = 50);
//        Task<int> GetUnreadCountAsync(int userId);
//        Task<Notification> CreateAsync(Notification notification);
//        Task MarkAsReadAsync(int notificationId, int userId);
//        Task MarkAllAsReadAsync(int userId);
//    }
//}

using System.Collections.Generic;
using System.Threading.Tasks;
using SmartBudgetTracker.Models;

namespace SmartBudgetTracker.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<int> SaveTokenAsync(int userId, string fcmToken, string email);
        Task<List<string>> GetUserTokensAsync(int userId);
        Task<bool> DeactivateTokenAsync(string fcmToken);
        Task<UserFCMToken> GetTokenAsync(int userId, string fcmToken);
    }
}