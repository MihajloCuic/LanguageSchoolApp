using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.exceptions.Exams
{
    public enum ExamExceptionType 
    { 
        ExamNotFound,
        ExamAlreadyExists,
        InvalidLanguageName,
        InvalidLanguageLevel,
        InvalidExamDate,
        InvalidMaxParticipants,
        TeacherIsBusy
    }

    public class ExamException : Exception
    {
        public string Text { get; set; }
        public ExamExceptionType Type { get; set; }

        public ExamException(string _text, ExamExceptionType _type) 
        { 
            Text = _text;
            Type = _type;
        }
    }
}
