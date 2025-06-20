﻿using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Notifications;
using System.Collections.ObjectModel;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Notifications;
using Microsoft.Extensions.DependencyInjection;
using LanguageSchoolApp.viewModel.Courses;
using LanguageSchoolApp.viewModel.Exams;
using LanguageSchoolApp.viewModel.Users;
using LanguageSchoolApp.viewModel.Reports;
using LanguageSchoolApp.view.Users;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.service.Users.Teachers;
using LanguageSchoolApp.viewModel.Notifications;

namespace LanguageSchoolApp.viewModel
{
    public class MainViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;

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

        public RelayCommand<object> ChangeToMenuItem1Command { get; set; }
        public RelayCommand<object> ChangeToMenuItem2Command { get; set; }
        public RelayCommand<object> ChangeToMenuItem3Command { get; set; }
        public RelayCommand<object> ChangeToMenuItem4Command { get; set; }
        public RelayCommand<object> ChangeToMenuItem5Command { get; set; }
        public RelayCommand<object> ChangeToMenuItem6Command { get; set; }

        //student menu options
        public AvailableCoursesViewModel AvailableCoursesVM { get; set; }
        public ActiveCourseViewModel ActiveCourseVM { get; set; }
        public AvailableExamsViewModel AvailableExamsVM { get; set; }
        public StudentExamsViewModel StudentExamsVM { get; set; }
        public FinishedCoursesViewModel FinishedCoursesVM { get; set; }

        //teacher menu options
        public TeacherCoursesViewModel TeacherCoursesVM { get; set; }
        public StartCourseViewModel StartCourseVM { get; set; }
        public CreateCourseViewModel CreateCourseVM { get; set; }
        public TeacherExamsViewModel TeacherExamsVM { get; set; }
        public CreateExamViewModel CreateExamVM { get; set; }
        public GradeExamsViewModel GradeExamsVM { get; set; }
        public StartExamViewModel StartExamVM { get; set; } 

        //director menu options
        public ActiveTeachersViewModel ActiveTeachersVM { get; set; }
        public CreateTeacherViewModel CreateTeacherVM { get; set; }
        public ExamGradeNotificationViewModel SendExamResultsVM { get; set; }
        public CourseGradeNotificationViewModel SendCourseResultsVM { get; set; }
        public ReportsViewModel ReportsVM { get; set; }

        public RelayCommand<object> LogoutCommand { get; set; }
        public RelayCommand<object> EditUserProfileCommand { get; set; }
        public Action CloseAction { get; set; }

