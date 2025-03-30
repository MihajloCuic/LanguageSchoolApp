using LanguageSchoolApp.model.Exams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Notifications;

namespace LanguageSchoolApp.repository.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        private static readonly string filename = Path.Combine("..", "..", "..", "data", "Notifications.json");
        private readonly Dictionary<int, Notification> allNotifications;

        public NotificationRepository() 
        {
            allNotifications = ReadFromFile();
        }

        public Dictionary<int, Notification> GetAllNotifications() 
        { 
            return allNotifications;
        }

        public Notification GetNotification(int id) 
        {
            if (!allNotifications.ContainsKey(id)) 
            { 
                //throw notification exception
            }
            return allNotifications[id];
        }

        public List<Notification> GetUnreadNotificationsByReceiver(string receiverId) 
        { 
            List<Notification> foundNotifications = new List<Notification>();
            foreach (Notification notification in allNotifications.Values) 
            { 
                if(notification.ReceiverId == receiverId && notification.IsRead == false) 
                { 
                    foundNotifications.Add(notification);
                }
            }
            return foundNotifications;
        }

        public void AddNotification(int id, Notification notification) 
        { }

        public void DeleteNotification(int id) 
        { }

        public void WriteToFile()
        {
            try
            {
                string serializedNotifications = JsonConvert.SerializeObject(allNotifications, Formatting.Indented);
                File.WriteAllText(filename, serializedNotifications);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Dictionary<int, Notification> ReadFromFile()
        {
            Dictionary<int, Notification> notifications = new Dictionary<int, Notification>();
            try
            {
                string data = File.ReadAllText(filename);
                notifications = JsonConvert.DeserializeObject<Dictionary<int, Notification>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return notifications;
        }
    }
}
