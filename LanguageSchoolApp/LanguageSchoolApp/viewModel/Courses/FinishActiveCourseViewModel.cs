using LanguageSchoolApp.core;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Users;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using LanguageSchoolApp.view;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.service.Users.Directors;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class FinishActiveCourseViewModel : ObservableObject
    {
        private readonly IStudentService studentService;
        private readonly ICourseService courseService;
        private readonly IDirectorService directorService;
        private readonly Course course;
        private readonly Teacher teacher;
        private List<GradedStudent> _allGradedStudents;
        private List<Student> _allUngradedStudents;

        private ObservableCollection<GradedStudent> _gradedStudents;
        private ObservableCollection<Student> _ungradedStudents;

        private Student _selectedStudent;
        private int _grade;

        public Student SelectedStudent
        {
            get {  return _selectedStudent; } 
            set
            { 
                _selectedStudent = value; 
                OnPropertyChanged();
            }
        }
        public int Grade
        {
            get { return _grade; }
            set
            {
                _grade = value + 1;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GradedStudent> GradedStudents
        { 
            get { return _gradedStudents; }
            set 
            { 
                _gradedStudents = value; 
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Student> UngradedStudents
        {
            get { return _ungradedStudents; }
            set
            {
                _ungradedStudents = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> FinishCourseCommand { get; set; }
        public RelayCommand<object> CancelCommand { get; set; }
        public RelayCommand<object> GradeStudentCommand { get; set; }
        public Action<Teacher, int> CancelGrading { get; set; }
        public Action<string> SwitchToTeacherCourses { get; set; }

        public FinishActiveCourseViewModel(Course _course, Teacher _teacher) 
        {
            studentService = App.ServiceProvider.GetService<IStudentService>();
            courseService = App.ServiceProvider.GetService<ICourseService>();
            directorService = App.ServiceProvider.GetService<IDirectorService>();
            course = _course;
            teacher = _teacher;

            _allUngradedStudents = studentService.GetAllStudentsByIds(course.ParticipantsIds);
            UngradedStudents = new ObservableCollection<Student>(_allUngradedStudents);
            GradedStudents = new ObservableCollection<GradedStudent>();

            GradeStudentCommand = new RelayCommand<object>(GradeStudent, CanGradeStudent);
            CancelCommand = new RelayCommand<object>(Cancel, CanCancel);
            FinishCourseCommand = new RelayCommand<object>(FinishCourse, CanFinishCourse);
        }

        private bool CanGradeStudent(object? parameter) { return true; }
        private void GradeStudent(object? parameter) 
        {
            GradedStudents.Add(studentService.GradeStudent(SelectedStudent.Email, Grade));
            UngradedStudents.Remove(studentService.GetStudent(SelectedStudent.Email));
        }

        private bool CanFinishCourse(object? parameter) { return true; }
        private void FinishCourse(object? parameter) 
        {
            if (UngradedStudents.Count() > 0)
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", "All student must be graded first !");
                errorPopup.Show();
                return;
            }
            PopupMessageView successPopup = new PopupMessageView("SUCCESS", "Course finished successfully !");
            successPopup.Show();
            courseService.FinishCourse(course.Id);
            directorService.AddFinishedCourse(course.Id);
            SwitchToTeacherCourses(teacher.Email);
        }

        private bool CanCancel(object? parameter) { return true; }
        private void Cancel(object? parameter) 
        {
            CancelGrading(teacher, course.Id);
        }
    }
}
