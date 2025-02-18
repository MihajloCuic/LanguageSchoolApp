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
        public Student Student { get; set; }
        public Course Course { get; set; }
        public bool Paused { get; set; }
        public bool Accepted { get; set; }

        public CourseApplication() { }

        public CourseApplication(Student _student, Course _course, bool _paused, bool _accepted) { 
            Student = _student;
            Course = _course;
            Paused = _paused;
            Accepted = _accepted;
        }

    }
}
