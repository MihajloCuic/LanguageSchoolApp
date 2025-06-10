using LanguageSchoolApp.model.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.repository.Notifications
{
    public interface INotificationRepository
    {
        Dictionary<int, Notification> GetAllNotifications();
        Notification GetNotification(int id);
        bool NotificationExists(int id);
        List<Notification> GetUnreadNotificationsByReceiver(string receiverId);
        void AddNotification(int id, Notification notification);
        void DeleteNotification(int id);
        void WriteToFile();
    }
}
