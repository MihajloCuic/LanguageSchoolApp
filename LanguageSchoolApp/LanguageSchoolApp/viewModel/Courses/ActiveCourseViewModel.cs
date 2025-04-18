using Microsoft.Extensions.DependencyInjection;
using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.service.Users.Teachers;
using System.Collections.ObjectModel;
using LanguageSchoolApp.service.Users.PenaltyPoints;
using LanguageSchoolApp.model;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class ActiveCourseViewModel : ObservableObject
    {
        private readonly ICourseService courseService;
        private readonly ITeacherService teacherService;

        public ActiveCourseDetailsViewModel CourseDetailsVM { get; }
        public ActiveCourseScheduleViewModel CourseScheduleVM { get; }
        public object TeacherBubbleVM { get; }
        public object CurrentPenaltyPointView { get; }

        public ActiveCourseViewModel(Student student) 
        { 
            courseService = App.ServiceProvider.GetService<ICourseService>();
            teacherService = App.ServiceProvider.GetService<ITeacherService>();

            Course course = courseService.GetCourse(student.EnrolledCourseId);
            Teacher teacher = teacherService.GetTeacherByCourseId(student.EnrolledCourseId);

            CourseDetailsVM = new ActiveCourseDetailsViewModel(student);
            CourseScheduleVM = new ActiveCourseScheduleViewModel(course);
            TeacherBubbleVM = new ActiveCourseTeacherDetailsViewModel(student, teacher);
            CurrentPenaltyPointView = new ActiveCoursePenaltyPointsViewModel(student);
        }

        public ActiveCourseViewModel(Teacher _teacher, int courseId)
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            teacherService = App.ServiceProvider.GetService<ITeacherService>();

            Course course = courseService.GetCourse(courseId);
            Teacher teacher = _teacher;

            CourseDetailsVM = new ActiveCourseDetailsViewModel(teacher, courseId);
            CourseScheduleVM = new ActiveCourseScheduleViewModel(course);
            TeacherBubbleVM = new ActiveCourseDropoutRequestViewModel(courseId);
            CurrentPenaltyPointView = new ActiveCourseAssignPenaltyPointViewModel(course);
        }
    }
}
