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
        public int Id { get; set; }
        public string StudentId {  get; set; }
        public int ExamId { get; set; }
        public AcceptationType Acceptance { get; set; }

        public ExamApplication() { }

        public ExamApplication(int _id, string _studentId, int _examId, AcceptationType _acceptance) { 
            Id = _id;
            StudentId = _studentId;
            ExamId = _examId;
            Acceptance = _acceptance;
        }
    }
}
