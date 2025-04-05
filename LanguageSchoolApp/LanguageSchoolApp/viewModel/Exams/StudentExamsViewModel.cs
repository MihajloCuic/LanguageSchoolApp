using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.service.Exams;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageSchoolApp.viewModel.Exams
{
    public class StudentExamsViewModel : ObservableObject
    {
        private readonly IExamService examService;
        private readonly ICourseService courseService;
        private Student _student;

        public ExamFilterViewModel ExamFilterVM { get; }
        public ExamSortingViewModel ExamSortingVM { get; }
        public CancelExamFiltersViewModel CancelExamFiltersVM { get; }

        private ObservableCollection<ExamResultsDTO> _examResults;
        private List<ExamResultsDTO> _results;
        private int _pageNumber;

        public ObservableCollection<ExamResultsDTO> ExamResults
        { 
            get { return _examResults; }
            set 
            { 
                _examResults = value; 
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

        public StudentExamsViewModel(Student student) 
        { 
            examService = App.ServiceProvider.GetService<IExamService>();
            courseService = App.ServiceProvider.GetService<ICourseService>();
            PageNumber = 1;

            _student = student;
            _results = examService.GetExamResultsDTO(student.FinishedExamResults);
            ExamResults = new ObservableCollection<ExamResultsDTO>(GetSlicedExamResults());

            ExamFilterVM = new ExamFilterViewModel(this);
            ExamSortingVM = new ExamSortingViewModel(this);
            CancelExamFiltersVM = new CancelExamFiltersViewModel(ExamFilterVM);
        }

        private List<ExamResultsDTO> GetSlicedExamResults()
        { 
            int elementsToSkip = (PageNumber - 1) * 6;
            return _results.Skip(elementsToSkip).Take(6).ToList();
        }

        public void FilterList(string languageNameFilter, string languageLevelFilter)
        { 
            List<Exam> filteredExams = examService.GetAllFilteredExams(GetExams(), languageNameFilter, languageLevelFilter);
            var filteredIds = filteredExams.Select(exam => exam.Id).ToHashSet();
            _results = GetExamResults().Where(result => filteredIds.Contains(result.ExamId)).ToList();
            UpdateExamList();
        }

        public void SortList(string examDateSorting)
        {
            List<int> examsIds = _results.Select(result => result.ExamId).ToList();
            List<Exam> exams = examService.GetAllExamsById(examsIds);
            List<Exam> sortedExams = examService.SortExams(exams, examDateSorting);
            var examResultsToDict = _results.ToDictionary(result => result.ExamId);
            _results = sortedExams.Select(exam => examResultsToDict[exam.Id]).ToList();
        }

        private List<Exam> GetExams()
        {
            List<int> examIds = _student.FinishedExamResults.Select(fc => fc.ExamId).ToList();
            return examService.GetAllExamsById(examIds);
        }

        private List<ExamResultsDTO> GetExamResults()
        { 
            return examService.GetExamResultsDTO(_student.FinishedExamResults);
        }

        public void UpdateExamList()
        {
            ExamResults.Clear();
            foreach(ExamResultsDTO examResult in GetSlicedExamResults())
            { 
                ExamResults.Add(examResult);
            }
        }

        private bool CanNextPage(object? parameter) { return PageNumber < (double)_examResults.Count / 6; }
        private void NextPage(object? parameter)
        {
            PageNumber++;
            ExamResults.Clear();
            foreach (var course in GetSlicedExamResults())
            {
                ExamResults.Add(course);
            }
        }

        private bool CanPreviousPage(object? parameter) { return PageNumber > 1; }
        private void PreviousPage(object? parameter)
        {
            PageNumber--;
            ExamResults.Clear();
            foreach (var course in GetSlicedExamResults())
            {
                ExamResults.Add(course);
            }
        }
    }
}
