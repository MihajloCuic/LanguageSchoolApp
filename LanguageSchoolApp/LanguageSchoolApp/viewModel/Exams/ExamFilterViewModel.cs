using LanguageSchoolApp.core;
using LanguageSchoolApp.service.Exams;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.viewModel.Exams
{
    public class ExamFilterViewModel : ObservableObject
    {
        private readonly IExamService examService;
        private AvailableExamsViewModel _availableExamsViewModel;
        private StudentExamsViewModel _studentExamsViewModel;

        private string _languageNameFilter;
        private string _languageLevelFilter;
        private bool _languageNameVisible;
        private bool _languageLevelVisible;

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

        public RelayCommand<object> ApplyFiltersCommand { get; set; }
        public RelayCommand<string> CancelFilterCommand { get; set; }

        public ExamFilterViewModel(AvailableExamsViewModel availableExamsVM) 
        { 
            examService = App.ServiceProvider.GetService<IExamService>();
            _availableExamsViewModel = availableExamsVM;

            LanguageLevelVisible = false;
            LanguageNameVisible = false;

            ApplyFiltersCommand = new RelayCommand<object>(ApplyFilters, CanApplyFilters);
            CancelFilterCommand = new RelayCommand<string>(CancelFilters, CanCancelFilters);
        }

        public ExamFilterViewModel(StudentExamsViewModel studentExamsVM) 
        {
            examService = App.ServiceProvider.GetService<IExamService>();
            _studentExamsViewModel = studentExamsVM;

            LanguageLevelVisible = false;
            LanguageNameVisible = false;

            ApplyFiltersCommand = new RelayCommand<object>(ApplyFilters, CanApplyFilters);
            CancelFilterCommand = new RelayCommand<string>(CancelFilters, CanCancelFilters);
        }

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

            if (_availableExamsViewModel != null)
            {
                _availableExamsViewModel.FilterList(LanguageNameFilter, LanguageLevelFilter);
            }
            else if (_studentExamsViewModel != null)
            {
                _studentExamsViewModel.FilterList(LanguageNameFilter, LanguageLevelFilter);
            }
        }

        private bool CanCancelFilters(object? parameter) { return true; }
        private void CancelFilters(string filterType) 
        {
            switch (filterType) 
            {
                case "languageName":
                    LanguageNameFilter = "";
                    LanguageNameVisible = false;
                    break;
                case "languageLevel":
                    LanguageLevelFilter = "";
                    LanguageLevelVisible = false;
                    break;
            }

            ApplyFilters(null);
        }
    }
}
