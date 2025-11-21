using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudgetTracker.Services
{
    public interface IFirebaseNotificationService
    {
        Task<string> SendNotificationAsync(string token, string title, string body, Dictionary<string, string> data = null);
        Task<BatchResponse> SendToMultipleAsync(List<string> tokens, string title, string body);
    }

    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        public FirebaseNotificationService()
        {
            // Initialize Firebase Admin SDK only once
            if (FirebaseApp.DefaultInstance == null)
            {
                try
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile("firebase-adminsdk.json")
                    });
                    Console.WriteLine(" Firebase Admin SDK initialized successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error initializing Firebase: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<string> SendNotificationAsync(
            string token,
            string title,
            string body,
            Dictionary<string, string> data = null)
        {
            try
            {
                var message = new Message()
                {
                    Token = token,
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data,
                    Webpush = new WebpushConfig()
                    {
                        Notification = new WebpushNotification()
                        {
                            Icon = "/favicon.ico",
                            Badge = "/favicon.ico"
                        }
                    }
                };

                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($" Notification sent successfully: {response}");
                return response;
            }
            catch (FirebaseMessagingException ex)
            {
                Console.WriteLine($" Firebase error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error sending notification: {ex.Message}");
                throw;
            }
        }

        public async Task<BatchResponse> SendToMultipleAsync(
            List<string> tokens,
            string title,
            string body)
        {
            var message = new MulticastMessage()
            {
                Tokens = tokens,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                }
            };

            BatchResponse response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            Console.WriteLine($" {response.SuccessCount} notifications sent successfully");

            if (response.FailureCount > 0)
            {
                Console.WriteLine($"{response.FailureCount} notifications failed");
            }

            return response;
        }
    }
}