        public MainViewModel(User currentUser) 
        {
            _currentUser = currentUser;
            _username = currentUser.Name;

            _notificationService = App.ServiceProvider.GetService<INotificationService>();
            _studentService = App.ServiceProvider.GetService<IStudentService>();
            _teacherService = App.ServiceProvider.GetService<ITeacherService>();
            List<Notification> unreadNotificationsList = _notificationService.GetUnreadNotificationsByReceiver(CurrentUser.Email);
            UnreadNotifications = new ObservableCollection<Notification>(unreadNotificationsList);
            UnreadNotificationsExist = unreadNotificationsList.Count > 0;

            if (currentUser is Student student)
            {
                AvailableCoursesVM = new AvailableCoursesViewModel(student);
                if (student.EnrolledCourseId != -1)
                {
                    ActiveCourseVM = new ActiveCourseViewModel(student);
                }
                AvailableExamsVM = new AvailableExamsViewModel(student);
                StudentExamsVM = new StudentExamsViewModel(student);
                FinishedCoursesVM = new FinishedCoursesViewModel(student);
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
                TeacherCoursesVM = new TeacherCoursesViewModel(teacher.Email);
                TeacherCoursesVM.SwitchToActiveCourseView = SwitchToActiveCourseView;
                TeacherCoursesVM.SwitchToEditCourseView = SwitchToEditCourseView;
                TeacherCoursesVM.SwitchToStartCourseView = SwitchToStartCourseView;
                CreateCourseVM = new CreateCourseViewModel(teacher);
                TeacherExamsVM = new TeacherExamsViewModel(teacher.Email);
                TeacherExamsVM.SwitchToEditExamView = SwitchToEditExamView;
                TeacherExamsVM.SwitchToFinishExamView = SwitchToFinishExamView;
                TeacherExamsVM.SwitchToStartExamView = SwitchToStartExamView;
                CreateExamVM = new CreateExamViewModel(teacher);
                CurrentView = TeacherCoursesVM;

                MenuItem1 = "My Courses";
                MenuItem2 = "Create Course";
                MenuItem3 = "My Exams";
                MenuItem4 = "Create Exam";
                MenuItem5 = "";
                MenuItem6 = "";
            }
            else if (currentUser is Director director) 
            {
                ActiveTeachersVM = new ActiveTeachersViewModel();
                ActiveTeachersVM.OpenEditWindowAction = EditTeacher;
                ActiveTeachersVM.OpenCreateTeacherView = OpenCreateTeacherView;
                CreateCourseVM = new CreateCourseViewModel();
                CreateExamVM = new CreateExamViewModel();
                SendExamResultsVM = new ExamGradeNotificationViewModel(director);
                SendCourseResultsVM = new CourseGradeNotificationViewModel(director);
                ReportsVM = new ReportsViewModel();
                CurrentView = ActiveTeachersVM;

                MenuItem1 = "Active Teachers";
                MenuItem2 = "Smart Course";
                MenuItem3 = "Smart Exam";
                MenuItem4 = "Send Exam Results";
                MenuItem5 = "Send Course Results";
                MenuItem6 = "Reports";
            }

            ChangeToMenuItem1Command = new RelayCommand<object>(ChangeToMenuItem1, CanChangeToMenuItem1);
            ChangeToMenuItem2Command = new RelayCommand<object>(ChangeToMenuItem2, CanChangeToMenuItem2);
            ChangeToMenuItem3Command = new RelayCommand<object>(ChangeToMenuItem3, CanChangeToMenuItem3);
            ChangeToMenuItem4Command = new RelayCommand<object>(ChangeToMenuItem4, CanChangeToMenuItem4);
            ChangeToMenuItem5Command = new RelayCommand<object>(ChangeToMenuItem5, CanChangeToMenuItem5);
            ChangeToMenuItem6Command = new RelayCommand<object>(ChangeToMenuItem6, CanChangeToMenuItem6);
            LogoutCommand = new RelayCommand<object>(Logout, CanLogout);
            EditUserProfileCommand = new RelayCommand<object>(EditUserProfile, CanEditUserProfile);
        }

        private void SwitchToActiveCourseView(int courseId)
        {
            if (CurrentUser is Teacher teacher)
            { 
                ActiveCourseVM = new ActiveCourseViewModel(teacher, courseId);
                ActiveCourseVM.SwitchToTeacherCourses = SwitchToTeacherCourses;
                CurrentView = ActiveCourseVM;
            }
        }
        private void SwitchToStartCourseView(int courseId)
        {
            if (CurrentUser is Teacher teacher)
            {
                StartCourseVM = new StartCourseViewModel(courseId, teacher.Email);
                StartCourseVM.SwitchToTeacherCourses = SwitchToTeacherCourses;
                CurrentView = StartCourseVM;
            }
        }
        private void SwitchToEditCourseView(int courseId)
        {
            if (CurrentUser is Teacher teacher)
            { 
                CreateCourseVM = new CreateCourseViewModel(teacher, courseId);
                CreateCourseVM.SwitchToTeacherCoursesView = SwitchToTeacherCourses;
                CurrentView = CreateCourseVM;
            }
        }
        private void SwitchToTeacherCourses(string teacherId)
        {
            TeacherCoursesVM = new TeacherCoursesViewModel(CurrentUser.Email);
            TeacherCoursesVM.SwitchToActiveCourseView = SwitchToActiveCourseView;
            TeacherCoursesVM.SwitchToEditCourseView = SwitchToEditCourseView;
            TeacherCoursesVM.SwitchToStartCourseView = SwitchToStartCourseView;
            CurrentView = TeacherCoursesVM;
        }

