using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Courses
{
    public class ClassPeriod
    {
        public DaysOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }

        public ClassPeriod(DaysOfWeek _dayOfWeek, TimeOnly _startTime) 
        { 
            DayOfWeek = _dayOfWeek;
            StartTime = _startTime;
        }

        public override bool Equals(object? obj)
        {
            if (obj is ClassPeriod other)
            { 
                return this.DayOfWeek == other.DayOfWeek && this.StartTime == other.StartTime;
            }
            return false;
        }
    }
}
