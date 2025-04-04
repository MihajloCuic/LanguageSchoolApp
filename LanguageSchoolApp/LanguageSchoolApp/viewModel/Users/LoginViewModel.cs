using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.core;
using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.service.Validation;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.view.Users;

namespace LanguageSchoolApp.viewModel.Users
{
    public class LoginViewModel : ObservableObject
    {
        private string _username;
        private string _password;
        private bool _isUsernameValid = true;
        private bool _isPasswordValid = true;
        private string _errorMessage;

        public string Username 
        { 
            get { return _username; }
            set 
            { 
                _username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get { return _password; }
            set
            { 
                _password = value; 
                OnPropertyChanged();
            }
        }

        public bool IsUsernameValid 
        { 
            get { return _isUsernameValid; }
            set 
            { 
                _isUsernameValid = value;
                OnPropertyChanged();
            }
        }

        public bool IsPasswordValid
        {
            get { return _isPasswordValid; }
            set
            {
                _isPasswordValid = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage 
        { 
            get { return _errorMessage; }
            set 
            { 
                _errorMessage = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand<object> LoginCommand { get; set; }
        public RelayCommand<object> RegisterCommand { get; set; }
        public Action CloseAction { get; set; }

        public LoginViewModel() 
        {
            LoginCommand = new RelayCommand<object>(Login, CanLogin);
            RegisterCommand = new RelayCommand<object>(OpenRegisterWindow, CanOpenRegisterWindow);
        }

        public bool CanLogin(object? parameter) 
        {
            return true;
        }

        private void Login(object? parameter) 
        {
            try
            {
                User user = Validations.Login(Username, Password);
                MainWindow window = new MainWindow(user);
                window.Show();
                CloseAction();
            }
            catch (UserException ex) 
            { 
                IsUsernameValid = false;
                IsPasswordValid = false;
                ErrorMessage = ex.Text;
            }
        }

        private void OpenRegisterWindow(object? parameter) 
        {
            Register registerWindow = new Register();
            registerWindow.Show();
            CloseAction();
        }

        private bool CanOpenRegisterWindow(object? parameter) 
        {
            return true;
        }
    }
}
