using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public List<Notification> GetUnreadNotificationsByReceiver(string receiverId) 
        { 
            return notificationRepository.GetUnreadNotificationsByReceiver(receiverId);
        }

        public void AddNotification(int id, Notification notification) 
        { }

        public void DeleteNotification(int id) 
        { }

        public int GenerateId(string senderId, string receiverId, NotificationType notificationType)
        {
            string combined = senderId + receiverId + notificationType.ToString() + DateTime.Now.ToString("ddMMyyyy");
            return combined.GetHashCode();
        }
    }
}
