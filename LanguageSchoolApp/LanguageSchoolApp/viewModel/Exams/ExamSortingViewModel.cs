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
    public class ExamSortingViewModel : ObservableObject
    {
        private readonly IExamService examService;
        private AvailableExamsViewModel _availableExamsVM;

        private string _examDateSorting;
        private bool _examDatePictureAsc;
        private bool _examDatePictureDesc;

        public string ExamDateSorting
        { 
            get { return _examDateSorting; } 
            set 
            { 
                _examDateSorting = value;
                OnPropertyChanged();
            }
        }
        public bool ExamDatePictureAsc
        { 
            get { return _examDatePictureAsc; }
            set
            { 
                _examDatePictureAsc = value;
                OnPropertyChanged();
            }
        }
        public bool ExamDatePictureDesc
        {
            get { return _examDatePictureDesc; }
            set
            {
                _examDatePictureDesc = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> ApplySortingCommand { get; set; }

        public ExamSortingViewModel(AvailableExamsViewModel availableExamsVM) 
        { 
            examService = App.ServiceProvider.GetService<IExamService>();
            _availableExamsVM = availableExamsVM;

            ExamDateSorting = "None";
            ExamDatePictureAsc = false;
            ExamDatePictureDesc = false;

            ApplySortingCommand = new RelayCommand<object>(Apply, CanApply);
        }

        private bool CanApply(object? parameter) { return true; }
        private void Apply(object parameter)
        {
            ExamDateSorting = SortingType(ExamDateSorting);

            if (_availableExamsVM != null) 
            { 
                _availableExamsVM.SortList(ExamDateSorting);
            }
        }

        private string SortingType(string direction) 
        {
            string newDirection = "";
            if (direction == "None")
            {
                ExamDatePictureDesc = true;
                ExamDatePictureAsc = false;
                newDirection = "Descending";
            }
            if (direction == "Descending")
            {
                ExamDatePictureDesc = false;
                ExamDatePictureAsc = true;
                newDirection = "Ascending";
            }
            if (direction == "Ascending")
            {
                ExamDatePictureDesc = false;
                ExamDatePictureAsc = false;
                newDirection = "None";
            }
            return newDirection;
        }
    }
}
