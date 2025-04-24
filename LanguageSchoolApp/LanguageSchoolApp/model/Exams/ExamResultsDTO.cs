using LanguageSchoolApp.model.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Exams
{
    public class ExamResultsDTO
    {
        public int ExamId { get; set; }
        public LanguageProficiency LanguageProficiency { get; set; }
        public int TotalScore { get; set; }
        public Dictionary<ExamPart, int> PartialScores { get; set; }
        public bool Passed { get; set; }

        public ExamResultsDTO() { }

        public ExamResultsDTO(int _examId, LanguageProficiency _languageProficiency, int _totalScore, Dictionary<ExamPart, int> _partialScores, bool _passed)
        { 
            ExamId = _examId;
            LanguageProficiency = _languageProficiency;
            TotalScore = _totalScore;
            PartialScores = _partialScores;
            Passed = _passed;
        }
    }
}
