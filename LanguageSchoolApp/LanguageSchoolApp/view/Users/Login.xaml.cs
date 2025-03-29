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
using System.Windows.Shapes;

namespace LanguageSchoolApp.view.Users
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            if (DataContext is LoginViewModel viewModel)
            { 
                viewModel.CloseAction = new Action(this.Close);
            }
        }

        private void Exit(object sender, RoutedEventArgs e) 
        {
            this.Close();
        }
        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ShowPassword(object sender, RoutedEventArgs e)
        {
            textBoxPassword.Text = passwordBox.Password;
            textBoxPassword.Visibility = Visibility.Visible;
            passwordBox.Visibility = Visibility.Collapsed;
        }
        private void HidePassword(object sender, RoutedEventArgs e)
        {
            passwordBox.Password = textBoxPassword.Text;
            passwordBox.Visibility = Visibility.Visible;
            textBoxPassword.Visibility = Visibility.Collapsed;
        }
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel) 
            { 
                viewModel.Password = passwordBox.Password;
            }
        }
    }
}
