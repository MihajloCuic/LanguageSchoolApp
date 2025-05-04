using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.viewModel.Users;
using LanguageSchoolApp.model;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            Button confirmButton = (Button)FindName("ConfirmButton");
            confirmButton.Content = "Register";
            DataContext = new RegisterViewModel();
            if (DataContext is RegisterViewModel viewModel) 
            {
                viewModel.CloseAction = new Action(this.Close);
            }
        }

        public Register(Student student) 
        {
            InitializeComponent();
            SetupPage(student.Gender, true);

            DataContext = new RegisterViewModel(student);
            if (DataContext is RegisterViewModel viewModel)
            {
                viewModel.CloseAction = new Action(this.Close);
            }
        }

        public Register(Teacher teacher)
        {
            InitializeComponent();
            SetupPage(teacher.Gender, false);

            DataContext = new RegisterViewModel(teacher);
            if (DataContext is RegisterViewModel viewModel)
            {
                viewModel.CloseAction = new Action(this.Close);
            }
        }

        public Register(Teacher teacher, Director director)
        {
            InitializeComponent();
            SetupPage(teacher.Gender, false);

            DataContext = new RegisterViewModel(teacher, director);
            if (DataContext is RegisterViewModel viewModel)
            {
                viewModel.CloseAction = new Action(this.Close);
            }
        }

        public Register(Director director)
        {
            InitializeComponent();
            SetupPage(director.Gender, false);

            DataContext = new RegisterViewModel(director);
            if (DataContext is RegisterViewModel viewModel)
            {
                viewModel.CloseAction = new Action(this.Close);
            }
        }

        private void SetupPage(Gender gender, bool isStudent) 
        {
            Button confirmButton = (Button)FindName("ConfirmButton");
            confirmButton.Content = "Confirm";
            if (gender.Equals(Gender.Male))
            {
                RadioButton maleRadioButton = (RadioButton)FindName("MaleButton");
                RadioButton femaleRadioButton = (RadioButton)FindName("FemaleButton");
                maleRadioButton.IsChecked = true;
                femaleRadioButton.IsChecked = false;
            }
            else
            {
                RadioButton maleRadioButton = (RadioButton)FindName("MaleButton");
                RadioButton femaleRadioButton = (RadioButton)FindName("FemaleButton");
                maleRadioButton.IsChecked = false;
                femaleRadioButton.IsChecked = true;
            }

            Label loginLabel = (Label)FindName("loginLabel");
            Button loginButton = (Button)FindName("loginButton");
            TextBox emailTextBox = (TextBox)FindName("emailTextBox");
            loginLabel.Visibility = Visibility.Hidden;
            loginButton.Visibility = Visibility.Hidden;
            emailTextBox.IsEnabled = false;

            if (!isStudent)
            {
                Label degreeLabel = (Label)FindName("degreeLabel");
                ComboBox degreeComboBox = (ComboBox)FindName("degreeComboBox");
                degreeLabel.Visibility = Visibility.Collapsed;
                degreeComboBox.Visibility = Visibility.Collapsed;
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
            if (DataContext is RegisterViewModel viewModel)
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
            if (DataContext is RegisterViewModel viewModel)
            {
                viewModel.ConfirmPassword = CPPasswordBox.Password;
            }
        }
    }
}