        private void SwitchToEditExamView(int examId)
        {
            if (CurrentUser is Teacher teacher) 
            { 
                CreateExamVM = new CreateExamViewModel(teacher, examId);
                CreateExamVM.SwitchToTeacherExams = SwitchToTeacherExams;
                CurrentView = CreateExamVM;
            }
        }
        private void SwitchToFinishExamView(int examId)
        {
            if (CurrentUser is Teacher teacher)
            {
                GradeExamsVM = new GradeExamsViewModel(examId);
                GradeExamsVM.SwitchToTeacherExamView = SwitchToTeacherExams;
                CurrentView = GradeExamsVM;
            }
        }
        private void SwitchToStartExamView(int examId)
        {
            if (CurrentUser is Teacher teacher)
            { 
                StartExamVM = new StartExamViewModel(examId);
                CurrentView = StartExamVM;
            }
        }
        private void SwitchToTeacherExams()
        { 
            TeacherExamsVM = new TeacherExamsViewModel(CurrentUser.Email);
            TeacherExamsVM.SwitchToEditExamView = SwitchToEditExamView;
            TeacherExamsVM.SwitchToFinishExamView = SwitchToFinishExamView;
            TeacherExamsVM.SwitchToStartExamView = SwitchToStartExamView;
            CurrentView = TeacherExamsVM;
        }

        private bool CanChangeToMenuItem1(object? parameter) 
        {
            if (CurrentUser is Student)
            {
                return !(CurrentView is AvailableCoursesViewModel);
            }
            if (CurrentUser is Teacher)
            {
                return !(CurrentView is TeacherCoursesViewModel);
            }
            if (CurrentUser is Director)
            {
                return !(CurrentView is ActiveTeachersViewModel);
            }
            return false;
        }
        private void ChangeToMenuItem1(object parameter) 
        {
            if (CurrentUser is Student student)
            {
                student = _studentService.GetStudent(student.Email);
                AvailableCoursesVM = new AvailableCoursesViewModel(student);
                CurrentView = AvailableCoursesVM;
            }
            if (CurrentUser is Teacher)
            {
                TeacherCoursesVM = new TeacherCoursesViewModel(CurrentUser.Email);
                TeacherCoursesVM.SwitchToActiveCourseView = SwitchToActiveCourseView;
                TeacherCoursesVM.SwitchToEditCourseView = SwitchToEditCourseView;
                CurrentView = TeacherCoursesVM;
            }
            if (CurrentUser is Director)
            {
                CurrentView = ActiveTeachersVM;
            }
        }

        private bool CanChangeToMenuItem2(object? parameter)
        {
            if (CurrentUser is Student student)
            {
                return !(CurrentView is ActiveCourseViewModel) && student.EnrolledCourseId != -1;
            }
            if (CurrentUser is Teacher)
            {
                return !(CurrentView is CreateCourseViewModel);
            }
            if (CurrentUser is Director)
            {
                return !(CurrentView is CreateCourseViewModel);
            }
            return false;
        }
        private void ChangeToMenuItem2(object parameter)
        {
            if (CurrentUser is Student student)
            {
                student = _studentService.GetStudent(student.Email);
                if (student.EnrolledCourseId == -1)
                {
                    return;
                }
                ActiveCourseVM = new ActiveCourseViewModel(student);
                CurrentView = ActiveCourseVM;
            }
            if (CurrentUser is Teacher teacher)
            {
                CreateCourseVM = new CreateCourseViewModel(teacher);
                CurrentView = CreateCourseVM;
            }
            if (CurrentUser is Director)
            {
                CurrentView = CreateCourseVM;
            }
        }

