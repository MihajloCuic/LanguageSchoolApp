using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Courses
{
    public class Course
    {
        public int Id { get; set; }
        public LanguageProficiency LanguageProficiency { get; set; }
        public int MaxParticipants { get; set; }
        public int Duration { get; set; }
        public DateTime ClassDays { get; set; }
        public DateTime BeginningDate { get; set; }
        public CourseType CourseType { get; set; }

        public Course() { }

        public Course(int _id, LanguageProficiency _languageProficiency, int _maxParticipants, 
            int _duration, DateTime _classDays, DateTime _beginningDate, CourseType _courseType)
        {
            Id = _id;
            LanguageProficiency = _languageProficiency;
            MaxParticipants = _maxParticipants;
            Duration = _duration;
            ClassDays = _classDays;
            BeginningDate = _beginningDate;
            CourseType = _courseType;
        }

        public Course(LanguageProficiency _languageProficiency, int _maxParticipants, int _duration, 
            DateTime _classDays, DateTime _beginningDate, CourseType _courseType) {
            Id = generateId(_languageProficiency, _beginningDate, _courseType);
            LanguageProficiency = _languageProficiency;
            MaxParticipants = _maxParticipants;
            Duration = _duration;
            ClassDays = _classDays;
            BeginningDate = _beginningDate;
            CourseType = _courseType;
        }

        private int generateId(LanguageProficiency languageProficiency, DateTime beginningDate, CourseType courseType) {
            string combination = languageProficiency.LanguageName + 
                                languageProficiency.LanguageLevel.ToString() + 
                                beginningDate.ToString("ddMMyyyy") + courseType.ToString();
            return combination.GetHashCode();
        }
    }
}
