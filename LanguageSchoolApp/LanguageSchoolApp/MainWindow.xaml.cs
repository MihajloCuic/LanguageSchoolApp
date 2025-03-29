﻿using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.repository.Courses;
using LanguageSchoolApp.repository.Exams;
using LanguageSchoolApp.repository.Notifications;
using LanguageSchoolApp.repository.Users;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.service.Users.Directors;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.service.Users.Teachers;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LanguageSchoolApp.service.Validation;
using LanguageSchoolApp.exceptions.Users;

namespace LanguageSchoolApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _user;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly IDirectorService _directorService;
        private readonly ICourseService _courseService;
        private readonly IExamService _examService;
        public MainWindow(User user)
        {
            InitializeComponent();
            _user = user;
            _studentService = App.ServiceProvider.GetService<IStudentService>(); //prosledjivanje service-a
            _teacherService = App.ServiceProvider.GetService<ITeacherService>();
            _directorService = App.ServiceProvider.GetService<IDirectorService>();
            _courseService = App.ServiceProvider.GetService<ICourseService>();
            _examService = App.ServiceProvider.GetService<IExamService>();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //_studentService.CreateStudent("name", "surname", "Male", "16.05.2003.", "0612758739", "email@gmail.com", "password", "password", "Master");
            //_studentService.UpdateStudent("email@gmail.com", "marko", "markovic", "Female", "13.03.2001.", "0654430598", "sifra", "sifra", "Doctorate");
            //_studentService.GetStudent("email@gmail.com");
            //_studentService.GetAllStudents();

            //List<KeyValuePair<string, string>> languageProficiencies = new List<KeyValuePair<string, string>>();
            //languageProficiencies.Add(new KeyValuePair<string, string>("English", "B2"));
            //languageProficiencies.Add(new KeyValuePair<string, string>("Spanish", "C1"));
            //_teacherService.CreateTeacher("name", "surname", "Female", "18.04.2002.", "064832295", "teacher@gmail.com", "password", "password", languageProficiencies);
            //_teacherService.UpdateTeacher("teacher@gmail.com", "mirka", "mirkovic", "Male", "18.05.2003.", "0654432456", "sifra", "sifra", languageProficiencies);
            //Teacher teacher = _teacherService.GetTeacher("teacher@gmail.com");
            //_teacherService.GetAllTeachers();

            //_directorService.UpdateDirector("director", "surname", "Male", "19.05.1973.", "1234567890", "director@gmail.com", "sifra", "sifra");
            //_directorService.GetDirector();

            //List<KeyValuePair<string, string>> classPeriods = new List<KeyValuePair<string, string>>();
            //classPeriods.Add(new KeyValuePair<string, string>("Monday", "13:30"));
            //classPeriods.Add(new KeyValuePair<string, string>("Wednesday", "17:00"));
            //classPeriods.Add(new KeyValuePair<string, string>("Friday", "20:30"));
            //_courseService.CreateCourse("English", "B2", 25, 2, classPeriods, "13.05.2025.", "Live", "teacher@gmail.com");
            //_courseService.GetAllCourses();
            //_courseService.GetCourse(952824467);
            //_courseService.UpdateCourse(952824467, "Spanish", "C1", 30, 3, classPeriods, "12.05.2025.", "Live");
            //_courseService.DeleteCourse(952824467);

            //_examService.CreateExam("Spanish", "B2", "12.04.2025 11:30", 10, teacher);
            //_examService.UpdateExam(1390667643, "English", "C1", "13.04.2025 20:00", 10, teacher);
            //_examService.DeleteExam(1390667643);
            //_examService.GetAllExams();
            //List<int> allIds = new List<int>();
            //allIds.Add(1390667643);
            //_examService.GetAllExamsById(allIds);
            //_examService.GetExam(1390667643);

            //try
            //{
            //    User user = Validations.Login("director@gmail.com", "sifra");
            //    login.Content = user.Name;
            //
            //    User user = Validations.Login("director@gmail.com", "jaaa");
            //    login.Content = user.Name;
            //}
            //catch (UserException ex) 
            //{ 
            //    login.Content = ex.Text;
            //}
        }
    }
}