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
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }

        public Notification() { }

        public Notification(int _id, string _senderId, string _receiverId, NotificationType _notificationType, string _message, bool _isRead)
        {
            Id = _id;
            SenderId = _senderId;
            ReceiverId = _receiverId;
            NotificationType = _notificationType;
            Message = _message;
            IsRead = _isRead;
        }
    }
}
