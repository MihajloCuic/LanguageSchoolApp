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
        public bool Passed { get; set; }

        public ExamResults() { }

        public ExamResults(int _examId, int _totalScore, Dictionary<ExamPart, int> _partialScores, bool _passed) 
        { 
            ExamId = _examId;
            TotalScore = _totalScore;
            PartialScores = _partialScores;
            Passed = _passed;
        }
    }
}
