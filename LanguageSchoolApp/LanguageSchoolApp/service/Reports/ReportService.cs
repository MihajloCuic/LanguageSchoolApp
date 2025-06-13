using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.service.Users.PenaltyPoints;
using LanguageSchoolApp.service.Users.Students;

namespace LanguageSchoolApp.service.Reports
{
    public class ReportService : IReportService
    {
        private readonly ICourseService courseService;
        private readonly IStudentService studentService;
        private readonly ICourseDropoutRequestService dropoutService;
        private readonly IExamService examService;
        private readonly IPenaltyPointService penaltyPointService;

        public ReportService(ICourseService _courseService, IStudentService _studentService, ICourseDropoutRequestService _dropoutService, IExamService _examService, IPenaltyPointService _penaltyPointService)
        {
            courseService = _courseService;
            studentService = _studentService;
            dropoutService = _dropoutService;
            examService = _examService;
            penaltyPointService = _penaltyPointService;
        }

        public Dictionary<string, double> AverageCourseGradeReport()
        {
            Dictionary<string, double> reportResults = new Dictionary<string, double>();
            Dictionary<int, Course> allCourses = courseService.GetAllCourses();
            foreach (Course course in allCourses.Values)
            {
                List<Student> participants = studentService.GetAllStudentsByIds(course.ParticipantsIds); //taking list of all participants
                var grades = participants.SelectMany(student => student.FinishedCourses) //we take all finishedCourses
                    .Where(finishedCourse => finishedCourse.CourseId == course.Id) //where the courseId matches with our course
                    .Select(finishedCourse => finishedCourse.Grade); //and only take grade
                double averageGrade = grades.Any() ? grades.Average() : 0.0; // calculate average grade or if list is empty we set it to 0.0
                string courseName = course.LanguageProficiency.LanguageName + " " + course.LanguageProficiency.LanguageLevel.ToString() + " " + course.Id;
                reportResults.Add(courseName, averageGrade); //in the end adding the results and course name to results dictionary
            }

            return reportResults;
        }

        public Dictionary<string, int> AverageDropoutReport()
        {
            Dictionary<string, int> reportResults = new Dictionary<string, int>();
            Dictionary<int, Course> allCourses = courseService.GetAllCourses();
            Dictionary<int, CourseDropoutRequest> allDropoutRequests = dropoutService.GetAllDropoutRequests();
            reportResults["dropout"] = allDropoutRequests.Count; //this will collect all student who dropped out
            int finished = 0;
            foreach (Course course in allCourses.Values) 
            {
                finished += course.ParticipantsIds.Count; //we count all students that participated in all courses
            }
            reportResults["finished"] = finished - allDropoutRequests.Count; // in the end we subtract the ones that dropped out

            return reportResults;
        }

        public Dictionary<string, int> PassedFailedExamReport()
        {
            Dictionary<string, int> reportResults = new Dictionary<string, int>();
            reportResults.Add("passed", 0);
            reportResults.Add("failed", 0);
            Dictionary<string, Student> allStudents = studentService.GetAllStudents();
            foreach (Student student in allStudents.Values)
            {
                reportResults["passed"] += student.FinishedExamResults.Where(finishedExam => finishedExam.Passed == true).Count();
                reportResults["failed"] += student.FinishedExamResults.Where(finishedExam => finishedExam.Passed == false).Count();
            }
            return reportResults;
        }

        public Dictionary<string, double> AverageExamScoreReport()
        { 
            Dictionary<string, double> reportResults = new Dictionary<string, double>();
            Dictionary<int, Exam> allExams = examService.GetAllExams();
            foreach (Exam exam in allExams.Values) 
            { 
                List<Student> participants = studentService.GetAllStudentsByIds(exam.Participants);
                var scores = participants.SelectMany(student => student.FinishedExamResults)
                                         .Where(finishedExam => finishedExam.ExamId == exam.Id)
                                         .Select(finishedExam => finishedExam.TotalScore);
                double average = scores.Any() ? scores.Average() : 0.0;
                string examName = exam.LanguageProficiency.LanguageName + " " + exam.LanguageProficiency.LanguageLevel.ToString() + " " + exam.Id;
                reportResults.Add(examName, average);
            }

            return reportResults;
        }

        public Dictionary<string, double> AverageLanguageNumberReport()
        { 
            Dictionary<string, double> reportResults = new Dictionary<string, double>();
            Dictionary<int, Course> allCourses = courseService.GetAllCourses();
            Dictionary<int, Exam> allExams = examService.GetAllExams();
            foreach (Course course in allCourses.Values) 
            {
                if (!reportResults.ContainsKey(course.LanguageProficiency.LanguageName))
                {
                    reportResults[course.LanguageProficiency.LanguageName] = 1.0;
                }
                reportResults[course.LanguageProficiency.LanguageName]++;
            }
            foreach(Exam exam in allExams.Values)
            {
                if (!reportResults.ContainsKey(exam.LanguageProficiency.LanguageName))
                {
                    reportResults[exam.LanguageProficiency.LanguageName] = 1.0;
                }
                reportResults[exam.LanguageProficiency.LanguageName]++;
            }

            return reportResults;
        }

        public Dictionary<string, int> AveragePenaltyPointsPerLanguageReport()
        {
            Dictionary<string, int> reportResults = new Dictionary<string, int>();
            Dictionary<int, Course> allCourses = courseService.GetAllCourses();
            Dictionary<int, Exam> allExams = examService.GetAllExams();
            Dictionary<int, PenaltyPoint> allPenaltyPoints = penaltyPointService.GetAllPenaltyPoints();

            foreach (Course course in allCourses.Values) //first we find all languages in courses
            {
                if (reportResults.ContainsKey(course.LanguageProficiency.LanguageName))
                {
                    continue;
                }
                reportResults[course.LanguageProficiency.LanguageName] = 0;
            }
            foreach (Exam exam in allExams.Values) //then all languages in exams
            {
                if (reportResults.ContainsKey(exam.LanguageProficiency.LanguageName))
                {
                    continue;
                }
                reportResults[exam.LanguageProficiency.LanguageName] = 0;
            }

            foreach (PenaltyPoint penaltyPoint in allPenaltyPoints.Values) //then we count how many penalty points each language has
            { 
                Course course = courseService.GetCourse(penaltyPoint.CourseId);
                reportResults[course.LanguageProficiency.LanguageName]++;
            }

            return reportResults;
        }
    }
}
