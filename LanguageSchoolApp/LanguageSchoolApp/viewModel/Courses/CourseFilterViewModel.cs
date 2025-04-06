using LanguageSchoolApp.core;
using LanguageSchoolApp.service.Courses;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class CourseFilterViewModel : ObservableObject
    {
        private readonly ICourseService courseService;
        private AvailableCoursesViewModel _availableCoursesViewModel;
        private FinishedCoursesViewModel _finishedCoursesViewModel;
        private TeacherCoursesViewModel _teacherCoursesViewModel;

        private string _languageNameFilter;
        private string _languageLevelFilter;
        private string _courseTypeFilter;
        private bool _languageNameVisible;
        private bool _languageLevelVisible;
        private bool _courseTypeVisible;

        public string LanguageNameFilter
        {
            get { return _languageNameFilter; }
            set
            {
                _languageNameFilter = value;
                OnPropertyChanged();
            }
        }
        public string LanguageLevelFilter
        {
            get { return _languageLevelFilter; }
            set
            {
                _languageLevelFilter = value;
                OnPropertyChanged();
            }
        }
        public string CourseTypeFilter
        {
            get { return _courseTypeFilter; }
            set
            {
                _courseTypeFilter = value;
                OnPropertyChanged();
            }
        }
        public bool LanguageNameVisible
        {
            get { return _languageNameVisible; }
            set
            {
                _languageNameVisible = value;
                OnPropertyChanged();
            }
        }
        public bool LanguageLevelVisible
        {
            get { return _languageLevelVisible; }
            set
            {
                _languageLevelVisible = value;
                OnPropertyChanged();
            }
        }
        public bool CourseTypeVisible
        {
            get { return _courseTypeVisible; }
            set
            {
                _courseTypeVisible = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> LiveButtonCommand { get; set; }
        public RelayCommand<object> OnlineButtonCommand { get; set; }
        public RelayCommand<object> ApplyFiltersCommand { get; set; }
        public RelayCommand<string> CancelFilterCommand { get; set; }

        public CourseFilterViewModel(AvailableCoursesViewModel availableCoursesVM) 
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            _availableCoursesViewModel = availableCoursesVM;

            CourseTypeVisible = false;
            LanguageLevelVisible = false;
            LanguageNameVisible = false;

            LiveButtonCommand = new RelayCommand<object>(SetCourseStyleLive, CanSetCourseTypeLive);
            OnlineButtonCommand = new RelayCommand<object>(SetCourseTypeOnline, CanSetCourseTypeOnline);
            ApplyFiltersCommand = new RelayCommand<object>(ApplyFilters, CanApplyFilters);
            CancelFilterCommand = new RelayCommand<string>(CancelFilter, CanCancelFilter);
        }

        public CourseFilterViewModel(FinishedCoursesViewModel finishedCoursesVM)
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            _finishedCoursesViewModel = finishedCoursesVM;

            CourseTypeVisible = false;
            LanguageLevelVisible = false;
            LanguageNameVisible = false;

            LiveButtonCommand = new RelayCommand<object>(SetCourseStyleLive, CanSetCourseTypeLive);
            OnlineButtonCommand = new RelayCommand<object>(SetCourseTypeOnline, CanSetCourseTypeOnline);
            ApplyFiltersCommand = new RelayCommand<object>(ApplyFilters, CanApplyFilters);
            CancelFilterCommand = new RelayCommand<string>(CancelFilter, CanCancelFilter);
        }

        public CourseFilterViewModel(TeacherCoursesViewModel teacherCoursesVM) 
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            _teacherCoursesViewModel = teacherCoursesVM;

            CourseTypeVisible = false;
            LanguageLevelVisible = false;
            LanguageNameVisible = false;

            LiveButtonCommand = new RelayCommand<object>(SetCourseStyleLive, CanSetCourseTypeLive);
            OnlineButtonCommand = new RelayCommand<object>(SetCourseTypeOnline, CanSetCourseTypeOnline);
            ApplyFiltersCommand = new RelayCommand<object>(ApplyFilters, CanApplyFilters);
            CancelFilterCommand = new RelayCommand<string>(CancelFilter, CanCancelFilter);
        }

        private bool CanSetCourseTypeLive(object? parameter) { return true; }
        private void SetCourseStyleLive(object? parameter) { CourseTypeFilter = "Live"; }
        private bool CanSetCourseTypeOnline(object? parameter) { return true; }
        private void SetCourseTypeOnline(object? parameter) { CourseTypeFilter = "Online"; }

        private bool CanApplyFilters(object? parameter) { return true; }
        private void ApplyFilters(object? parameter)
        {
            if (!string.IsNullOrEmpty(LanguageNameFilter))
            {
                LanguageNameVisible = true;
            }
            if (!string.IsNullOrEmpty(LanguageLevelFilter))
            {
                LanguageLevelVisible = true;
            }
            if (!string.IsNullOrEmpty(CourseTypeFilter))
            {
                CourseTypeVisible = true;
            }

            if (_availableCoursesViewModel != null)
            {
                _availableCoursesViewModel.FilterList(LanguageNameFilter, LanguageLevelFilter, CourseTypeFilter);
            }
            else if (_finishedCoursesViewModel != null)
            {
                _finishedCoursesViewModel.FilterList(LanguageNameFilter, LanguageLevelFilter, CourseTypeFilter);
            }
            else if (_teacherCoursesViewModel != null)
            {
                _teacherCoursesViewModel.FilterList(LanguageNameFilter, LanguageLevelFilter, CourseTypeFilter);
            }
        }

        private bool CanCancelFilter(object? parameter) { return true; }
        private void CancelFilter(string filterType)
        {
            switch (filterType)
            {
                case "languageName":
                    LanguageNameFilter = "";
                    LanguageNameVisible = false;
                    break;
                case "languageLevel":
                    LanguageLevelVisible = false;
                    LanguageLevelFilter = "";
                    break;
                case "courseType":
                    CourseTypeVisible = false;
                    CourseTypeFilter = "";
                    break;
            }

            ApplyFilters(null);
        }
    }
}
