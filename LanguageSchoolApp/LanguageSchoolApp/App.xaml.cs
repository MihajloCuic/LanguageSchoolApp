using System.Configuration;
using System.Data;
using System.ServiceProcess;
using System.Windows;
using LanguageSchoolApp.repository.Courses;
using LanguageSchoolApp.repository.Exams;
using LanguageSchoolApp.repository.Notifications;
using LanguageSchoolApp.repository.Users.Directors;
using LanguageSchoolApp.repository.Users.PenaltyPoints;
using LanguageSchoolApp.repository.Users.Students;
using LanguageSchoolApp.repository.Users.Teachers;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.service.Helpers;
using LanguageSchoolApp.service.Notifications;
using LanguageSchoolApp.service.Users.Directors;
using LanguageSchoolApp.service.Users.PenaltyPoints;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.service.Users.Teachers;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageSchoolApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            base.OnStartup(e);
        }

        private void ConfigureServices(ServiceCollection services) 
        {
            services.AddSingleton<IStudentRepository, StudentRepository>();
            services.AddSingleton<IStudentService, StudentService>();

            services.AddSingleton<ITeacherRepository, TeacherRepository>();
            services.AddSingleton<ITeacherService, TeacherService>();

            services.AddSingleton<IDirectorRepository, DirectorRepository>();
            services.AddSingleton<IDirectorService, DirectorService>();

            services.AddSingleton<ICourseRepository, CourseRepository>();
            services.AddSingleton<ICourseService, CourseService>();

            services.AddSingleton<IExamRepository, ExamRepository>();
            services.AddSingleton<IExamService, ExamService>();

            services.AddSingleton<INotificationRepository, NotificationRepository>();
            services.AddSingleton<INotificationService, NotificationService>();

            services.AddSingleton<ICourseApplicationRepository, CourseApplicationRepository>();
            services.AddSingleton<ICourseApplicationService, CourseApplicationService>();

            services.AddSingleton<IExamApplicationRepository, ExamApplicationRepository>();
            services.AddSingleton<IExamApplicationService, ExamApplicationService>();

            services.AddSingleton<IPenaltyPointsRepository, PenaltyPointRepository>();
            services.AddSingleton<IPenaltyPointService, PenaltyPointService>();

            services.AddSingleton<ICourseDropoutRequestRepository, CourseDropoutRequestRepository>();
            services.AddSingleton<ICourseDropoutRequestService, CourseDropoutRequestService>();

            services.AddSingleton<IEmailService, EmailService>();
        }
    }

}
