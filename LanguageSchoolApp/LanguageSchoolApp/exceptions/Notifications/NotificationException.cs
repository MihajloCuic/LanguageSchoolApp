using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.exceptions.Notifications
{
    public enum NotificationExceptionType
    { 
        NotificationNotFound
    }

    class NotificationException : Exception
    {
        public string Text { get; set; }
        public NotificationExceptionType Type { get; set; }

        public NotificationException(string _text, NotificationExceptionType _type) 
        { 
            Text = _text;
            Type = _type;
        }
    }
}
