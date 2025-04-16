using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Courses
{
    public class CourseDropoutRequest
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public DropoutReason Reason { get; set; }
        public string Details { get; set; }
        public bool Pending { get; set; }

        public CourseDropoutRequest(int _id, string _studentId, int _courseId, DropoutReason _reason, string _details)
        { 
            Id = _id;
            StudentId = _studentId;
            CourseId = _courseId;
            Reason = _reason;
            Details = _details;
            Pending = true;
        }
    }
}
