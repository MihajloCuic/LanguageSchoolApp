using LanguageSchoolApp.core;
using LanguageSchoolApp.exceptions.Courses;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.view;
using LanguageSchoolApp.view.Courses;
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
        private readonly ICourseApplicationService courseApplicationService;
        private Student _student;
        public CourseFilterViewModel CourseFilterVM { get; }
        public CourseSortingViewModel CourseSortingVM { get; }
        public CancelCourseFiltersViewModel CancelCourseFiltersVM { get; }

        private ObservableCollection<Course> _availableCourses;
        private List<Course> _allAvailableCourses;
        private int _pageNumber;
        private string _studentId;

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
        public RelayCommand<List<ClassPeriod>> ScheduleCommand { get; set; }
        public RelayCommand<object> PreviousPageCommand { get; set; }
        public RelayCommand<object> NextPageCommand { get; set; }


        public AvailableCoursesViewModel(Student student)
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            courseApplicationService = App.ServiceProvider.GetService<ICourseApplicationService>();
            PageNumber = 1;
            _student = student;
            StudentId = student.Email;
            _allAvailableCourses = courseService.GetAllAvailableCourses();
            AvailableCourses = new ObservableCollection<Course>(GetSlicedAvailableCourses());

            CourseFilterVM = new CourseFilterViewModel(this);
            CourseSortingVM = new CourseSortingViewModel(this);
            CancelCourseFiltersVM = new CancelCourseFiltersViewModel(CourseFilterVM);

            ApplyCommand = new RelayCommand<int>(Apply, CanApply);
            ScheduleCommand = new RelayCommand<List<ClassPeriod>>(SeeSchedule, CanSeeSchedule);
            PreviousPageCommand = new RelayCommand<object>(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand<object>(NextPage, CanNextPage);
        }

        public void SortList(string beginningDateSorting, string durationSorting)
        { 
            UpdateCourseList(courseService.SortCourses(_allAvailableCourses, beginningDateSorting, durationSorting));
        }

        public void FilterList(string languageNameFilter, string languageLevelFilter, string courseTypeFilter)
        {
            List<Course> courses = courseService.GetAllAvailableCourses();
            UpdateCourseList(courseService.GetAllFilteredCourses(courses, languageNameFilter, languageLevelFilter, courseTypeFilter));
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

        private bool CanApply(int courseId) { return _student.EnrolledCourseId == -1; }
        private async void Apply(int courseId) 
        {
            Course course = courseService.GetCourse(courseId);
            //TODO: Create Popup that tells user he applied or withdrew his course application
            try
            {
                int courseApplicationId = courseApplicationService.GenerateId(StudentId, courseId);
                if (courseApplicationService.CourseApplicationExists(courseApplicationId))
                {
                    courseApplicationService.DeleteCourseApplication(courseApplicationId);
                    PopupMessageView popupMessage = new PopupMessageView("SUCCESS", $"You successfully withdrew from {course.LanguageProficiency.LanguageName} {course.LanguageProficiency.LanguageLevel.ToString()} !");
                    popupMessage.Show();
                }
                else
                {
                    courseApplicationService.CreateCourseApplication(StudentId, courseId);
                    PopupMessageView popupMessage = new PopupMessageView("SUCCESS", $"You successfully signed up for {course.LanguageProficiency.LanguageName} {course.LanguageProficiency.LanguageLevel.ToString()} !");
                    popupMessage.Show();
                }
                UpdateCourseList(_allAvailableCourses); //update button content
            }
            catch (CourseApplicationException ex)
            {
                PopupMessageView popupMessage = new PopupMessageView("ERROR", ex.Text);
                popupMessage.Show();
            }
        }

        private bool CanSeeSchedule(List<ClassPeriod> classes) { return true; }
        private void SeeSchedule(List<ClassPeriod> classes) 
        { 
            CourseSchedule schedule = new CourseSchedule(classes);
            schedule.Show();
        }

        private bool CanNextPage(object? parameter) { return PageNumber < (double)_allAvailableCourses.Count / 6; }
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
