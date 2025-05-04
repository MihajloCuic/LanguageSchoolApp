using LanguageSchoolApp.viewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LanguageSchoolApp.view.Users
{
    /// <summary>
    /// Interaction logic for CreateTeacherView.xaml
    /// </summary>
    public partial class CreateTeacherView : UserControl
    {
        public CreateTeacherView()
        {
            InitializeComponent();
        }
        private void ShowPassword(object sender, RoutedEventArgs e)
        {
            PTextBox.Text = PPasswordBox.Password;
            PTextBox.Visibility = Visibility.Visible;
            PPasswordBox.Visibility = Visibility.Collapsed;
        }
        private void HidePassword(object sender, RoutedEventArgs e)
        {
            PPasswordBox.Password = PTextBox.Text;
            PPasswordBox.Visibility = Visibility.Visible;
            PTextBox.Visibility = Visibility.Collapsed;
        }
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateTeacherViewModel viewModel)
            {
                viewModel.Password = PPasswordBox.Password;
            }
        }

        private void ShowConfirmPassword(object sender, RoutedEventArgs e)
        {
            CPTextBox.Text = CPPasswordBox.Password;
            CPTextBox.Visibility = Visibility.Visible;
            CPPasswordBox.Visibility = Visibility.Collapsed;
        }
        private void HideConfirmPassword(object sender, RoutedEventArgs e)
        {
            CPPasswordBox.Password = CPTextBox.Text;
            CPPasswordBox.Visibility = Visibility.Visible;
            CPTextBox.Visibility = Visibility.Collapsed;
        }
        private void ConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateTeacherViewModel viewModel)
            {
                viewModel.ConfirmPassword = CPPasswordBox.Password;
            }
        }
    }

}
