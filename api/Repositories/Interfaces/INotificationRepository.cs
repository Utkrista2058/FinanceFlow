using SmartBudgetTracker.Models;

namespace SmartBudgetTracker.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetUserNotificationsAsync(int userId, int limit = 50);
        Task<int> GetUnreadCountAsync(int userId);
        Task<Notification> CreateAsync(Notification notification);
        Task MarkAsReadAsync(int notificationId, int userId);
        Task MarkAllAsReadAsync(int userId);
    }
}