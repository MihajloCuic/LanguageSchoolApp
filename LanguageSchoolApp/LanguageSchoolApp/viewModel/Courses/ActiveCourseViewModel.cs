using Microsoft.Extensions.DependencyInjection;
using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.service.Users.Teachers;
using System.Collections.ObjectModel;
using LanguageSchoolApp.service.Users.PenaltyPoints;
using LanguageSchoolApp.model;
using LanguageSchoolApp.view.Courses;
using LanguageSchoolApp.view;
using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.service.Users.Students;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class ActiveCourseViewModel : ObservableObject
    {
        private readonly ICourseService courseService;
        private readonly ITeacherService teacherService;
        private readonly IPenaltyPointService penaltyPointService;
        private readonly IStudentService studentService;
        private readonly Course course;
        private readonly Teacher teacher;
        private readonly Student student;

        private string _languageProficiency;
        private DateTime _beginningDate;
        private string _duration;
        private string _courseType;

        private string _teacherName;
        private string _teacherSurname;
        private double _averageGrade;
        private string _teachersGrade;
        private int _givenGrade;

        private List<ClassPeriod> _allClasses;
        private ObservableCollection<ClassPeriod> _mondayClasses;
        private ObservableCollection<ClassPeriod> _tuesdayClasses;
        private ObservableCollection<ClassPeriod> _wednesdayClasses;
        private ObservableCollection<ClassPeriod> _thursdayClasses;
        private ObservableCollection<ClassPeriod> _fridayClasses;
        private ObservableCollection<ClassPeriod> _saturdayClasses;
        private ObservableCollection<ClassPeriod> _sundayClasses;

        private ObservableCollection<PenaltyPoint> _penaltyPoints;

        public string LanguageProficiency
        { 
            get { return _languageProficiency; }
            set
            {
                _languageProficiency = value;
                OnPropertyChanged();
            }
        }
        public DateTime BeginningDate
        {
            get { return _beginningDate; }
            set
            { 
                _beginningDate = value;
                OnPropertyChanged();
            }
        }
        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }
        public string CourseType
        {
            get { return _courseType; }
            set 
            { 
                _courseType = value;
                OnPropertyChanged();
            }
        }
        public string TeacherName
        {
            get { return _teacherName; }
            set
            {  
                _teacherName = value;
                OnPropertyChanged();
            }
        }
        public string TeacherSurname
        { 
            get { return _teacherSurname; }
            set
            {
                _teacherSurname = value;
                OnPropertyChanged();
            }
        }
        public double AverageGrade
        {
            get { return _averageGrade; }
            set
            {
                _averageGrade = value;
                OnPropertyChanged();
            }
        }
        public string TeachersGrade
        { 
            get { return _teachersGrade; }
            set 
            { 
                _teachersGrade = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> MondayClasses
        {
            get { return _mondayClasses; }
            set
            {
                _mondayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> TuesdayClasses
        {
            get { return _tuesdayClasses; }
            set
            {
                _tuesdayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> WednesdayClasses
        {
            get { return _wednesdayClasses; }
            set
            {
                _wednesdayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> ThursdayClasses
        {
            get { return _thursdayClasses; }
            set
            {
                _thursdayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> FridayClasses
        {
            get { return _fridayClasses; }
            set
            {
                _fridayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> SaturdayClasses
        {
            get { return _saturdayClasses; }
            set
            {
                _saturdayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> SundayClasses
        {
            get { return _sundayClasses; }
            set
            {
                _sundayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<PenaltyPoint> PenaltyPoints
        { 
            get { return _penaltyPoints; }
            set
            { 
                _penaltyPoints = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> DropoutCommand { get; set; }
        public RelayCommand<object> GradeTeacherCommand { get; set; }
        public Action DisableTeacherGradingAction { get; set; }

        public ActiveCourseViewModel(Student _student) 
        { 
            courseService = App.ServiceProvider.GetService<ICourseService>();
            teacherService = App.ServiceProvider.GetService<ITeacherService>();
            penaltyPointService = App.ServiceProvider.GetService<IPenaltyPointService>();
            studentService = App.ServiceProvider.GetService<IStudentService>();

            student = _student;
            course = courseService.GetCourse(student.EnrolledCourseId);
            teacher = teacherService.GetTeacherByCourseId(student.EnrolledCourseId);
            _allClasses = course.ClassPeriods;

            SetSchedule();
            SetCourseInfo();
            SetTeacherInfo();

            List<PenaltyPoint> penaltyPoints = penaltyPointService.GetAllPenaltyPointsByIds(student.PenaltyPoints);
            PenaltyPoints = new ObservableCollection<PenaltyPoint>(penaltyPoints);

            DropoutCommand = new RelayCommand<object>(Dropout, CanDropout);
            GradeTeacherCommand = new RelayCommand<object>(GradeTeacher, CanGradeTeacher);
        }

        private void SetSchedule()
        {
            MondayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Monday)));
            TuesdayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Tuesday)));
            WednesdayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Wednesday)));
            ThursdayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Thursday)));
            FridayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Friday)));
            SaturdayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Saturday)));
            SundayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Sunday)));
        }

        private void SetCourseInfo()
        { 
            LanguageProficiency = course.LanguageProficiency.LanguageName + " " + course.LanguageProficiency.LanguageLevel.ToString();
            BeginningDate = course.BeginningDate;
            Duration = course.Duration.ToString() + " weeks";
            CourseType = course.CourseType.ToString();
        }

        private void SetTeacherInfo()
        {
            TeacherName = teacher.Name;
            TeacherSurname = teacher.Surname;
            AverageGrade = teacher.CalculateAverageGrade();
        }

        public bool CanDropout(object? parameter) 
        { 
            return (DateTime.Now - course.BeginningDate).TotalDays >= 7; 
        }
        public void Dropout(object? parameter) 
        { 
            DropoutFormView dropoutForm = new DropoutFormView(student.Email, course.Id);
            dropoutForm.Show();
        }

        public bool CanGradeTeacher(object? parameter) 
        {
            //Need to add condition that checks if course is finished
            return true;
        }
        public void GradeTeacher(object? parameter) 
        {
            if (string.IsNullOrEmpty(TeachersGrade))
            {
                PopupMessageView invalidGradeError = new PopupMessageView("ERROR", "Please enter grade !");
                invalidGradeError.Show();
                return;
            }
            if (!int.TryParse(TeachersGrade, out _givenGrade))
            {
                PopupMessageView invalidGradeError = new PopupMessageView("ERROR", "Grade must be number between 1-10 !");
                invalidGradeError.Show();
                TeachersGrade = "";
                return;
            }
            try
            {
                teacherService.GradeTeacher(_givenGrade, teacher.Email);
                PopupMessageView successPopup = new PopupMessageView("SUCCESS", "Teacher graded successfully !");
                successPopup.Show();
                SetTeacherInfo();
                studentService.WithdrawStudentFromCourse(student.Email);
                DisableTeacherGradingAction();
            }
            catch (UserException ex)
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", ex.Text);
                errorPopup.Show();
                TeachersGrade = "";
            }
        }
    }
}
