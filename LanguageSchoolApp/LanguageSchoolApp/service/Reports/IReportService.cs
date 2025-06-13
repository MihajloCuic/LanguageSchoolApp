using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.service.Reports
{
    public interface IReportService
    {
        Dictionary<string, double> AverageCourseGradeReport();
        Dictionary<string, int> AverageDropoutReport();
        Dictionary<string, int> PassedFailedExamReport();
        Dictionary<string, double> AverageExamScoreReport();
        Dictionary<string, double> AverageLanguageNumberReport();
        Dictionary<string, int> AveragePenaltyPointsPerLanguageReport();
    }
}
