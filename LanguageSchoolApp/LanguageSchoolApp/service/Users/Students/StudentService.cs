using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.model;
using LanguageSchoolApp.service.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.repository.Users.Students;
using LanguageSchoolApp.service.Validation;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.repository.Exams;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.service.Users.PenaltyPoints;

namespace LanguageSchoolApp.service.Users.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;
        private readonly ICourseApplicationService courseApplicationService;
        private readonly IExamApplicationService examApplicationService;
        private readonly ICourseService courseService;

        public StudentService(IStudentRepository _studentRepository, ICourseApplicationService _courseApplicationService, IExamApplicationService _examApplicationService, ICourseService _courseService)
        {
            studentRepository = _studentRepository;
            courseApplicationService = _courseApplicationService;
            examApplicationService = _examApplicationService;
            courseService = _courseService;
        }

        public Dictionary<string, Student> GetAllStudents() 
        {
            return studentRepository.GetAllStudents();
        }

        public List<Student> GetAllStudentsByIds(List<string> ids)
        { 
            return studentRepository.GetAllStudentsByIds(ids);
        }

        public Student GetStudent(string studentId)
        { 
            return studentRepository.GetStudent(studentId);
        }

        public bool StudentExists(string studentId)
        { 
            return studentRepository.StudentExists(studentId);
        }

        public void CreateStudent(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string email, string password, string confirmPassword, string professionalDegreeStr) 
        { 
            ValidateStudent(name, surname, genderStr, birthdayStr, phoneNumber, password, confirmPassword, professionalDegreeStr);
            Validations.ValidateEmail(email);
            Gender gender = Enum.Parse<Gender>(genderStr);
            DateTime birthday = DateTime.Parse(birthdayStr);
            ProfessionalDegree professionalDegree = Enum.Parse<ProfessionalDegree>(professionalDegreeStr);
            Student newStudent = new Student(name, surname, gender, birthday, phoneNumber, email, password, professionalDegree);
            studentRepository.AddStudent(newStudent);
        }

        public Student UpdateStudent(string studentId, string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, string professionalDegreeStr) 
        { 
            ValidateStudent(name, surname, genderStr, birthdayStr, phoneNumber, password, confirmPassword, professionalDegreeStr);
            if (!StudentExists(studentId))
            {
                throw new UserException("Student not found !", UserExceptionType.UserNotFound);
            }
            Student student = GetStudent(studentId);
            student.Name = name;
            student.Surname = surname;
            student.Gender = Enum.Parse<Gender>(genderStr);
            student.Birthday = DateTime.Parse(birthdayStr);
            student.PhoneNumber = phoneNumber;
            student.Password = password;
            student.ProfessionalDegree = Enum.Parse<ProfessionalDegree>(professionalDegreeStr);
            studentRepository.UpdateStudent(studentId, student);
            return student;
        }

        public void DeleteStudent(string studentId) 
        {
            if (!StudentExists(studentId))
            {
                throw new UserException("Student not found !", UserExceptionType.UserNotFound);
            }
            studentRepository.DeleteStudent(studentId);
        }

        public bool ValidateStudent(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, string professionalDegreeStr) 
        {
            Validations.ValidateUser(name, surname, genderStr, birthdayStr, phoneNumber, password, confirmPassword);
            try
            { 
                ProfessionalDegree professionalDegree = Enum.Parse<ProfessionalDegree>(professionalDegreeStr);
            }
            catch (ArgumentException) 
            {
                throw new UserValidationException("Professional Degree is not valid", UserValidationExceptionType.InvalidProfessionalDegree);
            }
            return true;
        }

        public void WithdrawStudentFromCourse(string studentId)
        { 
            Student student = GetStudent(studentId);
            courseService.RemoveStudentFromCourse(studentId, student.EnrolledCourseId);
            student.EnrolledCourseId = -1;
            studentRepository.UpdateStudent(studentId, student);
            List<CourseApplication> studentsPendingApplications = courseApplicationService.GetCourseApplicationsByStudentId(studentId);
            foreach (CourseApplication application in studentsPendingApplications)
            { 
                courseApplicationService.UnpauseCourseApplication(application.Id);
            }
        }

        public void EnrollStudentToCourse(string studentId, int courseId) 
        {
            Student student = GetStudent(studentId);
            student.EnrolledCourseId = courseId;
            studentRepository.UpdateStudent(studentId , student);
            courseService.AddStudentToCourse(studentId , courseId);
            List<CourseApplication> studentPendingApplications = courseApplicationService.GetCourseApplicationsByStudentId(studentId);
            foreach (CourseApplication application in studentPendingApplications)
            {
                courseApplicationService.PauseCourseApplication(application.Id);
            }
        }

        private void WithdrawAllCourseApplications(string studentId)
        { 
            List<CourseApplication> studentPendingApplications = courseApplicationService.GetCourseApplicationsByStudentId(studentId);
            foreach (CourseApplication application in studentPendingApplications)
            { 
                courseApplicationService.DeleteCourseApplication(application.Id);
            }
        }

        public void WithdrawAllExamApplications(string studentId)
        {
            List<ExamApplication> studentPendingApplications = examApplicationService.GetAllExamApplicationsByStudentId(studentId);
            foreach (ExamApplication application in studentPendingApplications)
            {
                examApplicationService.DeleteExamApplication(application.Id);
            }
        }

        public void AssignStudentPenaltyPoint(string studentId, int penaltyPointId)
        {
            if (!StudentExists(studentId))
            { 
                throw new UserException("Student not found !", UserExceptionType.UserNotFound);
            }
            Student student = GetStudent(studentId);
            student.PenaltyPoints.Add(penaltyPointId);
            if (student.PenaltyPoints.Count == 3)
            {
                student.Blocked = true;
                student.EnrolledCourseId = -1;
                WithdrawAllCourseApplications(studentId);
                WithdrawAllExamApplications(studentId);
            }

            studentRepository.UpdateStudent(studentId , student);
        }

        public void DeleteStudentPenaltyPoint(string studentId, int penaltyPointId)
        {
            if (!StudentExists(studentId))
            {
                throw new UserException("Student not found !", UserExceptionType.UserNotFound);
            }

            Student student = GetStudent(studentId);
            student.PenaltyPoints.Remove(penaltyPointId);

            if (student.Blocked == true && student.PenaltyPoints.Count < 3)
            {
                student.Blocked = false;
            }
            studentRepository.UpdateStudent(studentId, student);
        }

        public GradedStudent GradeStudent(string studentId, int grade)
        {
            if (!StudentExists(studentId)) 
            {
                throw new UserException("Student not found !", UserExceptionType.UserNotFound);
            }
            Student student = GetStudent(studentId);
            FinishedCourse finishedCourse = new FinishedCourse(student.EnrolledCourseId, grade);
            student.FinishedCourses.Add(finishedCourse);
            studentRepository.UpdateStudent(studentId, student);

            return new GradedStudent(studentId, student.Name + " " + student.Surname, grade);
        }

        public void GradeStudentsExam(string studentId, ExamResults examResults)
        {
            if (!StudentExists(studentId))
            {
                throw new UserException("Student not found !", UserExceptionType.UserNotFound);
            }
            Student student = GetStudent(studentId);
            student.FinishedExamResults.Add(examResults);
            studentRepository.UpdateStudent(studentId, student);
        }
    }
}
