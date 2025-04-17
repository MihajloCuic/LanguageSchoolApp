using LanguageSchoolApp.core;
using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.service.Users.Teachers;
using LanguageSchoolApp.view;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class ActiveCourseTeacherDetailsViewModel : ObservableObject
    {
        private readonly ITeacherService teacherService;
        private readonly IStudentService studentService;
        private Student student;
        private Teacher teacher;

        private string _teacherName;
        private string _teacherSurname;
        private double _averageGrade;
        private string _teachersGrade;
        private int _givenGrade;

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

        public RelayCommand<object> GradeTeacherCommand { get; set; }
        public Action DisableTeacherGradingAction { get; set; }

        public ActiveCourseTeacherDetailsViewModel(Student _student, Teacher _teacher)
        { 
            teacherService = App.ServiceProvider.GetService<ITeacherService>();
            studentService = App.ServiceProvider.GetService<IStudentService>();

            student = _student;
            teacher = _teacher;
            SetTeacherInfo();

            GradeTeacherCommand = new RelayCommand<object>(GradeTeacher, CanGradeTeacher);
        }

        private void SetTeacherInfo()
        {
            TeacherName = teacher.Name;
            TeacherSurname = teacher.Surname;
            AverageGrade = teacher.CalculateAverageGrade();
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
