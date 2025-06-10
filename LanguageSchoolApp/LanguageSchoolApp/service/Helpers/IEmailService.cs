using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.service.Helpers
{
    public interface IEmailService
    {
        void SendCourseResults(List<Student> students, Course course);
    }
}
