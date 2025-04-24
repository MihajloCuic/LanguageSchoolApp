using LanguageSchoolApp.core;
using LanguageSchoolApp.exceptions.Exams;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.service.Users.Teachers;
using LanguageSchoolApp.view;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LanguageSchoolApp.viewModel.Exams
{
    public class CreateExamViewModel : ObservableObject
    {
        private readonly IExamService examService;
        private readonly ITeacherService teacherService;
        private readonly IExamApplicationService examApplicationService;
        private readonly Teacher teacher;
        private Exam exam;

        private string _languageName;
        private string _languageLevel;
        private int _maxParticipants;
        private DateTime _examDate;
        private string _examTime;

        private string _languageNameError;
        private string _languageLevelError;
        private string _maxParticipantsError;
        private string _examDateError;
        private string _examTimeError;
        private string _defaultError;

        private Visibility _deleteButtonVisible;
        public Visibility DeleteButtonVisible 
        { 
            get { return _deleteButtonVisible; }
            set
            { 
                _deleteButtonVisible = value;
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
        public int MaxParticipants
        {
            get { return _maxParticipants; }
            set
            {
                _maxParticipants = value;
                OnPropertyChanged();
            }
        }
        public DateTime ExamDate
        {
            get { return _examDate; }
            set
            {
                _examDate = value;
                OnPropertyChanged();
            }
        }
        public string ExamTime
        { 
            get { return _examTime; }
            set 
            { 
                _examTime = value;
                OnPropertyChanged();
            }
        }

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
                _languageLevelError = value;
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
        public string ExamDateError
        {
            get { return _examDateError; }
            set
            {
                _examDateError = value;
                OnPropertyChanged();
            }
        }
        public string ExamTimeError
        { 
            get { return _examTimeError; }
            set 
            {
                _examTimeError = value;
                OnPropertyChanged();
            }
        }
        public string DefaultError
        {
            get { return _defaultError; }
            set 
            { 
                _defaultError = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> SubmitCommand { get; set; }
        public RelayCommand<object> DeleteCommand { get; set; }
        public Action SwitchToTeacherExams {  get; set; }

        public CreateExamViewModel(Teacher _teacher) 
        { 
            examService = App.ServiceProvider.GetService<IExamService>();
            teacherService = App.ServiceProvider.GetService<ITeacherService>();
            examApplicationService = App.ServiceProvider.GetService<IExamApplicationService>();

            teacher = _teacher;
            ExamDate = DateTime.Now;
            SetErrorField();
            DeleteButtonVisible = Visibility.Collapsed;
            SubmitCommand = new RelayCommand<object>(Submit, CanSubmit);
        }

        public CreateExamViewModel(Teacher _teacher, int examId)
        {
            examService = App.ServiceProvider.GetService<IExamService>();
            teacherService = App.ServiceProvider.GetService<ITeacherService>();
            teacher = _teacher;
            exam = examService.GetExam(examId);

            LanguageName = exam.LanguageProficiency.LanguageName;
            LanguageLevel = exam.LanguageProficiency.LanguageLevel.ToString();
            MaxParticipants = exam.MaxParticipants;
            ExamDate = exam.ExamDate;
            ExamTime = exam.ExamDate.ToString("HH:mm");
            SetErrorField();
            DeleteButtonVisible = Visibility.Visible;

            SubmitCommand = new RelayCommand<object>(Submit, CanSubmit);
            DeleteCommand = new RelayCommand<object>(Delete, CanDelete);
        }

        private bool CanSubmit(object? parameter) { return true; }
        private void Submit(object? parameter) 
        {
            SetErrorField();
            try
            {
                TimeSpan timeSpan = TimeSpan.Parse(ExamTime);
                DateTime combinedDateTime = ExamDate.Add(timeSpan);
                if (exam == null)
                {
                    Exam exam = examService.CreateExam(LanguageName, LanguageLevel, combinedDateTime.ToString("dd.MM.yyyy. HH:mm"), MaxParticipants, teacher);
                    teacherService.AddExam(exam.Id, teacher.Email);
                    PopupMessageView successPopup = new PopupMessageView("SUCCESS", "Exam created successfully !");
                    successPopup.Show();
                    EmptyFields();
                }
                else
                {
                    examService.UpdateExam(exam.Id, LanguageName, LanguageLevel, combinedDateTime.ToString("dd.MM.yyyy. HH:mm"), MaxParticipants, teacher);
                    PopupMessageView successPopup = new PopupMessageView("SUCCESS", "Exam updated successfully !");
                    successPopup.Show();
                }
            }
            catch (ExamException ex) 
            {
                switch (ex.Type)
                { 
                    case ExamExceptionType.InvalidLanguageName:
                        LanguageNameError = ex.Text;
                        break;
                    case ExamExceptionType.InvalidLanguageLevel:
                        LanguageLevelError = ex.Text;
                        break;
                    case ExamExceptionType.InvalidMaxParticipants:
                        MaxParticipantsError = ex.Text;
                        break;
                    case ExamExceptionType.InvalidExamDate:
                        ExamDateError = ex.Text;
                        break;
                    case ExamExceptionType.TeacherIsBusy:
                        DefaultError = ex.Text;
                        break;
                    case ExamExceptionType.ExamAlreadyExists:
                        DefaultError = ex.Text;
                        break;
                    default:
                        DefaultError = ex.Text;
                        break;
                }
            }
        }

        private bool CanDelete(object? parameter) { return true; }
        private void Delete(object? parameter)
        {
            try
            {
                examService.DeleteExam(exam.Id);
                List<ExamApplication> examApplications = examApplicationService.GetAllExamApplicationsByExamId(exam.Id);
                examApplicationService.DeleteAllExamApplicationsByIds(examApplications.Select(x => x.ExamId).ToList());
                PopupMessageView successPopup = new PopupMessageView("SUCCESS", "Exam deleted successfully !");
                successPopup.Show();
                SwitchToTeacherExams();
            }
            catch (ExamException ex) 
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", ex.Text);
                errorPopup.Show();
            }
        }

        private void SetErrorField()
        {
            LanguageNameError = "";
            LanguageLevelError = "";
            MaxParticipantsError = "";
            ExamDateError = "";
            ExamTimeError = "";
        }

        private void EmptyFields()
        {
            LanguageName = "";
            LanguageLevel = "";
            MaxParticipants = 0;
            ExamDate = DateTime.Now;
            ExamTime = "";
        }
    }
}
