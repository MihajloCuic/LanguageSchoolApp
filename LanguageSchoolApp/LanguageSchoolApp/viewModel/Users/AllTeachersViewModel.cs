using LanguageSchoolApp.core;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Users.Teachers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;


namespace LanguageSchoolApp.viewModel.Users
{
    public class AllTeachersViewModel : ObservableObject
    {
        private readonly ITeacherService teacherService;

        public TeacherFiltersViewModel Filters { get; }
        public CancelTeacherFiltersViewModel CancelFilters { get; }
        public SortingTeachersViewModel Sorting { get; }

        private List<Teacher> allTeachers;
        private ObservableCollection<Teacher> _teachers;
        private int _pageNumber;

        public ObservableCollection<Teacher> Teachers
        {
            get { return _teachers; }
            set
            {
                _teachers = value;
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

        public RelayCommand<object> PreviousPageCommand { get; set; }
        public RelayCommand<object> NextPageCommand { get; set; }

        public AllTeachersViewModel()
        {
            teacherService = App.ServiceProvider.GetService<ITeacherService>();
            PageNumber = 1;

            allTeachers = teacherService.GetAllTeachers().Select(teacher => teacher.Value).ToList();
            Teachers = new ObservableCollection<Teacher>(GetSlicedTeachersList());

            PreviousPageCommand = new RelayCommand<object>(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand<object>(NextPage, CanNextPage);
        }

        public List<Teacher> GetSlicedTeachersList() 
        {
            int elementsToSkip = PageNumber * 6;
            return allTeachers.Skip(elementsToSkip).Take(6).ToList();
        }

        public void FilterTeachers(string languageName, LanguageLevel languageLevel, int grade) { }
        public void SortList(string name, string grade) { }

        private bool CanNextPage(object? parameter) { return PageNumber < (double)allTeachers.Count / 6; }
        private void NextPage(object? parameter)
        {
            PageNumber++;
            Teachers.Clear();
            foreach (var course in GetSlicedTeachersList())
            {
                Teachers.Add(course);
            }
        }

        private bool CanPreviousPage(object? parameter) { return PageNumber > 1; }
        private void PreviousPage(object? parameter)
        {
            PageNumber--;
            Teachers.Clear();
            foreach (var course in GetSlicedTeachersList())
            {
                Teachers.Add(course);
            }
        }
    }
}
