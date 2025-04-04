using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.service.Courses;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class AvailableCoursesViewModel : ObservableObject
    {
        private readonly ICourseService courseService;
        public CourseFilterViewModel CourseFilterVM { get; }
        public CourseSortingViewModel CourseSortingVM { get; }
        public CancelCourseFiltersViewModel CancelCourseFiltersVM { get; }

        private ObservableCollection<Course> _availableCourses;
        private List<Course> _allAvailableCourses;
        private int _pageNumber;

        public ObservableCollection<Course> AvailableCourses
        {
            get { return _availableCourses; }
            set
            {
                _availableCourses = value;
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

        public RelayCommand<object> ApplyCommand { get; set; }
        public RelayCommand<object> ScheduleCommand { get; set; }
        public RelayCommand<object> PreviousPageCommand { get; set; }
        public RelayCommand<object> NextPageCommand { get; set; }


        public AvailableCoursesViewModel()
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            PageNumber = 1;
            _allAvailableCourses = courseService.GetAllAvailableCourses();
            AvailableCourses = new ObservableCollection<Course>(GetSlicedAvailableCourses());

            CourseFilterVM = new CourseFilterViewModel(this);
            CourseSortingVM = new CourseSortingViewModel(this);
            CancelCourseFiltersVM = new CancelCourseFiltersViewModel(CourseFilterVM);


            ApplyCommand = new RelayCommand<object>(Apply, CanApply);
            ScheduleCommand = new RelayCommand<object>(SeeSchedule, CanSeeSchedule);
            PreviousPageCommand = new RelayCommand<object>(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand<object>(NextPage, CanNextPage);
        }

        public void SortList(string beginningDateSorting, string durationSorting)
        { 
            UpdateCourseList(courseService.SortCourses(_allAvailableCourses, beginningDateSorting, durationSorting));
        }

        public void FilterList(string languageNameFilter, string languageLevelFilter, string courseTypeFilter)
        { 
            UpdateCourseList(courseService.GetAllFilteredCourses(_allAvailableCourses, languageNameFilter, languageLevelFilter, courseTypeFilter));
        }

        public void UpdateCourseList(List<Course> courseList)
        {
            _allAvailableCourses = courseList;
            AvailableCourses.Clear();
            foreach (var course in GetSlicedAvailableCourses())
            {
                AvailableCourses.Add(course);
            }
        }

        private List<Course> GetSlicedAvailableCourses() 
        { 
            int elementsToSkip = (PageNumber - 1) * 6;
            return _allAvailableCourses.Skip(elementsToSkip).Take(6).ToList();
        }

        private bool CanApply(object? parameter) { return true; }
        private void Apply(object? parameter) 
        { 
            //TODO: Create Course Application and Cancel Application
        }

        private bool CanSeeSchedule(object? parameter) { return true; }
        private void SeeSchedule(object? parameter) 
        { 
            //TODO: Create Schedule display
        }

        private bool CanNextPage(object? parameter) { return PageNumber < _allAvailableCourses.Count / 6; }
        private void NextPage(object? parameter) 
        {
            PageNumber++;
            AvailableCourses.Clear();
            foreach (var course in GetSlicedAvailableCourses()) 
            { 
                AvailableCourses.Add(course);
            }
        }

        private bool CanPreviousPage(object? parameter) { return PageNumber > 1; }
        private void PreviousPage(object? parameter) 
        { 
            PageNumber--;
            AvailableCourses.Clear();
            foreach (var course in GetSlicedAvailableCourses())
            {
                AvailableCourses.Add(course);
            }
        }
    }
}
