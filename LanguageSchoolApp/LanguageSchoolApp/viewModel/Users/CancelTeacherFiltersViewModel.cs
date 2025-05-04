using LanguageSchoolApp.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.viewModel.Users
{
    public class CancelTeacherFiltersViewModel : ObservableObject
    {
        public TeacherFiltersViewModel TeacherFilterVM { get; }
        public CancelTeacherFiltersViewModel(TeacherFiltersViewModel teacherFiltersViewModel) 
        { 
            TeacherFilterVM = teacherFiltersViewModel;
        }
    }
}
