using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.model.Notifications
{
    public class Notification
    {
        public int Id { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Message { get; set; }

        public Notification() { }

        public Notification(User _sender, User _receiver, NotificationType _notificationType, string _message)
        {
            Id = GenerateId(_sender.Email, _receiver.Email, _notificationType);
            Sender = _sender;
            Receiver = _receiver;
            NotificationType = _notificationType;
            Message = _message;
        }

        private int GenerateId(string senderId, string receiverId, NotificationType notificationType) {
            string combined = senderId + receiverId + notificationType.ToString() + DateTime.Now.ToString("ddMMyyyy");
            return combined.GetHashCode();
        }
    }
}
