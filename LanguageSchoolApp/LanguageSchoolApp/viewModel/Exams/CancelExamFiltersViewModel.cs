using LanguageSchoolApp.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.viewModel.Exams
{
    public class CancelExamFiltersViewModel : ObservableObject
    {
        public ExamFilterViewModel ExamFilterVM { get; }
        public CancelExamFiltersViewModel(ExamFilterViewModel examFilterVM) 
        { 
            ExamFilterVM = examFilterVM;
        }
    }
}
