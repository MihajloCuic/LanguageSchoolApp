using LanguageSchoolApp.model.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Courses
{
    public class CourseApplication
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public bool Paused { get; set; }
        public AcceptationType Acceptance { get; set; }

        public CourseApplication() { }

        public CourseApplication(int _id, string _studentId, int _courseId, bool _paused, AcceptationType _acceptance) { 
            Id = _id;
            StudentId = _studentId;
            CourseId = _courseId;
            Paused = _paused;
            Acceptance = _acceptance;
        }

    }
}
