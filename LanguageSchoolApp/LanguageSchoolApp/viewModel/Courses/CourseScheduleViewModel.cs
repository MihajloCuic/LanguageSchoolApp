using LanguageSchoolApp.core;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Courses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class CourseScheduleViewModel : ObservableObject
    {
        private ObservableCollection<ClassPeriod> _mondayClasses;
        private ObservableCollection<ClassPeriod> _tuesdayClasses;
        private ObservableCollection<ClassPeriod> _wednesdayClasses;
        private ObservableCollection<ClassPeriod> _thursdayClasses;
        private ObservableCollection<ClassPeriod> _fridayClasses;
        private ObservableCollection<ClassPeriod> _saturdayClasses;
        private ObservableCollection<ClassPeriod> _sundayClasses;
        private List<ClassPeriod> _allClasses;

        public ObservableCollection<ClassPeriod> MondayClasses
        { 
            get { return  _mondayClasses; }
            set 
            { 
                _mondayClasses = value; 
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> TuesdayClasses
        {
            get { return _tuesdayClasses; }
            set
            {
                _tuesdayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> WednesdayClasses
        {
            get { return _wednesdayClasses; }
            set
            {
                _wednesdayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> ThursdayClasses
        {
            get { return _thursdayClasses; }
            set
            {
                _thursdayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> FridayClasses
        {
            get { return _fridayClasses; }
            set
            {
                _fridayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> SaturdayClasses
        {
            get { return _saturdayClasses; }
            set
            {
                _saturdayClasses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassPeriod> SundayClasses
        {
            get { return _sundayClasses; }
            set
            {
                _sundayClasses = value;
                OnPropertyChanged();
            }
        }

        public CourseScheduleViewModel(List<ClassPeriod> allClasses) 
        { 
            _allClasses = allClasses;
            MondayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Monday)));
            TuesdayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Tuesday)));
            WednesdayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Wednesday)));
            ThursdayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Thursday)));
            FridayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Friday)));
            SaturdayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Saturday)));
            SundayClasses = new ObservableCollection<ClassPeriod>(_allClasses.Where(period => period.DayOfWeek.Equals(DaysOfWeek.Sunday)));
        }
    }
}
