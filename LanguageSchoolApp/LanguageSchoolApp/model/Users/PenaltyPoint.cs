using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Users
{
    public class PenaltyPoint
    {
        public int Id { get; set; }
        public PenaltyReason Reason { get; set; }
        public DateTime ReceivedDate { get; set; }

        public PenaltyPoint() { }

        public PenaltyPoint(string _studentId, PenaltyReason _reason, DateTime _receivedDate) { 
            Id = GenerateId(_studentId, _receivedDate, _reason);
            Reason = _reason;
            ReceivedDate = _receivedDate;
        }

        private int GenerateId(string studentId, DateTime receivedDate, PenaltyReason reason) {
            string combined = studentId + receivedDate.ToString("ddMMyyyy") + reason.ToString();
            return combined.GetHashCode();
        }
    }
}
