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
    public class NotificationRepository
    {
        public static readonly string filename = Path.Combine("..", "..", "..", "data", "Notifications.json");

        public static void WriteToFile(Dictionary<int, Notification> allNotifications)
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
            Dictionary<int, Notification> allNotifications = new Dictionary<int, Notification>();
            try
            {
                string data = File.ReadAllText(filename);
                allNotifications = JsonConvert.DeserializeObject<Dictionary<int, Notification>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return allNotifications;
        }
    }
}
