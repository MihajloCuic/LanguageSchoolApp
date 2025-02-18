using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.model.Notifications
{
    public class Notification
    {
        public static readonly string filename = Path.Combine("..", "..", "..", "data", "Notifications.json");

        public User Sender { get; set; }
        public User Receiver { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Message { get; set; }

        public Notification() { }

        public Notification(User _sender, User _receiver, NotificationType _notificationType, string _message)
        {
            Sender = _sender;
            Receiver = _receiver;
            NotificationType = _notificationType;
            Message = _message;
        }
    }
}
