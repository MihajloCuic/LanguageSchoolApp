using LanguageSchoolApp.core;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.model.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using LanguageSchoolApp.model.Exams;

namespace LanguageSchoolApp.viewModel.Exams
{
    public class StartExamViewModel : ObservableObject
    {
        private readonly IExamService examService;
        private readonly IStudentService studentService;
        private Student selectedStudent;

        private List<Student> allStudents;
        private ObservableCollection<Student> students;

        private string _languageName;
        private string _level;
        private string _examDate;
        private string _examTime;
        private string _participation;

        private string _name;
        private string _surname;
        private int _finishedCoursesNum;
        private int _finishedExamsNum;

        public ObservableCollection<Student> Students 
        { 
            get { return students; }
            set
            {
                students = value;
                OnPropertyChanged();
            }
        }

        public string LanguageName
        { 
            get { return  _languageName; }
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
        public string Participation
        {
            get { return _participation; }
            set
            {
                _participation = value;
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
        public int FinishedCoursesNum
        { 
            get { return _finishedCoursesNum; }
            set
            { 
                _finishedCoursesNum = value;
                OnPropertyChanged();
            }
        }
        public int FinishedExamsNum
        { 
            get { return _finishedExamsNum; }
            set
            {
                _finishedExamsNum = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<string> SelectStudentCommand { get; set; }

        public StartExamViewModel(int examId) 
        { 
            examService = App.ServiceProvider.GetService<IExamService>();
            studentService = App.ServiceProvider.GetService<IStudentService>();

            Exam exam = examService.GetExam(examId);
            LanguageName = exam.LanguageProficiency.LanguageName;
            Level = exam.LanguageProficiency.LanguageLevel.ToString();
            ExamDate = exam.ExamDate.ToString("dd.MM.yyyy.");
            ExamTime = exam.ExamDate.ToString("mm:HH");
            Participation = exam.Participants.Count + "/" + exam.MaxParticipants;

            allStudents = studentService.GetAllStudentsByIds(exam.Participants);
            Students = new ObservableCollection<Student>(allStudents);

            SelectStudentCommand = new RelayCommand<string>(SelectStudent, CanSelectStudent);
        }

        private bool CanSelectStudent(string studentId) { return allStudents.Count > 0; }
        private void SelectStudent(string studentId)
        { 
            selectedStudent = studentService.GetStudent(studentId);
            Name = selectedStudent.Name;
            Surname = selectedStudent.Surname;
            FinishedCoursesNum = selectedStudent.FinishedCourses.Count;
            FinishedExamsNum = selectedStudent.FinishedExamResults.Count;
        }
    }
}
