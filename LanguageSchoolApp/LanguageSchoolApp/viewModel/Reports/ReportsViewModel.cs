using LanguageSchoolApp.core;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.service.Exams;
using LanguageSchoolApp.service.Reports;
using LanguageSchoolApp.service.Users.PenaltyPoints;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.view;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LanguageSchoolApp.viewModel.Reports
{
    public class ReportsViewModel : ObservableObject
    {
        private readonly IPenaltyPointService penaltyPointService;
        private readonly IStudentService studentService;
        private readonly IReportService reportService;

        private CartesianChartViewModel cartesianChart;
        private PieChartViewModel pieChart;
        private string selectedReport;
        private string chart1Description;
        private string chart2Description;
        private Visibility reportDetailsVisibility;

        public CartesianChartViewModel CartesianChart
        { 
            get { return cartesianChart; }
            set
            {
                cartesianChart = value;
                OnPropertyChanged();
            }
        }
        public PieChartViewModel PieChart
        {
            get { return pieChart; }
            set
            {
                pieChart = value;
                OnPropertyChanged();
            }
        }
        public string SelectedReport
        { 
            get { return selectedReport; }
            set
            {
                selectedReport = value;
                OnPropertyChanged();
            }
        }
        public string Chart1Description
        { 
            get { return chart1Description; }
            set 
            { 
                chart1Description = value;
                OnPropertyChanged();
            }
        }
        public string Chart2Description
        {
            get { return chart2Description; }
            set
            {
                chart2Description = value;
                OnPropertyChanged();
            }
        }
        public Visibility ReportDetailsVisibility
        { 
            get { return reportDetailsVisibility; }
            set
            { 
                reportDetailsVisibility = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<string> ChooseReportCommand { get; set; }
        public RelayCommand<object> MailReportCommand { get; set; }

        public ReportsViewModel() 
        {
            penaltyPointService = App.ServiceProvider.GetRequiredService<IPenaltyPointService>();
            studentService = App.ServiceProvider.GetRequiredService<IStudentService>();
            reportService = App.ServiceProvider.GetRequiredService<IReportService>();

            ReportDetailsVisibility = Visibility.Hidden;

            ChooseReportCommand = new RelayCommand<string>(ChooseReport, CanChooseReport);
            MailReportCommand = new RelayCommand<object>(MailReport, CanMailReport);
        }

        private bool CanChooseReport(string reportType) { return true; }
        private void ChooseReport(string reportType) 
        {
            ReportDetailsVisibility = Visibility.Visible;
            SelectedReport = reportType;
            switch (reportType) 
            {
                case "PenaltyPoints":
                    FetchPenaltyPointsReport();
                    break;
                case "CourseGrades":
                    FetchCourseGradesReport();
                    break;
                case "ExamScores":
                    FetchExamScoresReport();
                    break;
                case "LanguageActivity":
                    FetchLanguageActivityReport();
                    break;
                default:
                    PopupMessageView errorMessage = new PopupMessageView("ERROR", "Unknown option. Try again !");
                    errorMessage.Show();
                    break;
            }
        }

        private void FetchPenaltyPointsReport() 
        {
            Chart1Description = "Number of penalty points per course";
            Chart2Description = "Number of students with penalty points";
            LoadCartesianChart(penaltyPointService.PenaltyPointsCourseReport(), "Courses", "Penalty Points");
            LoadPieChart(studentService.PenaltyPointsStudentReport());
        }
        private void FetchCourseGradesReport() 
        {
            Chart1Description = "Average grade per course";
            Chart2Description = "Average dropout statistics";
            LoadCartesianChart(reportService.AverageCourseGradeReport(), "Courses", "Average Grade");
            LoadPieChart(reportService.AverageDropoutReport());
        }
        private void FetchExamScoresReport() 
        {
            Chart1Description = "Average exam scores per exam";
            Chart2Description = "Number of passed / failed exam";
            LoadCartesianChart(reportService.AverageExamScoreReport(), "Exams", "Average Score");
            LoadPieChart(reportService.PassedFailedExamReport());
        }
        private void FetchLanguageActivityReport() 
        {
            Chart1Description = "Courses and exams per language";
            Chart2Description = "Penalty points per language";
            LoadCartesianChart(reportService.AverageLanguageNumberReport(), "Languages", "Courses/Exams");
            LoadPieChart(reportService.AveragePenaltyPointsPerLanguageReport());
        }
        
        private void LoadCartesianChart(Dictionary<string, double> chartValues, string xTitle, string yTitle)
        {
            CartesianChart = new CartesianChartViewModel(xTitle, yTitle, chartValues);
        }

        private void LoadPieChart(Dictionary<string, int> chartValues)
        {
            PieChart = new PieChartViewModel(chartValues);
        }

        private bool CanMailReport(object? parameter) { return true; }
        private void MailReport(object? parameter) { }
    }
}
