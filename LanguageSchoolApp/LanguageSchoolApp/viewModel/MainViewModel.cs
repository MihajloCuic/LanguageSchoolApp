using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Notifications;
using Microsoft.Extensions.DependencyInjection;
using LanguageSchoolApp.viewModel.Courses;
using LanguageSchoolApp.viewModel.Exams;
using LanguageSchoolApp.viewModel.Users;
using LanguageSchoolApp.viewModel.Reports;

namespace LanguageSchoolApp.viewModel
{
    public class MainViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;

        private object _currentView;
        private User _currentUser;
        private string _username;
        private ObservableCollection<Notification> _unreadNotifications;
        private bool _unreadNotificationsExist;

        public User CurrentUser 
        {
            get { return _currentUser; }
            set
            { 
                _currentUser = value;
                OnPropertyChanged();
            }
        }
        public object CurrentView
        { 
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public string Username
        {
            get { return _username; }
            set 
            { 
                _username = value; 
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Notification> UnreadNotifications
        { 
            get { return _unreadNotifications; }
            set
            { 
                _unreadNotifications = value;
                OnPropertyChanged();
            }
        }
        public bool UnreadNotificationsExist
        { 
            get { return _unreadNotificationsExist; }
            set 
            { 
                _unreadNotificationsExist = value;
                OnPropertyChanged();
            }
        }

        public string MenuItem1 { get; set; }
        public string MenuItem2 { get; set; }
        public string MenuItem3 { get; set; }
        public string MenuItem4 { get; set; }
        public string MenuItem5 { get; set; }
        public string MenuItem6 { get; set; }

        //student menu options
        public AvailableCoursesViewModel AvailableCoursesVM { get; set; }
        public ActiveCourseViewModel ActiveCourseVM { get; set; }
        public AvailableExamsViewModel AvailableExamsVM { get; set; }
        public StudentExamsViewModel StudentExamsVM { get; set; }
        public FinishedCoursesViewModel FinishedCoursesVM { get; set; }

        //teacher menu options
        public TeacherCoursesViewModel TeacherCoursesVM { get; set; }
        public PendingCoursesViewModel PendingCoursesVM { get; set; }
        public ActiveCourseViewModel ActiveCoursesVM { get; set; }
        public TeacherExamsViewModel TeacherExamsVM { get; set; }
        public PendingExamsViewModel PendingExamsVM { get; set; }
        public ActiveExamViewModel ActiveExamVM { get; set; }

        //director menu options
        public ActiveTeachersViewModel ActiveTeachersVM { get; set; }
        public SmartCourseMakingViewModel SmartCourseMakingVM { get; set; }
        public SmartExamMakingViewModel SmartExamMakingVM { get; set; }
        public SendExamResultsViewModel SendExamResultsVM { get; set; }
        public SendCourseResultsViewModel SendCourseResultsVM { get; set; }
        public ReportsViewModel ReportsVM { get; set; }

        public MainViewModel(User currentUser) 
        {
            _currentUser = currentUser;
            _username = currentUser.Name;

            _notificationService = App.ServiceProvider.GetService<INotificationService>();
            List<Notification> unreadNotificationsList = _notificationService.GetUnreadNotificationsByReceiver(CurrentUser.Email);
            UnreadNotifications = new ObservableCollection<Notification>(unreadNotificationsList);
            UnreadNotificationsExist = unreadNotificationsList.Count > 0;

            if (currentUser is Student student)
            {
                AvailableCoursesVM = new AvailableCoursesViewModel();
                ActiveCourseVM = new ActiveCourseViewModel();
                AvailableExamsVM = new AvailableExamsViewModel();
                StudentExamsVM = new StudentExamsViewModel();
                FinishedCoursesVM = new FinishedCoursesViewModel();
                CurrentView = AvailableCoursesVM;

                MenuItem1 = "Available Courses";
                MenuItem2 = "Active Course";
                MenuItem3 = "Available Exams";
                MenuItem4 = "My Exams";
                MenuItem5 = "FinishedCourses";
                MenuItem6 = "";
            }
            else if (currentUser is Teacher teacher)
            {
                TeacherCoursesVM = new TeacherCoursesViewModel();
                PendingCoursesVM = new PendingCoursesViewModel();
                ActiveCoursesVM = new ActiveCourseViewModel();
                TeacherExamsVM = new TeacherExamsViewModel();
                PendingExamsVM = new PendingExamsViewModel();
                ActiveExamVM = new ActiveExamViewModel();
                CurrentView = TeacherCoursesVM;

                MenuItem1 = "My Courses";
                MenuItem2 = "Pending Courses";
                MenuItem3 = "Active Courses";
                MenuItem4 = "My Exams";
                MenuItem5 = "Pending Exams";
                MenuItem6 = "Active Exam";
            }
            else if (currentUser is Director director) 
            {
                ActiveTeachersVM = new ActiveTeachersViewModel();
                SmartCourseMakingVM = new SmartCourseMakingViewModel();
                SmartExamMakingVM = new SmartExamMakingViewModel();
                SendExamResultsVM = new SendExamResultsViewModel();
                SendCourseResultsVM = new SendCourseResultsViewModel();
                ReportsVM = new ReportsViewModel();
                CurrentView = ActiveTeachersVM;

                MenuItem1 = "Active Teachers";
                MenuItem2 = "Smart Course Making";
                MenuItem3 = "Smart Exam Making";
                MenuItem4 = "Send Exam Results";
                MenuItem5 = "Send Course Results";
                MenuItem6 = "Reports";
            }
        }
    }
}
