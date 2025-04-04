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
    public class CourseSortingViewModel : ObservableObject
    {
        private readonly ICourseService courseService;
        private AvailableCoursesViewModel _availableCoursesViewModel;
        private FinishedCoursesViewModel _finishedCoursesViewModel;

        private string _beginningDateSorting;
        private string _durationSorting;
        private bool _beginningDatePictureAsc;
        private bool _beginningDatePictureDesc;
        private bool _durationPictureAsc;
        private bool _durationPictureDesc;

        public string BeginningDateSorting
        {
            get { return _beginningDateSorting; }
            set
            {
                _beginningDateSorting = value;
                OnPropertyChanged();
            }
        }
        public string DurationSorting
        {
            get { return _durationSorting; }
            set
            {
                _durationSorting = value;
                OnPropertyChanged();
            }
        }
        public bool BeginningDatePictureAsc
        {
            get { return _beginningDatePictureAsc; }
            set
            {
                _beginningDatePictureAsc = value;
                OnPropertyChanged();
            }
        }
        public bool BeginningDatePictureDesc
        {
            get { return _beginningDatePictureDesc; }
            set
            {
                _beginningDatePictureDesc = value;
                OnPropertyChanged();
            }
        }
        public bool DurationPictureAsc
        {
            get { return _durationPictureAsc; }
            set
            {
                _durationPictureAsc = value;
                OnPropertyChanged();
            }
        }
        public bool DurationPictureDesc
        {
            get { return _durationPictureDesc; }
            set
            {
                _durationPictureDesc = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<string> ApplySortingCommand { get; set; }

        public CourseSortingViewModel(AvailableCoursesViewModel availableCoursesVM) 
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            _availableCoursesViewModel = availableCoursesVM;

            BeginningDateSorting = "None";
            DurationSorting = "None";
            BeginningDatePictureAsc = false;
            BeginningDatePictureDesc = false;
            DurationPictureAsc = false;
            DurationPictureDesc = false;

            ApplySortingCommand = new RelayCommand<string>(ApplySorting, CanApplySorting);
        }

        public CourseSortingViewModel(FinishedCoursesViewModel finishedCoursesVM) 
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            _finishedCoursesViewModel = finishedCoursesVM;

            BeginningDateSorting = "None";
            DurationSorting = "None";
            BeginningDatePictureAsc = false;
            BeginningDatePictureDesc = false;
            DurationPictureAsc = false;
            DurationPictureDesc = false;

            ApplySortingCommand = new RelayCommand<string>(ApplySorting, CanApplySorting);
        }

        private bool CanApplySorting(object? parameter) { return true; }
        private void ApplySorting(string sortingType)
        {
            if (sortingType == "beginningDate")
            {
                BeginningDateSorting = SortingType(BeginningDateSorting, sortingType);

            }
            else if (sortingType == "duration")
            {
                DurationSorting = SortingType(DurationSorting, sortingType);
            }
            if (_availableCoursesViewModel != null)
            {
                _availableCoursesViewModel.SortList(BeginningDateSorting, DurationSorting);
            }
            else if (_finishedCoursesViewModel != null) 
            {
                _finishedCoursesViewModel.SortList(BeginningDateSorting, DurationSorting);
            }
        }

        private string SortingType(string direction, string sortingType)
        {
            string newDirection = "";
            if (direction == "None" && sortingType == "beginningDate")
            {
                BeginningDatePictureDesc = true;
                BeginningDatePictureAsc = false;
                newDirection = "Descending";
            }
            if (direction == "None" && sortingType == "duration")
            {
                DurationPictureDesc = true;
                DurationPictureAsc = false;
                newDirection = "Descending";
            }
            if (direction == "Descending" && sortingType == "beginningDate")
            {
                BeginningDatePictureDesc = false;
                BeginningDatePictureAsc = true;
                newDirection = "Ascending";
            }
            if (direction == "Descending" && sortingType == "duration")
            {
                DurationPictureDesc = false;
                DurationPictureAsc = true;
                newDirection = "Ascending";
            }
            if (direction == "Ascending" && sortingType == "beginningDate")
            {
                BeginningDatePictureDesc = false;
                BeginningDatePictureAsc = false;
                newDirection = "None";
            }
            if (direction == "Ascending" && sortingType == "duration")
            {
                DurationPictureDesc = false;
                DurationPictureAsc = false;
                newDirection = "None";
            }
            return newDirection;
        }
    }
}
