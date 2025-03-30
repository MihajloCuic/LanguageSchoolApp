using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.service.Notifications
{
    public interface INotificationService
    {
        Dictionary<int, Notification> GetAllNotifications();
        Notification GetNotification(int id);
        List<Notification> GetUnreadNotificationsByReceiver(string receiverId);
        void AddNotification(int id, Notification notification);
        void DeleteNotification(int id);
        int GenerateId(string senderId, string receiverId, NotificationType notificationType);
    }
}
