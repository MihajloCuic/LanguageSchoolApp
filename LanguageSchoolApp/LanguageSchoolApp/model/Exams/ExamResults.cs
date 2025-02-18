using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Exams
{
    public class ExamResults
    {
        public Exam Exam { get; set; }
        public int totalScore { get; set; }
        public Dictionary<ExamPart, int> partialScores { get; set; }

        public ExamResults() { }
    }
}
