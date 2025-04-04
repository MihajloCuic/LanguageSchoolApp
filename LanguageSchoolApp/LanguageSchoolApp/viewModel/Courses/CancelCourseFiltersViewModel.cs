using LanguageSchoolApp.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class CancelCourseFiltersViewModel : ObservableObject
    {
        public CourseFilterViewModel CourseFilterVM { get; }
        public CancelCourseFiltersViewModel(CourseFilterViewModel courseFilterVM) 
        { 
            CourseFilterVM = courseFilterVM;
        }
    }
}
