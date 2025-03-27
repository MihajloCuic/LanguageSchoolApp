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
        public int ExamId { get; set; }
        public int TotalScore { get; set; }
        public Dictionary<ExamPart, int> PartialScores { get; set; }

        public ExamResults() { }

        public ExamResults(int _examId, int _totalScore) 
        { 
            ExamId = _examId;
            TotalScore = _totalScore;
        }
    }
}
