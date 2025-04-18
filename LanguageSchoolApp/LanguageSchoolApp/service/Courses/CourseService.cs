using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.repository.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.service.Validation;
using LanguageSchoolApp.exceptions.Courses;
using System.Security.Cryptography;

namespace LanguageSchoolApp.service.Courses
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository courseRepository;

        public CourseService(ICourseRepository repository)
        {
            courseRepository = repository;
        }

        public Dictionary<int, Course> GetAllCourses()
        { 
            return courseRepository.GetAllCourses();
        }

        public List<Course> GetAllCoursesById(List<int> courseIds)
        { 
            return courseRepository.GetAllCoursesById(courseIds);
        }

        public Course GetCourse(int courseId) 
        { 
            return courseRepository.GetCourse(courseId);
        }

        public bool CourseExists(int courseId) 
        { 
            return courseRepository.CourseExists(courseId);
        }

        public List<Course> GetAllAvailableCourses() 
        { 
            return courseRepository.GetAllAvailableCourses();
        }

        public List<Course> GetAllFilteredCourses(List<Course> courses, string languageName, string languageLevelStr, string courseTypeStr)
        {
            LanguageLevel? languageLevel = null;
            if (!string.IsNullOrEmpty(languageLevelStr))
            {
                languageLevel = Enum.Parse<LanguageLevel>(languageLevelStr);
            }
            CourseType? courseType = null;
            if (!string.IsNullOrEmpty(courseTypeStr))
            { 
                courseType = Enum.Parse<CourseType>(courseTypeStr);
            }
            return courseRepository.GetAllFilteredCourses(courses, languageName, languageLevel, courseType);
        }

        public List<Course> SortCourses(List<Course> courses, string beginningDateSortingStr, string durationSortingStr)
        {
            SortingDirection beginningDateSorting = Enum.Parse<SortingDirection>(beginningDateSortingStr);
            SortingDirection durationSorting = Enum.Parse<SortingDirection>(durationSortingStr);
            return courseRepository.SortCourses(courses, beginningDateSorting, durationSorting);
        }

        public Course CreateCourse(string languageName, string languageLevelStr, int maxParticipants, 
            int duration, List<ClassPeriod> classPeriods, DateTime beginningDate, string courseTypeStr, Teacher teacher) 
        {
            Validations.ValidateCourse(languageName, languageLevelStr, maxParticipants, duration, beginningDate, courseTypeStr);
            LanguageLevel languageLevel = Enum.Parse<LanguageLevel>(languageLevelStr);
            LanguageProficiency languageProficiency = new LanguageProficiency(languageName, languageLevel);
            if (!teacher.LanguageProficiencies.Contains(languageProficiency)) 
            {
                throw new CourseException("You don't possess proficiency for this course", CourseExceptionType.InvalidLanguageProficiency);
            }

            CourseType courseType = Enum.Parse<CourseType>(courseTypeStr);
            if (courseType == CourseType.Live) 
            {
                List<Course> matchingCourses = courseRepository.CheckIfCoursesMatch(-1, beginningDate, courseType, classPeriods);
                if (matchingCourses.Count >= 2)
                {
                    throw new CourseException("All classrooms are full", CourseExceptionType.ClassroomsFull);
                }
            }

            int courseId = GenerateId(languageProficiency, beginningDate, courseType, teacher.Email);
            if (CourseExists(courseId)) 
            { 
                throw new CourseException("Same course already exists", CourseExceptionType.CourseAlreadyExists);
            }

            Course newCourse = new Course(courseId, languageProficiency, maxParticipants, duration, classPeriods, beginningDate, courseType);
            courseRepository.AddCourse(newCourse);
            return newCourse;
        }

        public void UpdateCourse(int courseId, string languageName, string languageLevelStr, int maxParticipants, 
            int duration, List<ClassPeriod> classPeriods, DateTime beginningDate, string courseTypeStr) 
        { 
            Validations.ValidateCourse(languageName, languageLevelStr, maxParticipants, duration, beginningDate, courseTypeStr);
            LanguageLevel languageLevel = Enum.Parse<LanguageLevel>(languageLevelStr);
            LanguageProficiency languageProficiency = new LanguageProficiency(languageName, languageLevel);
            CourseType courseType = Enum.Parse<CourseType>(courseTypeStr);

            if (courseType == CourseType.Live)
            {
                List<Course> matchingCourses = courseRepository.CheckIfCoursesMatch(courseId, beginningDate, courseType, classPeriods);
                if (matchingCourses.Count >= 2)
                {
                    throw new CourseException("All classrooms are full", CourseExceptionType.ClassroomsFull);
                }
            }

            Course course = GetCourse(courseId);
            course.LanguageProficiency = languageProficiency;
            course.MaxParticipants = maxParticipants;
            course.Duration = duration;
            course.ClassPeriods = classPeriods;
            course.BeginningDate = beginningDate;
            course.CourseType = courseType;

            courseRepository.UpdateCourse(courseId, course);
        }

        public void DeleteCourse(int courseId) 
        {
            if (!CourseExists(courseId))
            {
                throw new CourseException("Course not found", CourseExceptionType.CourseNotFound);
            }
            courseRepository.DeleteCourse(courseId);
        }

        public void DeleteAllCoursesByIds(List<int> courseIds)
        {
            foreach (int id in courseIds) 
            {
                if (!CourseExists(id)) 
                {
                    throw new CourseException($" Course with id {id} not found !", CourseExceptionType.CourseNotFound);
                }
                courseRepository.DeleteAllCoursesByIds(courseIds);
            }
        }

        public int GenerateId(LanguageProficiency languageProficiency, DateTime beginningDate, CourseType courseType, string teacherId)
        {
            string combination = teacherId + languageProficiency.LanguageName +
                                languageProficiency.LanguageLevel.ToString() +
                                beginningDate.ToString("ddMMyyyy") + courseType.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combination));
                int hashCode = BitConverter.ToInt32(hashBytes, 0);

                return hashCode == int.MinValue ? 0 : Math.Abs(hashCode);
            }
        }

        private FinishedCourseDTO CourseToFinishedCourseDTO(Course course, FinishedCourse finishedCourse)
        {
            return new FinishedCourseDTO(course.Id, course.LanguageProficiency, course.Duration, course.ClassPeriods, course.BeginningDate, course.CourseType, finishedCourse.Grade);
        }

        public List<FinishedCourseDTO> GetFinishedCoursesDTO(List<FinishedCourse> finishedCourses)
        { 
            List<int> courseIds = finishedCourses.Select(fc => fc.CourseId).ToList();
            List<Course> courses = GetAllCoursesById(courseIds);
            var coursesDict = courses.ToDictionary(c => c.Id);

            return finishedCourses.Where(fc => coursesDict.ContainsKey(fc.CourseId))
                                  .Select(fc => CourseToFinishedCourseDTO(coursesDict[fc.CourseId], fc))
                                  .ToList();
        }

        public List<Course> GetTeachersPendingCourses(List<int> allTeacherCoursesIds)
        {
            List<Course> allTeacherCourses = GetAllCoursesById(allTeacherCoursesIds);
            return allTeacherCourses.Where(course => (course.BeginningDate - DateTime.Now).TotalDays >= 7).ToList();
        }

        public List<Course> GetTeacherActiveCourses(List<int> allTeacherCoursesIds)
        {
            List<Course> allTeacherCourses = GetAllCoursesById(allTeacherCoursesIds);
            return allTeacherCourses.Where(course => (course.BeginningDate - DateTime.Now).TotalDays < 7).ToList();
        }

        public void RemoveStudentFromCourse(string studentId, int courseId) 
        {
            if (!CourseExists(courseId)) 
            { 
                throw new CourseException("Course not found !", CourseExceptionType.CourseNotFound);
            }
            Course course = GetCourse(courseId);
            course.ParticipantsIds.Remove(studentId);
            courseRepository.UpdateCourse(courseId, course);
        }

        public void AddStudentToCourse(string studentId, int courseId)
        {
            if (!CourseExists(courseId))
            {
                throw new CourseException("Course not found !", CourseExceptionType.CourseNotFound);
            }
            Course course = GetCourse(courseId);
            course.ParticipantsIds.Add(studentId);
            courseRepository.UpdateCourse(courseId, course);
        }
    }
}
