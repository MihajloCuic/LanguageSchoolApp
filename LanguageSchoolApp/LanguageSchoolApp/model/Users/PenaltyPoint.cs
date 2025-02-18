using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Users
{
    public class PenaltyPoint
    {
        public PenaltyReason Reason { get; set; }
        public DateTime ReceivedDate { get; set; }

        public PenaltyPoint() { }

        public PenaltyPoint(PenaltyReason _reason, DateTime _receivedDate) { 
            Reason = _reason;
            ReceivedDate = _receivedDate;
        }
    }
}
