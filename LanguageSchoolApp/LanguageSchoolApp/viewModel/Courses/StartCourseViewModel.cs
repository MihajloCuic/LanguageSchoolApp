using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Users.Students;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.view;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class StartCourseViewModel : ObservableObject
    {
        private readonly IStudentService studentService;
        private readonly ICourseApplicationService courseApplicationService;
        private readonly ICourseService courseService;
        private readonly ICourseDropoutRequestService courseDropoutRequestService;
        private Student? selectedStudent;
        private readonly Course course;
        private readonly string teacherId;

        private List<Student> allStudents;
        private List<CourseApplication> courseApplications;
        private List<FinishedCourse> _allFinishedCourses;
        private ObservableCollection<Student> _appliedStudents;
        private ObservableCollection<FinishedCourse> _finishedCourses;
        private string _selectedName;
        private string _selectedSurname;
        private int _selectedPenaltyPoints;
        private int _selectedDropoutCount;
        private string _reason;
        private int _placesLeft;

        public ObservableCollection<Student> AppliedStudents
        { 
            get { return _appliedStudents; }
            set 
            { 
                _appliedStudents = value; 
                OnPropertyChanged();
            }
        }
        public ObservableCollection<FinishedCourse> FinishedCourses
        { 
            get { return _finishedCourses; }
            set
            { 
                _finishedCourses = value;
                OnPropertyChanged();
            }
        }
        public string SelectedName 
        { 
            get { return _selectedName; }
            set
            { 
                _selectedName = value;
                OnPropertyChanged();
            }
        }
        public string SelectedSurname 
        { 
            get { return _selectedSurname; }
            set
            { 
                _selectedSurname = value;
                OnPropertyChanged();
            }
        }
        public int SelectedPenaltyPoints 
        { 
            get { return _selectedPenaltyPoints; }
            set
            { 
                _selectedPenaltyPoints = value;
                OnPropertyChanged();
            }
        }
        public int SelectedDropoutCount 
        {  
            get { return _selectedDropoutCount; }
            set
            { 
                _selectedDropoutCount = value;
                OnPropertyChanged();
            }
        }
        public string Reason 
        { 
            get { return _reason; }
            set
            { 
                _reason = value; 
                OnPropertyChanged();
            }
        }
        public int PlacesLeft
        { 
            get { return _placesLeft; }
            set
            { 
                _placesLeft = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<string> SelectStudentCommand {  get; set; }
        public RelayCommand<object> AcceptCommand { get; set; }
        public RelayCommand<object> RejectCommand { get; set; }
        public RelayCommand<object> StartCourseCommand { get; set; }
        public Action<string> SwitchToTeacherCourses { get; set; }

        public StartCourseViewModel(int _courseId, string _teacherId) 
        { 
            studentService = App.ServiceProvider.GetService<IStudentService>();
            courseApplicationService = App.ServiceProvider.GetService<ICourseApplicationService>();
            courseService = App.ServiceProvider.GetService<ICourseService>();
            courseDropoutRequestService = App.ServiceProvider.GetService<ICourseDropoutRequestService>();

            course = courseService.GetCourse(_courseId);
            teacherId = _teacherId;
            courseApplications = courseApplicationService.GetAllCourseApplicationsByCourseId(course.Id);
            allStudents = studentService.GetAllStudentsByIds(courseApplications.Select(application => application.StudentId).ToList());
            AppliedStudents = new ObservableCollection<Student>(allStudents);
            FinishedCourses = new ObservableCollection<FinishedCourse>();

            PlacesLeft = course.MaxParticipants - course.ParticipantsIds.Count;

            SelectStudentCommand = new RelayCommand<string>(SelectStudent, CanSelectStudent);
            AcceptCommand = new RelayCommand<object>(Accept, CanAccept);
            RejectCommand = new RelayCommand<object>(Reject, CanReject);
            StartCourseCommand = new RelayCommand<object>(StartCourse, CanStartCourse);
        }

        private bool CanSelectStudent(string studentId) { return true; }
        private void SelectStudent(string studentId) 
        {
            selectedStudent = studentService.GetStudent(studentId);
            SelectedName = selectedStudent.Name;
            SelectedSurname = selectedStudent.Surname;
            SelectedPenaltyPoints = selectedStudent.PenaltyPoints.Count;
            SelectedDropoutCount = courseDropoutRequestService.GetAllDropoutRequestsByStudentId(studentId).Count;
            _allFinishedCourses = selectedStudent.FinishedCourses;
            SetFinishedCourses();
        }

        private void UnselectStudent()
        {
            selectedStudent = null;
            SelectedName = "";
            SelectedSurname = "";
            SelectedPenaltyPoints = 0;
            SelectedDropoutCount = 0;
            _allFinishedCourses.Clear();
            SetFinishedCourses();
        }

        private void SetFinishedCourses() 
        { 
            FinishedCourses.Clear();
            if (_allFinishedCourses == null || _allFinishedCourses.Count == 0)
            {
                return;
            }

            foreach(FinishedCourse finishedCourse in _allFinishedCourses)
            { 
                FinishedCourses.Add(finishedCourse);
            }
        }

        private void SetAppliedStudents()
        {
            AppliedStudents.Clear();
            if (allStudents == null || allStudents.Count == 0)
            {
                return;
            }
            foreach (Student student in allStudents)
            {
                AppliedStudents.Add(student);
            }
        }

        private bool CanAccept(object? parameter) { return true; }
        private void Accept(object? parameter)
        {
            if (PlacesLeft <= 0)
            {
                PopupMessageView errorMessage = new PopupMessageView("ERROR", "There are no empty places left ! Start course !");
                errorMessage.Show();
                return;
            }
            if (selectedStudent == null)
            {
                PopupMessageView errorMessage = new PopupMessageView("ERROR", "Select student first !");
                errorMessage.Show();
                return;
            }

            studentService.EnrollStudentToCourse(selectedStudent.Email, course.Id);
            int applicationId = courseApplicationService.GenerateId(selectedStudent.Email, course.Id);
            courseApplicationService.AcceptCourseApplication(applicationId);
            PlacesLeft--;
            allStudents.Remove(selectedStudent);
            SetAppliedStudents();
            UnselectStudent();
        }

        private bool CanReject(object? parameter) { return true; }
        private void Reject(object? parameter) 
        {
            if (selectedStudent == null)
            {
                PopupMessageView errorMessage = new PopupMessageView("ERROR", "Select student first !");
                errorMessage.Show();
                return;
            }

            if (string.IsNullOrEmpty(Reason)) 
            {
                PopupMessageView errorMessage = new PopupMessageView("ERROR", "You must write reason for rejection !");
                errorMessage.Show();
                return;
            }

            //TODO: Send student notification he was rejected and why
            int applicationId = courseApplicationService.GenerateId(selectedStudent.Email, course.Id);
            courseApplicationService.DenyCourseApplication(applicationId);
            allStudents.Remove(selectedStudent);
            SetAppliedStudents();
            UnselectStudent();
        }

        private bool CanStartCourse(object? parameter) { return true; }
        private void StartCourse(object? parameter) 
        {
            if (allStudents.Count > 0 && PlacesLeft > 0)
            {
                PopupMessageView errorMessage = new PopupMessageView("ERROR", "Please handle all students applications before starting course !");
                errorMessage.Show();
                return;
            }

            if (PlacesLeft == 0 && allStudents.Count > 0) 
            {
                Reason = "All places are filled !";
                foreach (Student student in allStudents) 
                { 
                    selectedStudent = student;
                    Reject(null);
                }
            }

            PopupMessageView successMessage = new PopupMessageView("SUCCESS", $"All student applications handled. {course.LanguageProficiency.LanguageName} {course.LanguageProficiency.LanguageLevel.ToString()} begins on: {course.BeginningDate.ToString("dd.MM.yyyy.")} !");
            successMessage.Show();
            SwitchToTeacherCourses(teacherId);
        }
    }
}
