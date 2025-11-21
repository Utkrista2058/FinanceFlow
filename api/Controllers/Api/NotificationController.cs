//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SmartBudgetTracker.DTOs;
//using SmartBudgetTracker.Models;
//using SmartBudgetTracker.Repositories.Interfaces;
//using System.Security.Claims;

//namespace SmartBudgetTracker.Controllers.Api
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize]
//    public class NotificationController : ControllerBase
//    {
//        private readonly INotificationRepository _notificationRepo;

//        public NotificationController(INotificationRepository notificationRepo)
//        {
//            _notificationRepo = notificationRepo;
//        }

//        // GET: api/notification
//        [HttpGet]
//        public async Task<IActionResult> GetNotifications([FromQuery] int limit = 50)
//        {
//            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
//            var notifications = await _notificationRepo.GetUserNotificationsAsync(userId, limit);

//            var notificationDtos = notifications.Select(n => new NotificationDto
//            {
//                Id = n.Id,
//                Title = n.Title,
//                Message = n.Message,
//                Type = n.Type,
//                IsRead = n.IsRead,
//                Link = n.Link,
//                CreatedAt = n.CreatedAt
//            }).ToList();

//            return Ok(notificationDtos);
//        }

//        // GET: api/notification/unread-count
//        [HttpGet("unread-count")]
//        public async Task<IActionResult> GetUnreadCount()
//        {
//            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
//            var count = await _notificationRepo.GetUnreadCountAsync(userId);
//            return Ok(new { count });
//        }

//        // PATCH: api/notification/5/read
//        [HttpPatch("{id}/read")]
//        public async Task<IActionResult> MarkAsRead(int id)
//        {
//            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
//            await _notificationRepo.MarkAsReadAsync(id, userId);
//            return Ok(new { message = "Notification marked as read" });
//        }

//        // PATCH: api/notification/mark-all-read
//        [HttpPatch("mark-all-read")]
//        public async Task<IActionResult> MarkAllAsRead()
//        {
//            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
//            await _notificationRepo.MarkAllAsReadAsync(userId);
//            return Ok(new { message = "All notifications marked as read" });
//        }

//        // POST: api/notification (for testing or admin)
//        [HttpPost]
//        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
//        {
//            var notification = new Notification
//            {
//                UserId = dto.UserId,
//                Title = dto.Title,
//                Message = dto.Message,
//                Type = dto.Type,
//                Link = dto.Link
//            };

//            await _notificationRepo.CreateAsync(notification);
//            return Ok(notification);
//        }
//    }
//}



using Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBudgetTracker.DTOs;
using SmartBudgetTracker.Models;
using SmartBudgetTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBudgetTracker.Data;


namespace SmartBudgetTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly BudgetDbContext _context;
        private readonly IFirebaseNotificationService _notificationService;

        public NotificationsController(
            BudgetDbContext context,
            IFirebaseNotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // POST: api/notifications/register
        //[HttpPost("register")]
        //public async Task<IActionResult> RegisterToken([FromBody] RegisterTokenRequest request)
        //{
        //    try
        //    {
        //        // Check if token already exists for this user
        //        var existingToken = await _context.UserFCMTokens
        //            .FirstOrDefaultAsync(t => t.UserId == request.UserId && t.FcmToken == request.FcmToken);

        //        if (existingToken != null)
        //        {
        //            // Update existing token
        //            existingToken.UpdatedAt = DateTime.UtcNow;
        //            existingToken.IsActive = true;
        //            existingToken.Email = request.Email;
        //        }
        //        else
        //        {
        //            // Create new token
        //            var newToken = new UserFCMToken
        //            {
        //                UserId = request.UserId,
        //                FcmToken = request.FcmToken,
        //                Email = request.Email,
        //                CreatedAt = DateTime.UtcNow,
        //                UpdatedAt = DateTime.UtcNow,
        //                IsActive = true
        //            };

        //            _context.UserFCMTokens.Add(newToken);
        //        }

        //        await _context.SaveChangesAsync();

        //        return Ok(new
        //        {
        //            message = "Token registered successfully",
        //            userId = request.UserId
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error registering token: {ex.Message}");
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}
        [HttpPost("register")]
        public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceDto dto)
        {
            try
            {
                // Check if user exists
                var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
                if (!userExists)
                {
                    return BadRequest(new { error = $"User with ID {dto.UserId} does not exist" });
                }

                // Check if token already exists for this user
                var existingToken = await _context.DeviceTokens
                    .FirstOrDefaultAsync(dt => dt.UserId == dto.UserId && dt.FcmToken == dto.FcmToken);

                if (existingToken != null)
                {
                    // Update existing token
                    existingToken.LastUsedAt = DateTime.UtcNow;
                    existingToken.IsActive = true;
                }
                else
                {
                    // Create new token
                    var deviceToken = new DeviceToken
                    {
                        UserId = dto.UserId,
                        FcmToken = dto.FcmToken,
                        DeviceType = "web", // or get from dto
                        RegisteredAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    await _context.DeviceTokens.AddAsync(deviceToken);
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Device registered successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering device:", ex.Message);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/notifications/send
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest request)
        {
            try
            {
                // Get all active tokens for this user
                var tokens = await _context.UserFCMTokens
                    .Where(t => t.UserId == request.UserId && t.IsActive)
                    .Select(t => t.FcmToken)
                    .ToListAsync();

                if (!tokens.Any())
                {
                    return BadRequest(new { error = "No active FCM tokens found for this user" });
                }

                // Send to all user's devices
                var successCount = 0;
                var failedTokens = new List<string>();

                foreach (var token in tokens)
                {
                    try
                    {
                        await _notificationService.SendNotificationAsync(
                            token,
                            request.Title,
                            request.Body,
                            request.Data
                        );
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send to token: {ex.Message}");
                        failedTokens.Add(token);

                        // Deactivate invalid tokens
                        if (ex.Message.Contains("registration-token-not-registered") ||
                            ex.Message.Contains("invalid-registration-token"))
                        {
                            var tokenToDeactivate = await _context.UserFCMTokens
                                .FirstOrDefaultAsync(t => t.FcmToken == token);

                            if (tokenToDeactivate != null)
                            {
                                tokenToDeactivate.IsActive = false;
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }

                return Ok(new
                {
                    message = "Notifications processed",
                    successCount = successCount,
                    failedCount = failedTokens.Count,
                    totalDevices = tokens.Count
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: api/notifications/tokens/{userId}
        [HttpGet("tokens/{userId}")]
        public async Task<IActionResult> GetUserTokens(int userId)
        {
            var tokens = await _context.UserFCMTokens
                .Where(t => t.UserId == userId && t.IsActive)
                .ToListAsync();

            return Ok(tokens);
        }

        // DELETE: api/notifications/unregister
        [HttpPost("unregister")]
        public async Task<IActionResult> UnregisterToken([FromBody] RegisterTokenRequest request)
        {
            var token = await _context.UserFCMTokens
                .FirstOrDefaultAsync(t => t.UserId == request.UserId && t.FcmToken == request.FcmToken);

            if (token != null)
            {
                token.IsActive = false;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Token unregistered successfully" });
            }

            return NotFound(new { error = "Token not found" });
        }
    }
}