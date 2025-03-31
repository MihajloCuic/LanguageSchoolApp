using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.service.Courses;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class AvailableCoursesViewModel : ObservableObject
    {
        private readonly ICourseService courseService;

        private ObservableCollection<Course> _availableCourses;
        private List<Course> _allAvailableCourses;
        private int _pageNumber;

        public ObservableCollection<Course> AvailableCourses
        { 
            get { return  _availableCourses; }
            set
            { 
                _availableCourses = value;
                OnPropertyChanged();
            }
        }
        public int PageNumber
        {
            get { return _pageNumber; }
            set 
            { 
                _pageNumber = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ApplyCommand { get; set; }
        public RelayCommand ScheduleCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }

        public AvailableCoursesViewModel() 
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();
            PageNumber = 1;
            _allAvailableCourses = courseService.GetAllAvailableCourses();
            AvailableCourses = new ObservableCollection<Course>(GetSlicedAvailableCourses());
            

            ApplyCommand = new RelayCommand(Apply, CanApply);
            ScheduleCommand = new RelayCommand(SeeSchedule, CanSeeSchedule);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand(NextPage, CanNextPage);
        }

        private List<Course> GetSlicedAvailableCourses() 
        { 
            int elementsToSkip = (PageNumber - 1) * 6;
            return _allAvailableCourses.Skip(elementsToSkip).Take(6).ToList();
        }

        private void Apply(object? parameter) 
        { 
            //TODO: Create Course Application and Cancel Application
        }

        private bool CanApply(object? parameter) { return true; }

        private void SeeSchedule(object? parameter) 
        { 
            //TODO: Create Schedule display
        }

        private bool CanSeeSchedule(object? parameter) { return true; }

        private void NextPage(object? parameter) 
        {
            PageNumber++;
            AvailableCourses.Clear();
            foreach (var course in GetSlicedAvailableCourses()) 
            { 
                AvailableCourses.Add(course);
            }
        }
        private bool CanNextPage(object? parameter) 
        { 
            return PageNumber < _allAvailableCourses.Count/6; 
        }

        private void PreviousPage(object? parameter) 
        { 
            PageNumber--;
            AvailableCourses.Clear();
            foreach (var course in GetSlicedAvailableCourses())
            {
                AvailableCourses.Add(course);
            }
        }
        private bool CanPreviousPage(object? parameter) 
        { 
            return PageNumber > 1; 
        }
    }
}
