using LanguageSchoolApp.core;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.model.Courses;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Users.Teachers;
using LanguageSchoolApp.view.Courses;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class TeacherCoursesViewModel : ObservableObject
    {
        private readonly ICourseService courseService;
        private readonly ITeacherService teacherService;
        public CourseFilterViewModel FilterCoursesVM { get; }
        public CourseSortingViewModel SortCoursesVM { get; }
        public CancelCourseFiltersViewModel CancelFiltersVM { get; }

        private Teacher _teacher;
        private ObservableCollection<Course> _teacherCourses;
        private List<Course> _allTeachersCourses;
        private int _pageNumber;

        public ObservableCollection<Course> TeacherCourses
        { 
            get { return _teacherCourses; }
            set 
            { 
                _teacherCourses = value;
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

        public Action<int> SwitchToActiveCourseView { get; set; }
        public RelayCommand<int> EditCommand { get; set; }
        public RelayCommand<List<ClassPeriod>> ScheduleCommand { get; set; }
        public RelayCommand<object> PreviousPageCommand { get; set; }
        public RelayCommand<object> NextPageCommand { get; set; }

        public TeacherCoursesViewModel(string teacherId) 
        { 
            courseService = App.ServiceProvider.GetService<ICourseService>();
            teacherService = App.ServiceProvider.GetService<ITeacherService>();
            FilterCoursesVM = new CourseFilterViewModel(this);
            SortCoursesVM = new CourseSortingViewModel(this);
            CancelFiltersVM = new CancelCourseFiltersViewModel(FilterCoursesVM);

            _teacher = teacherService.GetTeacher(teacherId);
            PageNumber = 1;
            _allTeachersCourses = courseService.GetAllCoursesById(_teacher.MyCoursesIds);
            TeacherCourses = new ObservableCollection<Course>(GetSlicedAvailableCourses());

            EditCommand = new RelayCommand<int>(DisplayEdit, CanDisplayEdit);
            ScheduleCommand = new RelayCommand<List<ClassPeriod>>(DisplaySchedule, CanDisplaySchedule);
            PreviousPageCommand = new RelayCommand<object>(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand<object>(NextPage, CanNextPage);

        }

        public void FilterList(string languageNameFilter, string languageLevelFilter, string courseTypeFilter)
        {
            List<Course> courses = courseService.GetAllCoursesById(_teacher.MyCoursesIds);
            UpdateCourseList(courseService.GetAllFilteredCourses(courses, languageNameFilter, languageLevelFilter, courseTypeFilter));
        }

        public void SortList(string beginningDateSorting, string durationSorting)
        {
            UpdateCourseList(courseService.SortCourses(_allTeachersCourses, beginningDateSorting, durationSorting));
        }

        public void UpdateCourseList(List<Course> courseList)
        {
            _allTeachersCourses = courseList;
            TeacherCourses.Clear();
            foreach (var course in GetSlicedAvailableCourses())
            {
                TeacherCourses.Add(course);
            }
        }

        private List<Course> GetSlicedAvailableCourses()
        {
            int elementsToSkip = (PageNumber - 1) * 6;
            return _allTeachersCourses.Skip(elementsToSkip).Take(6).ToList();
        }

        private bool CanDisplayEdit(int courseId) { return true; }
        private void DisplayEdit(int courseId)
        {
            Course course = courseService.GetCourse(courseId);
            if ((course.BeginningDate - DateTime.Now).TotalDays <= 7)
            {
                SwitchToActiveCourseView(courseId);
            }
            else 
            {
                //TODO: Implement edit options for pending course
            }

        }

        private bool CanDisplaySchedule(List<ClassPeriod> classes) { return true; }
        private void DisplaySchedule(List<ClassPeriod> classes) 
        { 
            CourseSchedule schedule = new CourseSchedule(classes);
            schedule.Show();
        }

        private bool CanNextPage(object? parameter) { return PageNumber < (double)_allTeachersCourses.Count / 6; }
        private void NextPage(object? parameter)
        {
            PageNumber++;
            TeacherCourses.Clear();
            foreach (var course in GetSlicedAvailableCourses())
            {
                TeacherCourses.Add(course);
            }
        }

        private bool CanPreviousPage(object? parameter) { return PageNumber > 1; }
        private void PreviousPage(object? parameter)
        {
            PageNumber--;
            TeacherCourses.Clear();
            foreach (var course in GetSlicedAvailableCourses())
            {
                TeacherCourses.Add(course);
            }
        }
    }
}
