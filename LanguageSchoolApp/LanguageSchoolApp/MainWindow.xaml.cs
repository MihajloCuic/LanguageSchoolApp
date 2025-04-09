using LanguageSchoolApp.model.Users;
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
using LanguageSchoolApp.viewModel;
using LanguageSchoolApp.service.Notifications;

namespace LanguageSchoolApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(User user)
        {
            InitializeComponent();
            DataContext = new MainViewModel(user);
            if (DataContext is MainViewModel viewModel) 
            {
                viewModel.CloseAction = new Action(this.Close);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}