        private bool CanChangeToMenuItem3(object? parameter)
        {
            if (CurrentUser is Student)
            {
                return !(CurrentView is AvailableExamsViewModel);
            }
            if (CurrentUser is Teacher)
            {
                return !(CurrentView is TeacherExamsViewModel);
            }
            if (CurrentUser is Director)
            {
                return !(CurrentView is CreateExamViewModel);
            }
            return false;
        }
        private void ChangeToMenuItem3(object parameter)
        {
            if (CurrentUser is Student)
            {
                CurrentView = AvailableExamsVM;
            }
            if (CurrentUser is Teacher)
            {
                TeacherExamsVM = new TeacherExamsViewModel(CurrentUser.Email);
                TeacherExamsVM.SwitchToEditExamView = SwitchToEditExamView;
                TeacherExamsVM.SwitchToFinishExamView = SwitchToFinishExamView;
                TeacherExamsVM.SwitchToStartExamView = SwitchToStartExamView;
                CurrentView = TeacherExamsVM;
            }
            if (CurrentUser is Director)
            {
                CurrentView = CreateExamVM;
            }
        }

        private bool CanChangeToMenuItem4(object? parameter)
        {
            if (CurrentUser is Student)
            {
                return !(CurrentView is StudentExamsViewModel);
            }
            if (CurrentUser is Teacher)
            {
                return !(CurrentView is CreateExamViewModel);
            }
            if (CurrentUser is Director)
            {
                return !(CurrentView is ExamGradeNotificationViewModel);
            }
            return false;
        }
        private void ChangeToMenuItem4(object parameter)
        {
            if (CurrentUser is Student)
            {
                CurrentView = StudentExamsVM;
            }
            if (CurrentUser is Teacher)
            {
                CurrentView = CreateExamVM;
            }
            if (CurrentUser is Director)
            {
                CurrentView = SendExamResultsVM;
            }
        }

        private bool CanChangeToMenuItem5(object? parameter)
        {
            if (CurrentUser is Student)
            {
                return !(CurrentView is FinishedCoursesViewModel);
            }
            if (CurrentUser is Teacher)
            {
                return false;
            }
            if (CurrentUser is Director)
            {
                return !(CurrentView is CourseGradeNotificationViewModel);
            }
            return false;
        }
        private void ChangeToMenuItem5(object parameter)
        {
            if (CurrentUser is Student)
            {
                CurrentView = FinishedCoursesVM;
            }
            if (CurrentUser is Director)
            {
                CurrentView = SendCourseResultsVM;
            }
        }

        private bool CanChangeToMenuItem6(object? parameter)
        {
            if (CurrentUser is Student)
            {
                return false;
            }
            if (CurrentUser is Teacher)
            {
                return false;
            }
            if (CurrentUser is Director)
            {
                return !(CurrentView is ReportsViewModel);
            }
            return false;
        }
        private void ChangeToMenuItem6(object parameter)
        {
            if (CurrentUser is Director)
            {
                CurrentView = ReportsVM;
            }
        }

        private bool CanLogout(object? parameter) { return CurrentUser != null; }
        private void Logout(object? parameter)
        {
            Login login = new Login();
            login.Show();
            CloseAction();
        }

        private bool CanEditUserProfile(object? parameter) { return CurrentUser != null; }
        private void EditUserProfile(object? parameter)
        {
            if (CurrentUser is Student student)
            { 
                Register editProfile = new Register(student);
                editProfile.Show();
            }
            else if (CurrentUser is Teacher teacher) 
            { 
                Register editProfile = new Register(teacher);
                editProfile.Show();
            }
            else if (CurrentUser is Director director) 
            { 
                Register editProfile = new Register(director);
                editProfile.Show();
            }
            CloseAction();
        }

        private void EditTeacher(string teacherId)
        { 
            Register editTeacherView = new Register(_teacherService.GetTeacher(teacherId), (Director)CurrentUser);
            editTeacherView.Show();
            CloseAction();
        }

        private void OpenCreateTeacherView()
        {
            CreateTeacherVM = new CreateTeacherViewModel();
            CreateTeacherVM.CloseAction = CloseCreateTeacherView;
            CurrentView = CreateTeacherVM;
        }

        private void CloseCreateTeacherView()
        {
            if (CurrentUser is Director director)
            {
                ActiveTeachersVM = new ActiveTeachersViewModel();
                ActiveTeachersVM.OpenEditWindowAction = EditTeacher;
                ActiveTeachersVM.OpenCreateTeacherView = OpenCreateTeacherView;
                CurrentView = ActiveTeachersVM;
            }
        }
    }
}
