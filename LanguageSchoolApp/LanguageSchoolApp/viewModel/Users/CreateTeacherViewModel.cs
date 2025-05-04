using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.service.Users.Teachers;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using LanguageSchoolApp.model;
using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.view;

namespace LanguageSchoolApp.viewModel.Users
{
    public class CreateTeacherViewModel : ObservableObject
    {
        private readonly ITeacherService teacherService;
        private ObservableCollection<LanguageProficiencySlot> _proficiencies;
        private List<LanguageProficiency> allProficiencies;
        private int _pageNumber;
        private string _languageName;
        private string _languageLevel;

        private string _name;
        private string _surname;
        private string _email;
        private string _phoneNumber;
        private DateTime _birthday;
        private string _password;
        private string _confirmPassword;

        private string _nameError;
        private string _surnameError;
        private string _emailError;
        private string _phoneNumberError;
        private string _birthdayError;
        private string _passwordError;
        private string _confirmPasswordError;
        private string _error;

        public ObservableCollection<LanguageProficiencySlot> Proficiencies
        { 
            get { return _proficiencies; }
            set 
            { 
                _proficiencies = value;
                OnPropertyChanged();
            }
        }
        public int PageNumber
        { 
            get { return _pageNumber; }
            set
            { 
                _pageNumber = value;
                OnPropertyChanged();
            }
        }
        public string LanguageName
        { 
            get { return _languageName; }
            set 
            { 
                _languageName = value;
                OnPropertyChanged();
            }
        }
        public string ProficiencyLevel
        { 
            get { return _languageLevel; }
            set 
            { 
                _languageLevel = value;
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
        public string Email 
        { 
            get { return _email; }
            set
            { 
                _email = value;
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
        public string Gender { get; set; }

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
        public string EmailError
        {
            get { return _emailError; }
            set
            {
                _emailError = value;
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
        public string Error
        { 
            get { return _error; } 
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<string> GenderCommand { get; set; }
        public RelayCommand<object> AddProficiencyCommand { get; set; }
        public RelayCommand<LanguageProficiency> DeleteItemCommand { get; set; }
        public RelayCommand<object> PreviousPageCommand { get; set; }
        public RelayCommand<object> NextPageCommand { get; set; }
        public RelayCommand<object> SubmitCommand { get; set; }
        public Action CloseAction { get; set; }

        public CreateTeacherViewModel() 
        { 
            teacherService = App.ServiceProvider.GetService<ITeacherService>();

            Birthday = DateTime.Now;
            Gender = "Male";
            PageNumber = 1;
            allProficiencies = new List<LanguageProficiency>();
            InitializeProficienciesList();
            SetErrorFields();

            GenderCommand = new RelayCommand<string>(SetGender, CanSetGender);
            AddProficiencyCommand = new RelayCommand<object>(AddProficiency, CanAddProficiency);
            DeleteItemCommand = new RelayCommand<LanguageProficiency>(DeleteItem, CanDeleteItem);
            PreviousPageCommand = new RelayCommand<object>(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand<object>(NextPage, CanNextPage);
            SubmitCommand = new RelayCommand<object>(Submit, CanSubmit);
        }

        private void UpdateSlots() 
        {
            foreach (var slot in Proficiencies) //clearing slots
            {
                slot.HasData = false;
                slot.Proficiency = null;
            }

            int position = 0;
            foreach (LanguageProficiency proficiency in GetSlicedProficiencies())
            {
                Proficiencies[position].HasData = true;
                Proficiencies[position].Proficiency = proficiency;
                position++;
            }
        }

        private List<LanguageProficiency> GetSlicedProficiencies() 
        { 
            int elementsToSkip = (PageNumber - 1) * 10;
            return allProficiencies.Skip(elementsToSkip).Take(10).ToList();
        }

        private void InitializeProficienciesList() 
        {
            Proficiencies = new ObservableCollection<LanguageProficiencySlot>();
            for (int i = 0; i < 10; i++) 
            {
                Proficiencies.Add(new LanguageProficiencySlot
                {
                    HasData = false,
                    Proficiency = null
                });
            }
            UpdateSlots();
        }

        private bool CanSetGender(string gender) { return true; }
        private void SetGender(string gender) { Gender = gender; }

        private bool CanAddProficiency(object? parameter) { return true; }
        private void AddProficiency(object? parameter) 
        {
            LanguageLevel level = Enum.Parse<LanguageLevel>(ProficiencyLevel);
            allProficiencies.Add(new LanguageProficiency(LanguageName, level));
            UpdateSlots();
        }

        private bool CanDeleteItem(LanguageProficiency proficiency) { return proficiency != null; }
        private void DeleteItem(LanguageProficiency proficiency) 
        { 
            allProficiencies.Remove(proficiency);
            UpdateSlots();
        }

        private bool CanNextPage(object? parameter) { return PageNumber < (double)allProficiencies.Count / 10; }
        private void NextPage(object? parameter)
        {
            PageNumber++;
            UpdateSlots();
        }

        private bool CanPreviousPage(object? parameter) { return PageNumber > 1; }
        private void PreviousPage(object? parameter)
        {
            PageNumber--;
            UpdateSlots();
        }

        private bool CanSubmit(object? parameter) { return true; }
        private void Submit(object? parameter) 
        {
            try
            {
                teacherService.CreateTeacher(Name, Surname, Gender, Birthday.ToString("dd.MM.yyyy."), PhoneNumber, Email, Password, ConfirmPassword, allProficiencies);
                PopupMessageView successPopup = new PopupMessageView("SUCCESS", "Teacher created successfully !");
                successPopup.Show();
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
                    case UserValidationExceptionType.EmailExists: 
                        EmailError = ex.Text;
                        break;
                    case UserValidationExceptionType.InvalidEmailFormat:
                        EmailError = ex.Text;
                        break;
                    case UserValidationExceptionType.InvalidPhoneFormat: 
                        PhoneNumberError = ex.Text;
                        break;
                    case UserValidationExceptionType.InvalidBirthdayInput: 
                        BirthdayError = ex.Text;
                        break;
                    case UserValidationExceptionType.PasswordsNotMatching: 
                        PasswordError = ex.Text;
                        break;
                    case UserValidationExceptionType.EmptyPasswordField:
                        PasswordError = ex.Text;
                        break;
                    case UserValidationExceptionType.EmptyConfirmPasswordField: 
                        ConfirmPasswordError = ex.Text;
                        break;
                    default:
                        Error = ex.Text;
                        break;
                        

                }
            }
        }

        private void SetErrorFields()
        {
            NameError = "";
            SurnameError = "";
            EmailError = "";
            PhoneNumberError = "";
            BirthdayError = "";
            PasswordError = "";
            ConfirmPasswordError = "";
            Error = "";
        }
    }
}
