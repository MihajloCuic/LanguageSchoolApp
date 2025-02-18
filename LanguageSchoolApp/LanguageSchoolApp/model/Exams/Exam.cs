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

        public Exam() { }

        public Exam(int _id, LanguageProficiency _languageProficiency, DateTime _examDate, int _maxParticipants) {
            Id = _id;
            LanguageProficiency = _languageProficiency;
            ExamDate = _examDate;
            MaxParticipants = _maxParticipants;
        }

        public Exam(LanguageProficiency _languageProficiency, DateTime _examDate, int _maxParticipants) {
            Id = generateId(_languageProficiency, _examDate);
            LanguageProficiency = _languageProficiency;
            ExamDate = _examDate;
            MaxParticipants = _maxParticipants;
        }

        private int generateId(LanguageProficiency languageProficiency, DateTime examDate) { 
            string combination = languageProficiency.LanguageName + languageProficiency.LanguageLevel.ToString() + examDate.ToString("ddMMyyyy");
            return combination.GetHashCode();
        }
    }
}
