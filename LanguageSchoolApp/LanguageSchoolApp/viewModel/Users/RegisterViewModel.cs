using LanguageSchoolApp.core;
using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Users.Directors;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.service.Users.Teachers;
using LanguageSchoolApp.view;
using LanguageSchoolApp.view.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LanguageSchoolApp.viewModel.Users
{
    public class RegisterViewModel : ObservableObject
    {
        private readonly IStudentService studentService;
        private readonly ITeacherService teacherService;
        private readonly IDirectorService directorService;
        private Director? _director;
        private Teacher? _teacher;
        private Student? _student;

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

        private Visibility _deleteBtnVisibility = Visibility.Hidden;
        private bool _isDeleteBtnEnabled = false;

        public Visibility DeleteBtnVisibility
        {
            get { return _deleteBtnVisibility; }
            set
            {
                _deleteBtnVisibility = value;
                OnPropertyChanged();
            }
        }
        public bool IsDeleteBtnEnabled
        {
            get { return _isDeleteBtnEnabled; }
            set
            {
                _isDeleteBtnEnabled = value;
                OnPropertyChanged();
            }
        }

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

        public RelayCommand<string> GenderCommand { get; set; }
        public RelayCommand<object> RegisterCommand { get; set; }
        public RelayCommand<object> LoginCommand { get; set; }
        public RelayCommand<object> DeleteTeacherCommand { get; set; }
        public Action CloseAction { get; set; }

        public RegisterViewModel() 
        {
            studentService = App.ServiceProvider.GetService<IStudentService>();
            _birthday = DateTime.Now;
            Gender = "Male";
            ProfessionalDegreeStr = "ElementarySchool";
            _student = null;
            _teacher = null;
            _director = null;
            RegisterCommand = new RelayCommand<object>(Register, CanRegister);
            LoginCommand = new RelayCommand<object>(OpenLoginWindow, CanOpenLoginWindow);
            GenderCommand = new RelayCommand<string>(SetGender, CanSetGender);
        }

        public RegisterViewModel(Student student)
        {
            studentService = App.ServiceProvider.GetService<IStudentService>();
            _student = student;
            _teacher = null;
            _director = null;
            _birthday = _student.Birthday;
            Gender = _student.Gender.ToString();
            ProfessionalDegreeStr = _student.ProfessionalDegree.ToString();
            Degree = (int)_student.ProfessionalDegree;
            Name = _student.Name;
            Surname = _student.Surname;
            Email = _student.Email;
            PhoneNumber = _student.PhoneNumber;
            Password = _student.Password;
            ConfirmPassword = _student.Password;

            RegisterCommand = new RelayCommand<object>(Register, CanRegister);
            LoginCommand = new RelayCommand<object>(OpenLoginWindow, CanOpenLoginWindow);
            GenderCommand = new RelayCommand<string>(SetGender, CanSetGender);
        }

        public RegisterViewModel(Teacher teacher) 
        {
            teacherService = App.ServiceProvider.GetService<ITeacherService>();
            _teacher = teacher;
            _student = null;
            _director = null;
            _birthday = teacher.Birthday;
            Gender = teacher.Gender.ToString();
            Name = teacher.Name;
            Surname = teacher.Surname;
            Email = teacher.Email;
            PhoneNumber = teacher.PhoneNumber;
            Password= teacher.Password;
            ConfirmPassword = teacher.Password;

            RegisterCommand = new RelayCommand<object>(Register, CanRegister);
            LoginCommand = new RelayCommand<object>(OpenLoginWindow, CanOpenLoginWindow);
            GenderCommand = new RelayCommand<string>(SetGender, CanSetGender);
        }

        public RegisterViewModel(Teacher teacher, Director director)
        {
            teacherService = App.ServiceProvider.GetService<ITeacherService>();
            _teacher = teacher;
            _student = null;
            _director = director;
            _birthday = teacher.Birthday;
            Gender = teacher.Gender.ToString();
            Name = teacher.Name;
            Surname = teacher.Surname;
            Email = teacher.Email;
            PhoneNumber = teacher.PhoneNumber;
            Password = teacher.Password;
            ConfirmPassword = teacher.Password;

            DeleteBtnVisibility = Visibility.Visible;
            IsDeleteBtnEnabled = true;

            RegisterCommand = new RelayCommand<object>(Register, CanRegister);
            LoginCommand = new RelayCommand<object>(OpenLoginWindow, CanOpenLoginWindow);
            GenderCommand = new RelayCommand<string>(SetGender, CanSetGender);
            DeleteTeacherCommand = new RelayCommand<object>(DeleteTeacher, CanDeleteTeacher);
        }

        public RegisterViewModel(Director director)
        {
            directorService = App.ServiceProvider.GetService<IDirectorService>();
            _teacher = null;
            _student = null;
            _director = director;
            _birthday = director.Birthday;
            Gender = director.Gender.ToString();
            Name = director.Name;
            Surname = director.Surname;
            Email = director.Email;
            PhoneNumber = director.PhoneNumber;
            Password = director.Password;
            ConfirmPassword = director.Password;

            RegisterCommand = new RelayCommand<object>(Register, CanRegister);
            LoginCommand = new RelayCommand<object>(OpenLoginWindow, CanOpenLoginWindow);
            GenderCommand = new RelayCommand<string>(SetGender, CanSetGender);
        }

        private bool CanSetGender(string gender) { return true; }
        private void SetGender(string gender) { Gender = gender; }

        private bool CanDeleteTeacher(object? parameter) { return _teacher != null; }
        private void DeleteTeacher(object? parameter) 
        {
            try
            {
                if (_teacher == null)
                { 
                    throw new UserException("Teacher not found !", UserExceptionType.UserNotFound);
                }
                teacherService.DeleteTeacher(_teacher.Email);
                PopupMessageView successMessage = new PopupMessageView("SUCCESS", "Teacher deleted successfully !");
                successMessage.Show();
                MainWindow mainWindow = new MainWindow(_director);
                mainWindow.Show();
                CloseAction();
            }
            catch (UserException ex)
            {
                PopupMessageView errorMessage = new PopupMessageView("ERROR", ex.Text);
                errorMessage.Show();
            }
        }

        private bool CanRegister(object? parameter) { return true; }
        private void Register(object? parameter)
        {
            try
            {
                if (_student != null)
                {
                    _student = studentService.UpdateStudent(_student.Email, Name, Surname, Gender, Birthday.ToString("dd.MM.yyyy."), PhoneNumber, Password, ConfirmPassword, ProfessionalDegreeStr);
                    PopupMessageView successMessage = new PopupMessageView("SUCCESS", "Account edited successfully !");
                    successMessage.Show();
                    MainWindow mainWindow = new MainWindow(_student);
                    mainWindow.Show();
                }
                else if (_teacher != null)
                {
                    _teacher = teacherService.UpdateTeacher(_teacher.Email, Name, Surname, Gender, Birthday.ToString("dd.MM.yyyy."), PhoneNumber, Password, ConfirmPassword);
                    PopupMessageView successMessage = new PopupMessageView("SUCCESS", "Account edited successfully !");
                    successMessage.Show();
                    if (_director != null)
                    {
                        MainWindow mainWindow = new MainWindow(_director);
                        mainWindow.Show();
                    }
                    else 
                    {
                        MainWindow mainWindow = new MainWindow(_teacher);
                        mainWindow.Show();
                    }
                }
                else if (_director != null)
                { 
                    _director = directorService.UpdateDirector(Name, Surname, Gender, Birthday.ToString("dd.MM.yyyy."), PhoneNumber, Email, Password, ConfirmPassword);
                    PopupMessageView successMessage = new PopupMessageView("SUCCESS", "Account edited successfully !");
                    successMessage.Show();
                    MainWindow mainWindow = new MainWindow(_director);
                    mainWindow.Show();
                }
                else
                {
                    studentService.CreateStudent(Name, Surname, Gender, Birthday.ToString("dd.MM.yyyy."), PhoneNumber, Email, Password, ConfirmPassword, ProfessionalDegreeStr);
                    PopupMessageView successMessage = new PopupMessageView("SUCCESS", "Account created successfully !");
                    successMessage.Show();
                    Login loginWindow = new Login();
                    loginWindow.Show();
                }

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
