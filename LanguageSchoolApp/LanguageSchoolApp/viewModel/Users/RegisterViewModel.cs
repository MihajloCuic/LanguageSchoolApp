using LanguageSchoolApp.core;
using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.model;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.view.Users;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageSchoolApp.viewModel.Users
{
    public class RegisterViewModel : ObservableObject
    {
        private readonly IStudentService studentService;

        private string _name;
        private string _surname;
        private string _phoneNumber;
        private string _email;
        private DateTime _birthday;
        private string _password;
        private string _confirmPassword;
        private int _degree;

        private string _nameError;
        private string _surnameError;
        private string _phoneNumberError;
        private string _emailError;
        private string _birthdayError;
        private string _passwordError;
        private string _confirmPasswordError;
        private string _professionalDegreeError;
        private string _genderError;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        public DateTime Birthday
        { 
            get { return _birthday; }
            set
            {
                _birthday = value;
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
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }
        public int Degree
        { 
            get { return _degree; }
            set 
            { 
                _degree = value;
                ProfessionalDegree degree = (ProfessionalDegree) value;
                ProfessionalDegreeStr = degree.ToString();
                OnPropertyChanged();
            }
        }

        public string NameError
        {
            get { return _nameError; }
            set
            {
                _nameError = value;
                OnPropertyChanged();
            }
        }
        public string SurnameError
        {
            get { return _surnameError; }
            set
            {
                _surnameError = value;
                OnPropertyChanged();
            }
        }
        public string PhoneNumberError
        {
            get { return _phoneNumberError; }
            set
            {
                _phoneNumberError = value;
                OnPropertyChanged();
            }
        }
        public string EmailError
        {
            get { return _emailError; }
            set
            {
                _emailError = value;
                OnPropertyChanged();
            }
        }
        public string BirthdayError
        {
            get { return _birthdayError; }
            set
            {
                _birthdayError = value;
                OnPropertyChanged();
            }
        }
        public string PasswordError
        {
            get { return _passwordError; }
            set
            {
                _passwordError = value;
                OnPropertyChanged();
            }
        }
        public string ConfirmPasswordError
        {
            get { return _confirmPasswordError; }
            set
            {
                _confirmPasswordError = value;
                OnPropertyChanged();
            }
        }
        public string ProfessionalDegreeError
        {
            get { return _professionalDegreeError; }
            set
            {
                _professionalDegreeError = value;
                OnPropertyChanged();
            }
        }
        public string GenderError
        { 
            get { return _genderError; }
            set 
            {
                _genderError = value;
                OnPropertyChanged();
            }
        }

        public string ProfessionalDegreeStr { get; set; }
        public string Gender { get; set; }

        public RelayCommand<object> MaleCommand { get; set; }
        public RelayCommand<object> FemaleCommand { get; set; }
        public RelayCommand<object> RegisterCommand { get; set; }
        public RelayCommand<object> LoginCommand { get; set; }
        public Action CloseAction { get; set; }

        public RegisterViewModel() 
        {
            studentService = App.ServiceProvider.GetService<IStudentService>();
            _birthday = DateTime.Now;
            Gender = "Male";
            ProfessionalDegreeStr = "ElementarySchool";
            RegisterCommand = new RelayCommand<object>(Register, CanRegister);
            LoginCommand = new RelayCommand<object>(OpenLoginWindow, CanOpenLoginWindow);


            MaleCommand = new RelayCommand<object>(BeMale, CanBeMale);
            FemaleCommand = new RelayCommand<object>(BeFemale, CanBeFemale);
        }

        private bool CanBeMale(object? parameter) { return true; }
        private void BeMale(object? parameter) { Gender = "Male"; }
        private bool CanBeFemale(object? parameter) { return true; }
        private void BeFemale(object? parameter) { Gender = "Female"; }

        private bool CanRegister(object? parameter) { return true; }
        private void Register(object? parameter)
        {
            try
            {
                studentService.CreateStudent(Name, Surname, Gender, Birthday.ToString("dd.MM.yyyy."), PhoneNumber, Email, Password, ConfirmPassword, ProfessionalDegreeStr);
                //TODO: Add popup window which confirms creation of an account
                Login loginWindow = new Login();
                loginWindow.Show();
                CloseAction();
            }
            catch (UserValidationException ex) 
            {
                switch (ex.Type) 
                {
                    case UserValidationExceptionType.InvalidName:
                        NameError = ex.Text;
                        break;
                    case UserValidationExceptionType.InvalidSurname:
                        SurnameError = ex.Text;
                        break;
                    case UserValidationExceptionType.InvalidPhoneFormat:
                        PhoneNumberError = ex.Text;
                        break;
                    case UserValidationExceptionType.InvalidEmailFormat:
                        EmailError = ex.Text;
                        break;
                    case UserValidationExceptionType.EmptyEmailField:
                        EmailError = ex.Text;
                        break;
                    case UserValidationExceptionType.InvalidBirthdayInput:
                        BirthdayError = ex.Text;
                        break;
                    case UserValidationExceptionType.InvalidProfessionalDegree:
                        ProfessionalDegreeError = ex.Text;
                        break;
                    case UserValidationExceptionType.EmptyPasswordField:
                        PasswordError = ex.Text;
                        break;
                    case UserValidationExceptionType.EmptyConfirmPasswordField:
                        ConfirmPasswordError = ex.Text;
                        break;
                    case UserValidationExceptionType.PasswordsNotMatching:
                        ConfirmPasswordError = ex.Text;
                        break;
                    case UserValidationExceptionType.InvalidGender:
                        GenderError = ex.Text;
                        break;
                }
            }
        }

        private bool CanOpenLoginWindow(object? parameter) { return true; }
        private void OpenLoginWindow(object? parameter) 
        { 
            Login loginWindow = new Login();
            loginWindow.Show();
            CloseAction();
        }
    }
}
