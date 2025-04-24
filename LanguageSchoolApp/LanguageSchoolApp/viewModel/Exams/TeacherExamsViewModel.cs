using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.service.Users.Teachers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace LanguageSchoolApp.viewModel.Exams
{
    public class TeacherExamsViewModel : ObservableObject
    {
        private readonly IExamService examService;
        private readonly ITeacherService teacherService;
        private Teacher _teacher;
        public ExamFilterViewModel ExamFilterVM { get; }
        public ExamSortingViewModel ExamSortingVM { get; }
        public CancelExamFiltersViewModel CancelFilterVM { get; }

        private ObservableCollection<Exam> _teacherExams;
        private List<Exam> _allTeacherExams;
        private int _pageNumber;

        public ObservableCollection<Exam> TeacherExams
        { 
            get { return _teacherExams; }
            set
            { 
                _teacherExams = value;
                OnPropertyChanged();
            }
        }
        public int PageNumber
        { 
            get { return _pageNumber; }
            set
            { 
                _pageNumber = value; 
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> PreviousPageCommand { get; set; }
        public RelayCommand<object> NextPageCommand { get; set; }
        public RelayCommand<int> CardButtonCommand { get; set; }
        public Action<int> SwitchToEditExamView { get; set; }

        public TeacherExamsViewModel(string teacherId) 
        {
            examService = App.ServiceProvider.GetService<IExamService>();
            teacherService = App.ServiceProvider.GetService<ITeacherService>();
            ExamFilterVM = new ExamFilterViewModel(this);
            ExamSortingVM = new ExamSortingViewModel(this);
            CancelFilterVM = new CancelExamFiltersViewModel(ExamFilterVM);

            _teacher = teacherService.GetTeacher(teacherId);
            PageNumber = 1;
            _allTeacherExams = examService.GetAllExamsById(_teacher.MyExamsIds);
            TeacherExams = new ObservableCollection<Exam>(GetSlicedTeacherExams());
        
            PreviousPageCommand = new RelayCommand<object>(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand<object>(NextPage, CanNextPage);
            CardButtonCommand = new RelayCommand<int>(CardButton, CanCardButton);
        }

        public void FilterList(string languageNameFilter, string languageLevelFilter)
        { 
            List<Exam> exams = examService.GetAllExamsById(_teacher.MyExamsIds);
            UpdateList(examService.GetAllFilteredExams(exams, languageNameFilter, languageLevelFilter));
        }

        public void SortList(string examDateSorting)
        { 
            UpdateList(examService.SortExams(_allTeacherExams, examDateSorting));
        }

        private void UpdateList(List<Exam> exams)
        {
            _allTeacherExams = exams;
            TeacherExams.Clear();
            foreach (var exam in GetSlicedTeacherExams())
            { 
                TeacherExams.Add(exam);
            }
        }

        private List<Exam> GetSlicedTeacherExams()
        { 
            int elementsToSkip = (PageNumber - 1) * 6;
            return _allTeacherExams.Skip(elementsToSkip).Take(6).ToList();
        }

        private bool CanNextPage(object? parameter) { return PageNumber < (double)_allTeacherExams.Count / 6; }
        private void NextPage(object? parameter)
        {
            PageNumber++;
            TeacherExams.Clear();
            foreach (var course in GetSlicedTeacherExams())
            {
                TeacherExams.Add(course);
            }
        }

        private bool CanPreviousPage(object? parameter) { return PageNumber > 1; }
        private void PreviousPage(object? parameter)
        {
            PageNumber--;
            TeacherExams.Clear();
            foreach (var course in GetSlicedTeacherExams())
            {
                TeacherExams.Add(course);
            }
        }

        private bool CanCardButton(int examId) { return true; }
        private void CardButton(int examId) 
        { 
            Exam exam = examService.GetExam(examId);
            if ((exam.ExamDate - DateTime.Now).TotalDays <= 30)
            {
                //TODO: Implement active exam
            }
            else 
            {
                SwitchToEditExamView(examId);
                //TODO: Implement pending exam
            }
        }
    }
}
