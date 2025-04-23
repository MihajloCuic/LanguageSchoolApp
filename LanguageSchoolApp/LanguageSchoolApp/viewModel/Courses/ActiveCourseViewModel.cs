using Microsoft.Extensions.DependencyInjection;
using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.service.Users.Teachers;
using System.Collections.ObjectModel;
using LanguageSchoolApp.service.Users.PenaltyPoints;
using LanguageSchoolApp.model;
using LanguageSchoolApp.view.Courses;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class ActiveCourseViewModel : ObservableObject
    {
        private readonly ICourseService courseService;
        private readonly ITeacherService teacherService;
        private object _courseBubbleBM;
        private Course course;

        public ActiveCourseScheduleViewModel CourseScheduleVM { get; set; }
        public object CourseBubbleVM 
        { 
            get { return _courseBubbleBM; }
            set
            { 
                _courseBubbleBM = value;
                OnPropertyChanged();
            }
        }
        public object TeacherBubbleVM { get; set; }
        public object CurrentPenaltyPointView { get; set; }
        public Action<string> SwitchToTeacherCourses { get; set; }

        public ActiveCourseViewModel(Student student) 
        { 
            courseService = App.ServiceProvider.GetService<ICourseService>();
            teacherService = App.ServiceProvider.GetService<ITeacherService>();

            course = courseService.GetCourse(student.EnrolledCourseId);
            Teacher teacher = teacherService.GetTeacherByCourseId(student.EnrolledCourseId);

            CourseBubbleVM = new ActiveCourseDetailsViewModel(student);
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

            CourseBubbleVM = new ActiveCourseDetailsViewModel(teacher, courseId);
            if (CourseBubbleVM is ActiveCourseDetailsViewModel courseBubbleVM)
            {
                courseBubbleVM.EndCourseAction = ShowFinishCourseBubble;
            }
            CourseScheduleVM = new ActiveCourseScheduleViewModel(course);
            TeacherBubbleVM = new ActiveCourseDropoutRequestViewModel(courseId);
            CurrentPenaltyPointView = new ActiveCourseAssignPenaltyPointViewModel(course);
        }

        private void ShowFinishCourseBubble(Course _course, Teacher teacher)
        {
            CourseBubbleVM = new FinishActiveCourseViewModel(_course, teacher);
            if (CourseBubbleVM is FinishActiveCourseViewModel courseBubbleVM)
            {
                courseBubbleVM.CancelGrading = CancelGrading;
                courseBubbleVM.SwitchToTeacherCourses = SwitchToTeacherCourses;
            }
        }

        private void CancelGrading(Teacher _teacher, int _courseId)
        {
            CourseBubbleVM = new ActiveCourseDetailsViewModel(_teacher, _courseId);
            if (CourseBubbleVM is ActiveCourseDetailsViewModel courseBubbleVM)
            {
                courseBubbleVM.EndCourseAction = ShowFinishCourseBubble;
            }
        }
    }
}
