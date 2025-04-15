using LanguageSchoolApp.service.Courses;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Data;
using LanguageSchoolApp;
using System.Reflection.Metadata;
using LanguageSchoolApp.service.Exams;

namespace LanguageSchoolApp.converter
{
    public class StudentAppliedToStringConverter : IMultiValueConverter
    {
        private ICourseApplicationService _courseApplicationService;
        private IExamApplicationService _examApplicationService;
        private ICourseApplicationService CourseApplicationService => _courseApplicationService ??= App.ServiceProvider.GetService<ICourseApplicationService>();
        private IExamApplicationService ExamApplicationService => _examApplicationService ??= App.ServiceProvider.GetService<IExamApplicationService>();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] == null || values[1] == null)
            {
                return string.Empty;
            }
            string studentId = values[0].ToString();
            int moduleId;
            string moduleType = parameter as string;

            if (values[1] is int)
            {
                moduleId = (int)values[1];
            }
            else if (!int.TryParse(values[1].ToString(), out moduleId))
            {
                return string.Empty;
            }
            if (moduleType == "course")
            {
                int CourseApplicationId = CourseApplicationService.GenerateId(studentId, moduleId);
                return CourseApplicationService.CourseApplicationExists(CourseApplicationId) ? "Withdraw" : "Apply";
            }
            else
            { 
                int ExamApplicationId = ExamApplicationService.GenerateId(studentId, moduleId);
                return ExamApplicationService.ExamApplicationExists(ExamApplicationId) ? "Withdraw" : "Apply";
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}