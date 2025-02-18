using LanguageSchoolApp.model.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Exams
{
    public class ExamApplication
    {
        public Student Student {  get; set; }
        public Exam Exam { get; set; }

        public ExamApplication() { }

        public ExamApplication(Student _student, Exam _exam) { 
            Student = _student;
            Exam = _exam;
        }
    }
}
