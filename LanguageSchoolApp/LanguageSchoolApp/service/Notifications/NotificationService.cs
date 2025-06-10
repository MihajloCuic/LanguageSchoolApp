using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.exceptions.Notifications;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Notifications;
using LanguageSchoolApp.repository.Notifications;

namespace LanguageSchoolApp.service.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository notificationRepository;

        public NotificationService(INotificationRepository _notificationRepository)
        {
            notificationRepository = _notificationRepository;
        }

        public Dictionary<int, Notification> GetAllNotifications() 
        { 
            return notificationRepository.GetAllNotifications();
        }

        public Notification GetNotification(int id) 
        { 
            return notificationRepository.GetNotification(id);
        }

        public bool NotificationExists(int id)
        { 
            return notificationRepository.NotificationExists(id);
        }

        public List<Notification> GetUnreadNotificationsByReceiver(string receiverId) 
        { 
            return notificationRepository.GetUnreadNotificationsByReceiver(receiverId);
        }

        public void AddNotification(string senderId, string receiverId, NotificationType notificationType, string message) 
        { 
            int notificationId = GenerateId(senderId, receiverId, notificationType);
            Notification notification = new Notification(notificationId, senderId, receiverId, notificationType, message, false);
            notificationRepository.AddNotification(notificationId, notification);
        }

        public void DeleteNotification(int id) 
        {
            if (!NotificationExists(id)) 
            {
                throw new NotificationException("Notification not found !", NotificationExceptionType.NotificationNotFound);
            }
            notificationRepository.DeleteNotification(id);
        }

        public int GenerateId(string senderId, string receiverId, NotificationType notificationType)
        {
            string combined = senderId + receiverId + notificationType.ToString() + DateTime.Now.ToString("ddMMyyyy");
            return combined.GetHashCode();
        }
    }
}
