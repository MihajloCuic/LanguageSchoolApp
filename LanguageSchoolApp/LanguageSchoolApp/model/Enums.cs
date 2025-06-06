﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model
{
    public enum Gender
    {
        Male,
        Female
    }

    public enum ProfessionalDegree
    {
        ElementarySchool,
        HighSchool,
        VocationalSchool,
        Collage,
        Master,
        Doctorate
    }

    public enum DaysOfWeek
    { 
        Monday = 1,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
    public enum LanguageLevel
    {
        A1,
        A2,
        B1,
        B2,
        C1,
        C2
    }

    public enum PenaltyReason
    {
        MissedClass,
        ClassDisturbance,
        AssignmentMissed,
        CourseDropout
    }

    public enum ExamPart
    {
        Reading,
        Writing,
        Listening,
        Speaking
    }

    public enum CourseType 
    { 
        Live,
        Online
    }

    public enum NotificationType 
    { 
        CourseApplicationResponse,
        PenaltyGiven,
        DropoutRequest,
        CourseFinished,
        ExamResults,
        ExamAssigned,
        CourseAssigned
    }

    public enum SortingDirection 
    { 
        None,
        Descending,
        Ascending
    }

    public enum AcceptationType
    { 
        Pending,
        Accepted,
        Denied
    }

    public enum DropoutReason
    { 
        Other,
        CourseDifficult,
        NotInterested,
        InadequateTeachingStyle
    }
}
