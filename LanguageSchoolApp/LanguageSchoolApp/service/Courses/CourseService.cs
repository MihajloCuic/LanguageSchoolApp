using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model;
using LanguageSchoolApp.repository.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.service.Validation;
using LanguageSchoolApp.exceptions.Courses;

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

        public Course GetCourse(int courseId) 
        { 
            return courseRepository.GetCourse(courseId);
        }

        public void CreateCourse(string languageName, string languageLevelStr, int maxParticipants, 
            int duration, List<KeyValuePair<string, string>> classPeriodsStr, string beginningDateStr, string courseTypeStr, string teacherId) 
        {
            Validations.ValidateCourse(languageName, languageLevelStr, maxParticipants, duration, classPeriodsStr, beginningDateStr, courseTypeStr);
            LanguageLevel languageLevel = Enum.Parse<LanguageLevel>(languageLevelStr);
            LanguageProficiency languageProficiency = new LanguageProficiency(languageName, languageLevel);

            List<ClassPeriod> classPeriods = new List<ClassPeriod>();
            foreach (KeyValuePair<string, string> classPeriodStr in classPeriodsStr) 
            { 
                DaysOfWeek dayOfWeek = Enum.Parse<DaysOfWeek>(classPeriodStr.Key);
                TimeOnly startTime = TimeOnly.Parse(classPeriodStr.Value);
                ClassPeriod classPeriod = new ClassPeriod(dayOfWeek, startTime);
                classPeriods.Add(classPeriod);
            }

            DateTime beginningDate = DateTime.Parse(beginningDateStr);
            CourseType courseType = Enum.Parse<CourseType>(courseTypeStr);

            if (courseType == CourseType.Live) 
            {
                List<Course> matchingCourses = courseRepository.CheckIfCoursesMatch(beginningDate, courseType, classPeriods);
                if (matchingCourses.Count >= 2)
                {
                    throw new CourseException("All classrooms are full", CourseExceptionType.ClassroomsFull);
                }
            }

            int courseId = GenerateId(languageProficiency, beginningDate, courseType, teacherId);
            Course newCourse = new Course(courseId, languageProficiency, maxParticipants, duration, classPeriods, beginningDate, courseType);
            courseRepository.AddCourse(newCourse);
        }

        public void UpdateCourse(int courseId, string languageName, string languageLevelStr, int maxParticipants, 
            int duration, List<KeyValuePair<string, string>> classPeriodsStr, string beginningDateStr, string courseTypeStr) 
        { 
            Validations.ValidateCourse(languageName, languageLevelStr, maxParticipants, duration, classPeriodsStr, beginningDateStr, courseTypeStr);
            LanguageLevel languageLevel = Enum.Parse<LanguageLevel>(languageLevelStr);
            LanguageProficiency languageProficiency = new LanguageProficiency(languageName, languageLevel);

            List<ClassPeriod> classPeriods = new List<ClassPeriod>();
            foreach (KeyValuePair<string, string> classPeriodStr in classPeriodsStr)
            {
                DaysOfWeek dayOfWeek = Enum.Parse<DaysOfWeek>(classPeriodStr.Key);
                TimeOnly startTime = TimeOnly.Parse(classPeriodStr.Value);
                ClassPeriod classPeriod = new ClassPeriod(dayOfWeek, startTime);
                classPeriods.Add(classPeriod);
            }

            DateTime beginningDate = DateTime.Parse(beginningDateStr);
            CourseType courseType = Enum.Parse<CourseType>(courseTypeStr);

            if (courseType == CourseType.Live)
            {
                List<Course> matchingCourses = courseRepository.CheckIfCoursesMatch(beginningDate, courseType, classPeriods);
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
            GetCourse(courseId); //check if course exists else throws exception
            courseRepository.DeleteCourse(courseId);
        }

        public int GenerateId(LanguageProficiency languageProficiency, DateTime beginningDate, CourseType courseType, string teacherId)
        {
            string combination = teacherId + languageProficiency.LanguageName +
                                languageProficiency.LanguageLevel.ToString() +
                                beginningDate.ToString("ddMMyyyy") + courseType.ToString();
            return combination.GetHashCode();
        }
    }
}
