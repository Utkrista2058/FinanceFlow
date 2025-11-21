//using Microsoft.EntityFrameworkCore;
//using SmartBudgetTracker.Data;
//using SmartBudgetTracker.Models;
//using SmartBudgetTracker.Repositories.Interfaces;

//namespace SmartBudgetTracker.Repositories
//{
//    public class NotificationRepository : INotificationRepository
//    {
//        private readonly BudgetDbContext _context;

//        public NotificationRepository(BudgetDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<Notification>> GetUserNotificationsAsync(int userId, int limit = 50)
//        {
//            return await _context.Notifications
//                .Where(n => n.UserId == userId)
//                .OrderByDescending(n => n.CreatedAt)
//                .Take(limit)
//                .ToListAsync();
//        }

//        public async Task<int> GetUnreadCountAsync(int userId)
//        {
//            return await _context.Notifications
//                .Where(n => n.UserId == userId && !n.IsRead)
//                .CountAsync();
//        }

//        public async Task<Notification> CreateAsync(Notification notification)
//        {
//            _context.Notifications.Add(notification);
//            await _context.SaveChangesAsync();
//            return notification;
//        }

//        public async Task MarkAsReadAsync(int notificationId, int userId)
//        {
//            var notification = await _context.Notifications
//                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

//            if (notification != null)
//            {
//                notification.IsRead = true;
//                await _context.SaveChangesAsync();
//            }
//        }

//        public async Task MarkAllAsReadAsync(int userId)
//        {
//            var notifications = await _context.Notifications
//                .Where(n => n.UserId == userId && !n.IsRead)
//                .ToListAsync();

//            foreach (var notification in notifications)
//            {
//                notification.IsRead = true;
//            }

//            await _context.SaveChangesAsync();
//        }
//    }
//}


using Microsoft.EntityFrameworkCore;
using SmartBudgetTracker.Data;
using SmartBudgetTracker.Models;
using SmartBudgetTracker.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly BudgetDbContext _context;

        public NotificationRepository(BudgetDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveTokenAsync(int userId, string fcmToken, string email)
        {
            var existingToken = await _context.UserFCMTokens
                .FirstOrDefaultAsync(t => t.UserId == userId && t.FcmToken == fcmToken);

            if (existingToken != null)
            {
                // Update existing token
                existingToken.UpdatedAt = DateTime.UtcNow;
                existingToken.IsActive = true;
                existingToken.Email = email;
                await _context.SaveChangesAsync();
                return existingToken.Id;
            }
            else
            {
                // Create new token
                var newToken = new UserFCMToken
                {
                    UserId = userId,
                    FcmToken = fcmToken,
                    Email = email,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.UserFCMTokens.Add(newToken);
                await _context.SaveChangesAsync();
                return newToken.Id;
            }
        }

        public async Task<List<string>> GetUserTokensAsync(int userId)
        {
            return await _context.UserFCMTokens
                .Where(t => t.UserId == userId && t.IsActive)
                .Select(t => t.FcmToken)
                .ToListAsync();
        }

        public async Task<bool> DeactivateTokenAsync(string fcmToken)
        {
            var token = await _context.UserFCMTokens
                .FirstOrDefaultAsync(t => t.FcmToken == fcmToken);

            if (token != null)
            {
                token.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<UserFCMToken> GetTokenAsync(int userId, string fcmToken)
        {
            return await _context.UserFCMTokens
                .FirstOrDefaultAsync(t => t.UserId == userId && t.FcmToken == fcmToken);
        }
    }
}