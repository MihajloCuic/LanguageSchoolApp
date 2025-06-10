using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.service.Users.Students;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using LanguageSchoolApp.view;
using LanguageSchoolApp.model;
using LanguageSchoolApp.service.Users.Directors;

namespace LanguageSchoolApp.viewModel.Exams
{
    public class GradeExamsViewModel : ObservableObject
    {
        private readonly IExamService examService;
        private readonly IStudentService studentService;
        private readonly IDirectorService directorService;
        private readonly Exam exam;
        private Student? selectedStudent;

        private List<Student> _allStudents;
        private ObservableCollection<Student> _students;

        private string _languageName;
        private string _level;
        private string _examDate;
        private string _examTime;

        private string _studentFullName;
        private string _readingGrade;
        private string _writingGrade;
        private string _listeningGrade;
        private string _speakingGrade;
        private int _totalScore;

        public ObservableCollection<Student> Students
        { 
            get { return _students; }
            set 
            {
                _students = value;
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
        public string Level
        {
            get { return _level; }
            set
            {
                _level = value;
                OnPropertyChanged();
            }
        }
        public string ExamDate
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

        public string StudentFullName
        { 
            get { return _studentFullName; }
            set
            { 
                _studentFullName = value;
                OnPropertyChanged();
            }
        }
        public string ReadingGrade
        { 
            get { return _readingGrade; }
            set
            { 
                _readingGrade = value;
                CalculateTotalScore();
                OnPropertyChanged();
            }
        }
        public string WritingGrade
        {
            get { return _writingGrade; }
            set
            {
                _writingGrade = value;
                CalculateTotalScore();
                OnPropertyChanged();
            }
        }
        public string ListeningGrade
        {
            get { return _listeningGrade; }
            set
            {
                _listeningGrade = value;
                CalculateTotalScore();
                OnPropertyChanged();
            }
        }
        public string SpeakingGrade
        {
            get { return _speakingGrade; }
            set
            {
                _speakingGrade = value;
                CalculateTotalScore();
                OnPropertyChanged();
            }
        }
        public int TotalScore
        { 
            get { return _totalScore; }
            set
            { 
                _totalScore = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<string> SelectStudentCommand { get; set; }
        public RelayCommand<object> FinishCommand { get; set; }
        public RelayCommand<object> GradeStudentCommand { get; set; }
        public Action SwitchToTeacherExamView { get; set; }

        public GradeExamsViewModel(int examId) 
        { 
            examService = App.ServiceProvider.GetService<IExamService>();
            studentService = App.ServiceProvider.GetService<IStudentService>();
            directorService = App.ServiceProvider.GetService<IDirectorService>();

            exam = examService.GetExam(examId);
            LanguageName = exam.LanguageProficiency.LanguageName;
            Level = exam.LanguageProficiency.LanguageLevel.ToString();
            ExamDate = exam.ExamDate.ToString("dd.MM.yyyy.");
            ExamTime = exam.ExamDate.ToString("HH:mm");
            List<Student> unfilteredStudents = studentService.GetAllStudentsByIds(exam.Participants);
            _allStudents = unfilteredStudents.Where(student => !student.FinishedExamResults.Any(result => result.ExamId == examId)).ToList();
            Students = new ObservableCollection<Student>(_allStudents);

            SelectStudentCommand = new RelayCommand<string>(SelectStudent, CanSelectStudent);
            FinishCommand = new RelayCommand<object>(Finish, CanFinish);
            GradeStudentCommand = new RelayCommand<object>(GradeStudent, CanGradeStudent);
        }

        private bool CanSelectStudent(string studentId) { return true; }
        private void SelectStudent(string studentId) 
        { 
            selectedStudent = studentService.GetStudent(studentId);
            StudentFullName = selectedStudent.Name + " " + selectedStudent.Surname;
        }

        private bool CanFinish(object? parameter) { return true; }
        private void Finish(object? parameter) 
        {
            if (_allStudents.Count > 0)
            {
                PopupMessageView errorMessage = new PopupMessageView("ERROR", "You cannot finish exam before you grade all students !");
                errorMessage.Show();
                return;
            }

            examService.FinishExam(exam.Id);
            directorService.AddFinishedExam(exam.Id);
            SwitchToTeacherExamView();
        }

        private bool CanGradeStudent(object? parameter) { return true; }
        private void GradeStudent(object? parameter)
        {
            if (selectedStudent == null)
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", "Select student to grade first !");
                errorPopup.Show();
                return;
            }
            if (!int.TryParse(ReadingGrade, out int readingGrade) ||
                !int.TryParse(WritingGrade, out int writingGrade) ||
                !int.TryParse(ListeningGrade, out int listeningGrade) ||
                !int.TryParse(SpeakingGrade, out int speakingGrade))
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", "Please enter all the partial exam scores first !");
                errorPopup.Show();
                return;
            }

            Dictionary<ExamPart, int> partialScores = new Dictionary<ExamPart, int>();
            partialScores[ExamPart.Reading] = readingGrade;
            partialScores[ExamPart.Writing] = writingGrade;
            partialScores[ExamPart.Listening] = listeningGrade;
            partialScores[ExamPart.Speaking] = speakingGrade;
            bool passed = readingGrade >= 30 && writingGrade >= 30 && listeningGrade >= 20 && speakingGrade >= 25 && TotalScore >= 160;
            ExamResults examResults = new ExamResults(exam.Id, TotalScore, partialScores, passed);
            studentService.GradeStudentsExam(selectedStudent.Email, examResults);
            _allStudents.Remove(selectedStudent);
            UpdateStudentsList();
            UnSelectStudent();
        }

        private void UnSelectStudent()
        { 
            selectedStudent = null;
            StudentFullName = "";
            ReadingGrade = "";
            WritingGrade = "";
            ListeningGrade = "";
            SpeakingGrade = "";
            TotalScore = 0;
        }

        private void UpdateStudentsList()
        { 
            Students.Clear();
            foreach (Student student in _allStudents) 
            { 
                Students.Add(student);
            }
        }

        public void CalculateTotalScore()
        {
            int total = 0;
            if (int.TryParse(_readingGrade, out int readingGrade) && readingGrade >= 0 && readingGrade <= 60)
            {
                total += readingGrade;
            }
            if(int.TryParse(_writingGrade, out int writingGrade) && writingGrade >= 0 && writingGrade <= 60) 
            {
                total += writingGrade;
            }
            if (int.TryParse(_listeningGrade, out int listeningGrade) && listeningGrade >= 0 && listeningGrade <= 40)
            {
                total += listeningGrade;
            }
            if (int.TryParse(_speakingGrade, out int speakingGrade) && speakingGrade >= 0 && speakingGrade <= 50)
            {
                total += speakingGrade;
            }

            TotalScore = total;
        }
    }
}
