using LanguageSchoolApp.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Courses;
using System.Collections.ObjectModel;
using LanguageSchoolApp.service.Courses;
using Microsoft.Extensions.DependencyInjection;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class FinishedCoursesViewModel : ObservableObject
    {
        private readonly ICourseService courseService;
        private Student _student;
        public CourseFilterViewModel CourseFilterVM { get; }
        public CourseSortingViewModel CourseSortingVM { get; }
        public CancelCourseFiltersViewModel CancelCourseFiltersVM { get; }

        private ObservableCollection<FinishedCourseDTO> _finishedCourses;
        private List<FinishedCourseDTO> _allFinishedCourses;
        private int _pageNumber;

        public ObservableCollection<FinishedCourseDTO> FinishedCourses
        {
            get { return _finishedCourses; }
            set
            {
                _finishedCourses = value;
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

        public FinishedCoursesViewModel(Student currentStudent) 
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            PageNumber = 1;
            _student = currentStudent;
            _allFinishedCourses = GetFinishedCourses();
            FinishedCourses = new ObservableCollection<FinishedCourseDTO>(GetSlicedFinishedCourses());

            CourseFilterVM = new CourseFilterViewModel(this);
            CourseSortingVM = new CourseSortingViewModel(this);
            CancelCourseFiltersVM = new CancelCourseFiltersViewModel(CourseFilterVM);

            PreviousPageCommand = new RelayCommand<object>(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand<object>(NextPage, CanNextPage);
        }

        public void SortList(string beginningDateSorting, string durationSorting)
        {
            List<int> coursesIds = _allFinishedCourses.Select(fc => fc.CourseId).ToList();
            List<Course> courses = courseService.GetAllCoursesById(coursesIds);
            List<Course> sortedCourses = courseService.SortCourses(courses, beginningDateSorting, durationSorting);
            var finishedCoursesDict = _allFinishedCourses.ToDictionary(fc => fc.CourseId);
            _allFinishedCourses = sortedCourses.Select(course => finishedCoursesDict[course.Id]).ToList();
            UpdateCourseList();
        }

        public void FilterList(string languageNameFilter, string languageLevelFilter, string courseTypeFilter)
        {
            List<FinishedCourseDTO> finishedCourses = GetFinishedCourses();
            List<int> coursesIds = finishedCourses.Select(fc => fc.CourseId).ToList();
            List<Course> courses = courseService.GetAllCoursesById(coursesIds);
            List<Course> filteredCourses = courseService.GetAllFilteredCourses(courses, languageNameFilter, languageLevelFilter, courseTypeFilter);
            var filteredIds = filteredCourses.Select(c => c.Id).ToHashSet();
            _allFinishedCourses = finishedCourses.Where(fc => filteredIds.Contains(fc.CourseId)).ToList();
            UpdateCourseList();
        }

        private List<FinishedCourseDTO> GetFinishedCourses()
        { 
            return courseService.GetFinishedCoursesDTO(_student.FinishedCourses);
        }

        public void UpdateCourseList() 
        {
            FinishedCourses.Clear();
            foreach (var course in GetSlicedFinishedCourses())
            {
                FinishedCourses.Add(course);
            }
        }

        private List<FinishedCourseDTO> GetSlicedFinishedCourses()
        {
            int elementsToSkip = (PageNumber - 1) * 6;
            return _allFinishedCourses.Skip(elementsToSkip).Take(6).ToList();
        }

        private bool CanNextPage(object? parameter) { return PageNumber < (double)_allFinishedCourses.Count / 6; }
        private void NextPage(object? parameter)
        {
            PageNumber++;
            FinishedCourses.Clear();
            foreach (var course in GetSlicedFinishedCourses())
            {
                FinishedCourses.Add(course);
            }
        }

        private bool CanPreviousPage(object? parameter) { return PageNumber > 1; }
        private void PreviousPage(object? parameter)
        {
            PageNumber--;
            FinishedCourses.Clear();
            foreach (var course in GetSlicedFinishedCourses())
            {
                FinishedCourses.Add(course);
            }
        }
    }
}
