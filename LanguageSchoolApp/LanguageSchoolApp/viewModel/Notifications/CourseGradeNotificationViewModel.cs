using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Courses;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Documents;
using LanguageSchoolApp.service.Users.Directors;
using LanguageSchoolApp.service.Helpers;
using LanguageSchoolApp.view;
using LanguageSchoolApp.service.Users.Students;

namespace LanguageSchoolApp.viewModel.Notifications
{
    public class CourseGradeNotificationViewModel : ObservableObject
    {
        private readonly ICourseService courseService;
        private readonly IDirectorService directorService;
        private readonly IEmailService emailService;
        private readonly IStudentService studentService;
        private List<Course> _allFinishedCourses;
        private ObservableCollection<Course> _finishedCourses;
        private int _pageNumber;

        public ObservableCollection<Course> FinishedCourses
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

        public RelayCommand<int> ApplyCommand { get; set; }
        public RelayCommand<object> PreviousPageCommand { get; set; }
        public RelayCommand<object> NextPageCommand { get; set; }

        public CourseGradeNotificationViewModel(Director director) 
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            directorService = App.ServiceProvider.GetService<IDirectorService>();
            emailService = App.ServiceProvider.GetService<IEmailService>();
            studentService = App.ServiceProvider.GetService<IStudentService>();
            PageNumber = 1;
            _allFinishedCourses = courseService.GetAllCoursesById(director.FinishedCoursesIds);
            FinishedCourses = new ObservableCollection<Course>(GetSlicedFinishedCourses());
            ApplyCommand = new RelayCommand<int>(SendMessages, CanSendMessages);
            PreviousPageCommand = new RelayCommand<object>(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand<object>(NextPage, CanNextPage);
        }

        private List<Course> GetSlicedFinishedCourses()
        {
            int elementsToSkip = (PageNumber - 1) * 6;
            return _allFinishedCourses.Skip(elementsToSkip).Take(6).ToList();
        }

        private void UpdateCourseList(List<Course> courseList) 
        {
            _allFinishedCourses = courseList;
            FinishedCourses.Clear();
            foreach (var course in GetSlicedFinishedCourses())
            {
                FinishedCourses.Add(course);
            }
        }

        private bool CanSendMessages(int courseId) { return true; }
        private void SendMessages(int courseId)
        {
            try
            {
                Course course = courseService.GetCourse(courseId);
                List<Student> students = studentService.GetAllStudentsByIds(course.ParticipantsIds);
                int top10 = course.ParticipantsIds.Count == 0 ? 0 : Math.Max(1, (int)Math.Round(course.ParticipantsIds.Count * 0.1));
                List<Student> selectedStudents = studentService.SortStudentsByGrades(students, courseId, top10);
                emailService.SendCourseResults(selectedStudents, course);
                List<Course> finishedCourses = courseService.GetAllCoursesById(directorService.RemoveFinishedCourse(courseId));
                UpdateCourseList(finishedCourses);

                PopupMessageView successMessage = new PopupMessageView("SUCCESS", "Emails sent successfully !");
                successMessage.Show();
            }
            catch (Exception ex) 
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", ex.Message);
                errorPopup.Show();
            }
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
