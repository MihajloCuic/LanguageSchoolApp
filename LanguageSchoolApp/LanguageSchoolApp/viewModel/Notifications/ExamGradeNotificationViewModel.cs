using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.service.Helpers;
using LanguageSchoolApp.service.Users.Directors;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.view;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageSchoolApp.viewModel.Notifications
{
    public class ExamGradeNotificationViewModel : ObservableObject
    {
        private readonly IExamService examService;
        private readonly IDirectorService directorService;
        private readonly IEmailService emailService;
        private readonly IStudentService studentService;
        private List<Exam> _allFinishedExams;
        private ObservableCollection<Exam> _finishedExams;
        private int _pageNumber;

        public ObservableCollection<Exam> FinishedExams
        {
            get { return _finishedExams; }
            set
            {
                _finishedExams = value;
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

        public RelayCommand<int> ApplyCommand { get; set; }
        public RelayCommand<object> PreviousPageCommand { get; set; }
        public RelayCommand<object> NextPageCommand { get; set; }
        public ExamGradeNotificationViewModel(Director director) 
        { 
            examService = App.ServiceProvider.GetRequiredService<IExamService>();
            directorService = App.ServiceProvider.GetRequiredService<IDirectorService>();
            emailService = App.ServiceProvider.GetRequiredService<IEmailService>();
            studentService = App.ServiceProvider.GetRequiredService<IStudentService>();

            PageNumber = 1;
            _allFinishedExams = examService.GetAllExamsById(director.FinishedExamsIds);
            FinishedExams = new ObservableCollection<Exam>(GetSlicedFinishedExams());

            ApplyCommand = new RelayCommand<int>(SendMessages, CanSendMessages);
            PreviousPageCommand = new RelayCommand<object>(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand<object>(NextPage, CanNextPage);
        }

        private List<Exam> GetSlicedFinishedExams()
        {
            int elementsToSkip = (PageNumber - 1) * 6;
            return _allFinishedExams.Skip(elementsToSkip).Take(6).ToList();
        }

        private void UpdateExamList(List<Exam> ExamList)
        {
            _allFinishedExams = ExamList;
            FinishedExams.Clear();
            foreach (var Exam in GetSlicedFinishedExams())
            {
                FinishedExams.Add(Exam);
            }
        }

        private bool CanSendMessages(int ExamId) { return true; }
        private void SendMessages(int ExamId)
        {
            try
            {
                Exam Exam = examService.GetExam(ExamId);
                List<Student> students = studentService.GetAllStudentsByIds(Exam.Participants);
                emailService.SendExamResults(students, Exam);
                List<Exam> finishedExams = examService.GetAllExamsById(directorService.RemoveFinishedExam(ExamId));
                UpdateExamList(finishedExams);

                PopupMessageView successMessage = new PopupMessageView("SUCCESS", "Emails sent successfully !");
                successMessage.Show();
            }
            catch (Exception ex)
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", ex.Message);
                errorPopup.Show();
            }
        }

        private bool CanNextPage(object? parameter) { return PageNumber < (double)_allFinishedExams.Count / 6; }
        private void NextPage(object? parameter)
        {
            PageNumber++;
            FinishedExams.Clear();
            foreach (var Exam in GetSlicedFinishedExams())
            {
                FinishedExams.Add(Exam);
            }
        }

        private bool CanPreviousPage(object? parameter) { return PageNumber > 1; }
        private void PreviousPage(object? parameter)
        {
            PageNumber--;
            FinishedExams.Clear();
            foreach (var Exam in GetSlicedFinishedExams())
            {
                FinishedExams.Add(Exam);
            }
        }
    }
}
