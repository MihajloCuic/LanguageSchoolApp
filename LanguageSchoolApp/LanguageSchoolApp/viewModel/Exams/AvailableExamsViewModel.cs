using LanguageSchoolApp.core;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.model.Courses;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.service.Courses;

namespace LanguageSchoolApp.viewModel.Exams
{
    public class AvailableExamsViewModel : ObservableObject
    {
        private readonly IExamService examService;
        private readonly ICourseService courseService;
        private Student _student;

        public ExamFilterViewModel ExamFilterVM { get; }
        public ExamSortingViewModel ExamSortingVM { get; }
        public CancelExamFiltersViewModel CancelExamFiltersVM { get; }

        private ObservableCollection<Exam> _availableExams;
        private List<Exam> _allAvailableExams;
        private int _pageNumber;

        public ObservableCollection<Exam> AvailableExams
        { 
            get { return _availableExams; }
            set
            { 
                _availableExams = value;
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

        public RelayCommand<int> ApplyCommand { get; set; }
        public RelayCommand<object> PreviousPageCommand { get; set; }
        public RelayCommand<object> NextPageCommand { get; set; }

        public AvailableExamsViewModel(Student currentStudent) 
        {
            examService = App.ServiceProvider.GetService<IExamService>();
            courseService = App.ServiceProvider.GetService<ICourseService>();
            PageNumber = 1;

            _student = currentStudent;
            _allAvailableExams = GetAvailableExams();
            AvailableExams = new ObservableCollection<Exam>(GetSlicedAvailableExams());

            ExamFilterVM = new ExamFilterViewModel(this);
            ExamSortingVM = new ExamSortingViewModel(this);
            CancelExamFiltersVM = new CancelExamFiltersViewModel(ExamFilterVM);

            ApplyCommand = new RelayCommand<int>(Apply, CanApply);
            PreviousPageCommand = new RelayCommand<object>(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand<object>(NextPage, CanNextPage);
        }

        public void FilterList(string languageNameFilter, string languageLevelFilter) 
        {
            UpdateExamList(examService.GetAllFilteredExams(GetAvailableExams(), languageNameFilter, languageLevelFilter));
        }

        public void SortList(string examDateSorting)
        { 
            UpdateExamList(examService.SortExams(_allAvailableExams, examDateSorting));
        }

        public void UpdateExamList(List<Exam> examList) 
        {
            _allAvailableExams = examList;
            AvailableExams.Clear();
            foreach (var exam in GetSlicedAvailableExams()) 
            { 
                AvailableExams.Add(exam);
            }
        }

        private List<Exam> GetAvailableExams() 
        {
            List<int> coursesIds = _student.FinishedCourses.Select(fc => fc.CourseId).ToList();
            List<Course> finishedCourses = courseService.GetAllCoursesById(coursesIds);
            return examService.GetAvailableExams(finishedCourses);
        }

        private List<Exam> GetSlicedAvailableExams()
        { 
            int elementsToSkip = (PageNumber - 1) * 6;
            return _allAvailableExams.Skip(elementsToSkip).Take(6).ToList();
        }

        private bool CanNextPage(object? parameter) { return PageNumber < (double)_allAvailableExams.Count / 6; }
        private void NextPage(object? parameter)
        {
            PageNumber++;
            AvailableExams.Clear();
            foreach (var course in GetSlicedAvailableExams())
            {
                AvailableExams.Add(course);
            }
        }

        private bool CanPreviousPage(object? parameter) { return PageNumber > 1; }
        private void PreviousPage(object? parameter)
        {
            PageNumber--;
            AvailableExams.Clear();
            foreach (var course in GetSlicedAvailableExams())
            {
                AvailableExams.Add(course);
            }
        }

        private bool CanApply(int examId) { return true; }
        private void Apply(int examId)
        { 
            //TODO: Implement appllication to exams
        }
    }
}
