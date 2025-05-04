using LanguageSchoolApp.core;
using LanguageSchoolApp.model;
using System.Windows;

namespace LanguageSchoolApp.viewModel.Users
{
    public class TeacherFiltersViewModel : ObservableObject
    {
        private ActiveTeachersViewModel activeTeachersViewModel;

        private Array _levels;
        private string _languageName;
        private LanguageLevel _languageLevel;
        private int _grade;

        private Visibility _proficiencyVisibility = Visibility.Collapsed;
        private Visibility _gradeVisibility = Visibility.Collapsed;

        public Array Levels
        { 
            get { return _levels; }
            set 
            { 
                _levels = value; 
                OnPropertyChanged();
            }
        }
        public string LanguageName
        { 
            get { return _languageName; } 
            set 
            { 
                _languageName = value; 
                OnPropertyChanged();
            }
        }
        public LanguageLevel LanguageLevel
        { 
            get { return _languageLevel; }
            set
            {
                _languageLevel = value; 
                OnPropertyChanged();
            }
        }
        public int Grade
        { 
            get { return _grade; }
            set
            { 
                _grade = value;
                OnPropertyChanged();
            }
        }

        public Visibility ProficiencyVisibility 
        { 
            get { return _proficiencyVisibility; }
            set
            { 
                _proficiencyVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility GradeVisibility 
        { 
            get { return _gradeVisibility; }
            set 
            {
                _gradeVisibility = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> FilterCommand { get; set; }
        public RelayCommand<string> CancelFilterCommand { get; set; }

        public TeacherFiltersViewModel(ActiveTeachersViewModel _activeTeachersViewModel) 
        {
            Levels = Enum.GetValues(typeof(LanguageLevel));
            activeTeachersViewModel = _activeTeachersViewModel;
            Grade = -1;

            FilterCommand = new RelayCommand<object>(SetFilter, CanSetFilter);
            CancelFilterCommand = new RelayCommand<string>(CancelFilter, CanCancelFilter);
        }

        private bool CanSetFilter(object? parameter) { return true; }
        private void SetFilter(object? parameter) 
        {
            if (!string.IsNullOrEmpty(LanguageName) && !string.IsNullOrEmpty(LanguageLevel.ToString()))
            { 
                ProficiencyVisibility = Visibility.Visible;
            }
            if (Grade != -1)
            { 
                GradeVisibility = Visibility.Visible;
            }

            if (activeTeachersViewModel != null)
            {
                activeTeachersViewModel.FilterTeachers(LanguageName, LanguageLevel, Grade);
            }
        }

        private bool CanCancelFilter (string filterType) { return true; }
        private void CancelFilter(string filterType) 
        {
            switch (filterType) 
            {
                case "languageProficiency":
                    LanguageName = "";
                    ProficiencyVisibility = Visibility.Collapsed;
                    break;
                case "grade":
                    Grade = -1;
                    GradeVisibility = Visibility.Collapsed;
                    break;
            }

            SetFilter(null);
        }
    }
}
