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

namespace LanguageSchoolApp.service.Users.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;
        private readonly ICourseApplicationService courseApplicationService;

        public StudentService(IStudentRepository _studentRepository, ICourseApplicationService _courseApplicationService)
        {
            studentRepository = _studentRepository;
            courseApplicationService = _courseApplicationService;
        }

        public Dictionary<string, Student> GetAllStudents() 
        {
            return studentRepository.GetAllStudents();
        }

        public Student GetStudent(string studentId)
        { 
            return studentRepository.GetStudent(studentId);
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
            GetStudent(studentId); //checks if user exists else throws exception "user not found"
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

            List<CourseApplication> studentPendingApplications = courseApplicationService.GetCourseApplicationsByStudentId(studentId);
            foreach (CourseApplication application in studentPendingApplications)
            {
                courseApplicationService.PauseCourseApplication(application.Id);
            }
        }
    }
}
