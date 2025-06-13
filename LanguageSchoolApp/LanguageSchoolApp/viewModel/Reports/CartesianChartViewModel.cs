using LanguageSchoolApp.core;
using LiveCharts;
using System.Linq;

namespace LanguageSchoolApp.viewModel.Reports
{
    public class CartesianChartViewModel : ObservableObject
    {
        private ChartValues<double> _chartValues;
        private string[] _xValues;
        private string _xTitle;
        private string _yTitle;

        public ChartValues<double> ChartValues 
        {
            get { return _chartValues; }
            set 
            {
                _chartValues = value;
                OnPropertyChanged();
            } 
        }
        public string[] XValues 
        {
            get { return _xValues; }
            set 
            { 
                _xValues = value;
                OnPropertyChanged();
            }
        }
        public string XTitle 
        {
            get { return _xTitle; }
            set 
            { 
                _xTitle = value;
                OnPropertyChanged();
            } 
        }
        public string YTitle 
        { 
            get { return _yTitle; }
            set
            {
                _yTitle = value;
                OnPropertyChanged();
            }
        }

        public CartesianChartViewModel(string xTitle, string yTitle, Dictionary<string, double> chartValues) 
        {
            XTitle = xTitle;
            YTitle = yTitle;

            ChartValues = new ChartValues<double>(chartValues.Select(chartValue => chartValue.Value));
            XValues = chartValues.Keys.ToArray();
        }
    }
}
