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
        public List<string> ParticipantsIds { get; set; }
        public int Duration { get; set; }
        public List<ClassPeriod> ClassPeriods { get; set; }
        public DateTime BeginningDate { get; set; }
        public CourseType CourseType { get; set; }

        public Course() { }

        public Course(int _id, LanguageProficiency _languageProficiency, int _maxParticipants, 
            int _duration, List<ClassPeriod> _classPeriods, DateTime _beginningDate, CourseType _courseType)
        {
            Id = _id;
            LanguageProficiency = _languageProficiency;
            MaxParticipants = _maxParticipants;
            Duration = _duration;
            ClassPeriods = _classPeriods;
            BeginningDate = _beginningDate;
            CourseType = _courseType;
            ParticipantsIds = new List<string> ();
        }

        public Course(int _id, LanguageProficiency _languageProficiency, int _maxParticipants,
            int _duration, List<ClassPeriod> _classPeriods, DateTime _beginningDate, CourseType _courseType, List<string> _participantsIds)
        {
            Id = _id;
            LanguageProficiency = _languageProficiency;
            MaxParticipants = _maxParticipants;
            Duration = _duration;
            ClassPeriods = _classPeriods;
            BeginningDate = _beginningDate;
            CourseType = _courseType;
            ParticipantsIds = _participantsIds;
        }

        public string ClassPeriodsToString() 
        {
            return ClassPeriods.Count + "days/w";
        }
    }
}
