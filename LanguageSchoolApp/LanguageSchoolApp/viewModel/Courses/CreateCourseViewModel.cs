using LanguageSchoolApp.core;
using LanguageSchoolApp.exceptions.Courses;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.service.Users.Teachers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class CreateCourseViewModel : ObservableObject
    {
        private readonly ICourseService courseService;
        private readonly ITeacherService teacherService;
        private Teacher _teacher;
        private ObservableCollection<ClassPeriodSlot> _periods;
        private List<ClassPeriod> _allPeriods;
        private int _pageNumber;
        private string _classDay;
        private string _classTime;

        private string _languageName;
        private string _languageLevel;
        private DateTime _beginningDate;
        private string _maxParticipants;
        private string _duration;

        private string _languageNameError;
        private string _languageLevelError;
        private string _beginningDateError;
        private string _maxParticipantsError;
        private string _durationError;
        private string _courseTypeError;
        private string _classPeriodError;

        public ObservableCollection<ClassPeriodSlot> Periods
        { 
            get { return _periods; }
            set
            { 
                _periods = value;
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
        public string ClassDay
        { 
            get { return _classDay; }
            set
            { 
                _classDay = value;
                OnPropertyChanged();
            }
        }
        public string ClassTime
        { 
            get { return _classTime; }
            set
            { 
                _classTime = value;
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
        public string LanguageLevel
        {
            get { return _languageLevel; }
            set
            {
                _languageLevel = value;
                OnPropertyChanged();
            }
        }
        public DateTime BeginningDate
        { 
            get { return _beginningDate; }
            set
            { 
                _beginningDate = value;
                OnPropertyChanged();
            }
        }
        public string MaxParticipants
        {
            get { return _maxParticipants; }
            set
            {
                _maxParticipants = value;
                OnPropertyChanged();
            }
        }
        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }
        public string CourseType { get; set; }

        public string LanguageNameError
        { 
            get { return _languageNameError; }
            set
            { 
                _languageNameError = value;
                OnPropertyChanged();
            }
        }
        public string LanguageLevelError
        { 
            get { return _languageLevelError; }
            set
            { 
                _languageLevelError= value;
                OnPropertyChanged();
            }
        }
        public string BeginningDateError
        { 
            get { return _beginningDateError; }
            set
            { 
                _beginningDateError = value;
                OnPropertyChanged();
            }
        }
        public string MaxParticipantsError
        { 
            get { return _maxParticipantsError; }
            set
            {
                _maxParticipantsError = value;
                OnPropertyChanged();
            }
        }
        public string DurationError
        { 
            get { return _durationError; }
            set
            { 
                _durationError = value;
                OnPropertyChanged();
            }
        }
        public string CourseTypeError
        { 
            get { return _courseTypeError; }
            set
            { 
                _courseTypeError = value;
                OnPropertyChanged();
            }
        }
        public string ClassPeriodError
        { 
            get { return _classPeriodError; }
            set
            { 
                _classPeriodError = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> AddClassPeriodCommand { get; set; }
        public RelayCommand<ClassPeriod> DeleteItemCommand { get; set; }
        public RelayCommand<object> PreviousPageCommand { get; set; }
        public RelayCommand<object> NextPageCommand { get; set; }
        public RelayCommand<object> OnlineCommand { get; set; }
        public RelayCommand<object> LiveCommand { get; set; }
        public RelayCommand<object> SubmitCommand { get; set; }

        public CreateCourseViewModel(Teacher teacher) 
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            teacherService = App.ServiceProvider.GetService<ITeacherService>();
            _teacher = teacher;
            PageNumber = 1;
            BeginningDate = DateTime.Now;
            CourseType = "Online";
            _allPeriods = new List<ClassPeriod>();
            InitializePeriodsList();

            SetErrorFields();

            AddClassPeriodCommand = new RelayCommand<object>(AddClassPeriod, CanAddClassPeriod);
            DeleteItemCommand = new RelayCommand<ClassPeriod>(DeleteItem, CanDeleteItem);
            OnlineCommand = new RelayCommand<object>(IsOnline, CanBeOnline);
            LiveCommand = new RelayCommand<object>(IsLive, CanBeLive);
            PreviousPageCommand = new RelayCommand<object>(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand<object>(NextPage, CanNextPage);
            SubmitCommand = new RelayCommand<object>(Submit, CanSubmit);
        }

        private void InitializePeriodsList()
        { 
            Periods = new ObservableCollection<ClassPeriodSlot>();
            for (int i = 0; i < 10; i++) 
            {
                Periods.Add(new ClassPeriodSlot
                {
                    HasData = false,
                    Period = null
                });
            }
            UpdateSlots();
        }

        private void UpdateSlots()
        {
            foreach (var slot in Periods) //clearing slots
            {
                slot.HasData = false;
                slot.Period = null;
            }

            int position = 0;
            foreach (ClassPeriod classPeriod in GetSlicedClassPeriods()) 
            {
                Periods[position].HasData = true;
                Periods[position].Period = classPeriod;
                position++;
            }
        }

        private List<ClassPeriod> GetSlicedClassPeriods()
        { 
            int elementsToSkip = (PageNumber - 1) * 10;
            return _allPeriods.Skip(elementsToSkip).Take(10).ToList();
        }

        private bool CanAddClassPeriod(object? parameter) 
        {
            return !string.IsNullOrEmpty(ClassDay) && !string.IsNullOrEmpty(ClassTime);
        }

        private void AddClassPeriod(object parameter) 
        {
            DaysOfWeek dayOfWeek = Enum.Parse<DaysOfWeek>(ClassDay);
            TimeOnly classTime = TimeOnly.Parse(ClassTime);

            _allPeriods.Add(new ClassPeriod(dayOfWeek, classTime));
            UpdateSlots();
        }

        private bool CanDeleteItem(ClassPeriod period) 
        {
            return period != null;
        }

        private void DeleteItem(ClassPeriod period) 
        { 
            _allPeriods.Remove(period);
            UpdateSlots();
        }

        private bool CanNextPage(object? parameter) { return PageNumber < (double)_allPeriods.Count / 10; }
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

        private bool CanBeOnline(object? parameter) { return true; }
        private void IsOnline(object? parameter) { CourseType = "Online"; }
        private bool CanBeLive(object? parameter) { return true; }
        private void IsLive(object? parameter) { CourseType = "Live"; }

        private bool CanSubmit(object? parameter) 
        {
            return !string.IsNullOrEmpty(LanguageName) || !string.IsNullOrEmpty(LanguageLevel) || !string.IsNullOrEmpty(MaxParticipants) || !string.IsNullOrEmpty(Duration); 
        }

        private void Submit(object? parameter)
        {
            SetErrorFields();
            try
            {
                if (!int.TryParse(MaxParticipants, out int maxParticipants)) 
                { 
                    throw new CourseException("Max participants must be number", CourseExceptionType.InvalidMaxParticipants);
                }
                if (!int.TryParse(Duration, out int duration)) 
                { 
                    throw new CourseException("Duration must be number", CourseExceptionType.InvalidDuration);
                }
                Course course = courseService.CreateCourse(LanguageName, LanguageLevel, maxParticipants, duration, _allPeriods, BeginningDate, CourseType, _teacher);
                teacherService.AddCourse(course.Id, _teacher.Email);
                ClassPeriodError = "Success"; //TODO: Insert popup message successful course creation
            }
            catch (CourseException ex)
            {
                switch (ex.Type) 
                {
                    case CourseExceptionType.InvalidLanguageProficiency:
                        LanguageNameError = ex.Text;
                        break;
                    case CourseExceptionType.InvalidLanguageName:
                        LanguageNameError = ex.Text;
                        break;
                    case CourseExceptionType.InvalidLanguageLevel:
                        LanguageLevelError = ex.Text;
                        break;
                    case CourseExceptionType.InvalidBeginningDate:
                        BeginningDateError = ex.Text;
                        break;
                    case CourseExceptionType.InvalidMaxParticipants:
                        MaxParticipantsError = ex.Text;
                        break;
                    case CourseExceptionType.InvalidDuration:
                        DurationError = ex.Text;
                        break;
                    case CourseExceptionType.InvalidCourseType:
                        CourseTypeError = ex.Text;
                        break;
                    case CourseExceptionType.ClassroomsFull:
                        ClassPeriodError = ex.Text;
                        break;
                    case CourseExceptionType.CourseAlreadyExists:
                        ClassPeriodError = ex.Text;
                        break;

                }
            }
        }

        private void SetErrorFields()
        {
            LanguageNameError = "";
            LanguageLevelError = "";
            BeginningDateError = "";
            MaxParticipantsError = "";
            DurationError = "";
            CourseTypeError = "";
            ClassPeriodError = "";
        }
    }
}
