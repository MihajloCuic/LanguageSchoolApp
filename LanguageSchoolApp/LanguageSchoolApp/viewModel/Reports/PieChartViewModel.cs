using LanguageSchoolApp.core;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.viewModel.Reports
{
    public class PieChartViewModel : ObservableObject
    {
        private SeriesCollection _chartValues;

        public SeriesCollection ChartValues
        { 
            get { return _chartValues; }
            set
            {
                _chartValues = value;
                OnPropertyChanged();
            }
        }

        public PieChartViewModel(Dictionary<string, int> reportData) 
        {
            SetupPieChart(reportData);
        }

        private void SetupPieChart(Dictionary<string, int> reportData)
        {
            ChartValues = new SeriesCollection();
            foreach (var item in reportData)
            {

                ChartValues.Add(new PieSeries {
                                        Title = item.Key,
                                        Values = new ChartValues<ObservableValue> { new ObservableValue(item.Value) },
                                        DataLabels = true
                                });
            }
        }
    }
}
