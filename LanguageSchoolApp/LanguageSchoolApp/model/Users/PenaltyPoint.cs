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
        public bool Deleted { get; set; }

        public PenaltyPoint() { }

        public PenaltyPoint(int _id, PenaltyReason _reason, DateTime _receivedDate) {
            Id = _id;
            Reason = _reason;
            ReceivedDate = _receivedDate;
            Deleted = false;
        }
    }
}
