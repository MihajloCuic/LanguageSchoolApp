using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Courses
{
    public class FinishedCourse
    {
        public int CourseId { get; set; }
        public int Grade { get; set; }

        public FinishedCourse() { }

        public FinishedCourse(int _courseId, int _grade) { 
            CourseId = _courseId;
            Grade = _grade;
        }
    }
}
