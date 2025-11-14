using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBudgetTracker.DTOs;
using SmartBudgetTracker.Models;
using SmartBudgetTracker.Repositories.Interfaces;
using System.Security.Claims;

namespace SmartBudgetTracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepo;

        public NotificationController(INotificationRepository notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }

        // GET: api/notification
        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] int limit = 50)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var notifications = await _notificationRepo.GetUserNotificationsAsync(userId, limit);

            var notificationDtos = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                IsRead = n.IsRead,
                Link = n.Link,
                CreatedAt = n.CreatedAt
            }).ToList();

            return Ok(notificationDtos);
        }

        // GET: api/notification/unread-count
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var count = await _notificationRepo.GetUnreadCountAsync(userId);
            return Ok(new { count });
        }

        // PATCH: api/notification/5/read
        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _notificationRepo.MarkAsReadAsync(id, userId);
            return Ok(new { message = "Notification marked as read" });
        }

        // PATCH: api/notification/mark-all-read
        [HttpPatch("mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _notificationRepo.MarkAllAsReadAsync(userId);
            return Ok(new { message = "All notifications marked as read" });
        }

        // POST: api/notification (for testing or admin)
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
        {
            var notification = new Notification
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Message = dto.Message,
                Type = dto.Type,
                Link = dto.Link
            };

            await _notificationRepo.CreateAsync(notification);
            return Ok(notification);
        }
    }
}