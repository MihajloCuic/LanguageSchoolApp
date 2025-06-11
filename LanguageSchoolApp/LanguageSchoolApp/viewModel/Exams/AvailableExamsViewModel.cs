using LanguageSchoolApp.core;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.model.Courses;
using Microsoft.Extensions.DependencyInjection;
using LanguageSchoolApp.view;
using System.Collections.ObjectModel;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.exceptions.Exams;

namespace LanguageSchoolApp.viewModel.Exams
{
    public class AvailableExamsViewModel : ObservableObject
    {
        private readonly IExamService examService;
        private readonly ICourseService courseService;
        private readonly IExamApplicationService applicationService;
        private readonly Student _student;
        private string _studentId;

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
        public string StudentId
        { 
            get { return _studentId; }
            set
            { 
                _studentId = value;
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
            applicationService = App.ServiceProvider.GetService<IExamApplicationService>();
            PageNumber = 1;

            _student = currentStudent;
            StudentId = currentStudent.Email;
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
            List<int> examIds = _student.FinishedExamResults.Select(fe => fe.ExamId).ToList();
            List<Exam> finishedExams = examService.GetAllExamsById(examIds);
            return examService.GetAvailableExams(finishedCourses, finishedExams);
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

        private bool CanApply(int examId) 
        {
            try
            {
                if (!examService.ExamExists(examId))
                {
                    throw new ExamException("Exam not found !", ExamExceptionType.ExamNotFound);
                }
                Exam exam = examService.GetExam(examId);
                return (exam.ExamDate - DateTime.Now).TotalDays >= 10;
            }
            catch (ExamException) 
            {
                return false;
            }

        }
        private void Apply(int examId)
        {
            Exam exam = examService.GetExam(examId);
            try
            {
                int examApplicationId = applicationService.GenerateId(StudentId, examId);
                if (applicationService.ExamApplicationExists(examApplicationId))
                {
                    applicationService.DeleteExamApplication(examApplicationId);
                    PopupMessageView popup = new PopupMessageView("SUCCESS", $"You successfully withdrew from {exam.LanguageProficiency.LanguageName} {exam.LanguageProficiency.LanguageLevel.ToString()} !");
                    popup.Show();
                }
                else
                { 
                    applicationService.CreateExamApplication(StudentId, examId);
                    PopupMessageView popup = new PopupMessageView("SUCCESS", $"You successfully signed up for {exam.LanguageProficiency.LanguageName} {exam.LanguageProficiency.LanguageLevel.ToString()} !");
                    popup.Show();
                }
                UpdateExamList(_allAvailableExams); //trigger update button content
            }
            catch (ExamApplicationException ex)
            {
                PopupMessageView popup = new PopupMessageView("ERROR", ex.Text);
                popup.Show();
            }
        }
    }
}
