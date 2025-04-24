using LanguageSchoolApp.model.Courses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Exams
{
    public class Exam
    {
        public int Id { get; set; }
        public LanguageProficiency LanguageProficiency { get; set; }
        public DateTime ExamDate { get; set; }
        public int MaxParticipants { get; set; }
        public List<string> Participants { get; set; }
        public bool IsFinished { get; set; }

        public Exam() { }

        public Exam(int _id, LanguageProficiency _languageProficiency, DateTime _examDate, int _maxParticipants, List<string> _participants, bool _isFinished) {
            Id = _id;
            LanguageProficiency = _languageProficiency;
            ExamDate = _examDate;
            MaxParticipants = _maxParticipants;
            Participants = _participants;
            IsFinished = _isFinished;
        }

        public Exam(int _id, LanguageProficiency _languageProficiency, DateTime _examDate, int _maxParticipants)
        {
            Id = _id;
            LanguageProficiency = _languageProficiency;
            ExamDate = _examDate;
            MaxParticipants = _maxParticipants;
            Participants = new List<string>();
            IsFinished = false;
        }
    }
}
