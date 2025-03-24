using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.repository.Courses;
using LanguageSchoolApp.repository.Exams;
using LanguageSchoolApp.repository.Notifications;
using LanguageSchoolApp.repository.Users;
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

namespace LanguageSchoolApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly IDirectorService _directorService;
        public MainWindow()
        {
            InitializeComponent();
            _studentService = App.ServiceProvider.GetService<IStudentService>(); //prosledjivanje service-a
            _teacherService = App.ServiceProvider.GetService<ITeacherService>();
            _directorService = App.ServiceProvider.GetService<IDirectorService>();
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
            //_teacherService.GetTeacher("teacher@gmail.com");
            //_teacherService.GetAllTeachers();

            //_directorService.UpdateDirector("director", "surname", "Male", "19.05.1973.", "1234567890", "director@gmail.com", "sifra", "sifra");
            //_directorService.GetDirector();
        }
    }
}