using LanguageSchoolApp.core;
using LanguageSchoolApp.viewModel.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LanguageSchoolApp.viewModel.Users
{
    public class SortingTeachersViewModel : ObservableObject
    {
        private ActiveTeachersViewModel activeTeachersViewModel;

        private string _nameSorting;
        private string _gradeSorting;
        private Visibility _namePictureAsc = Visibility.Hidden;
        private Visibility _namePictureDesc = Visibility.Hidden;
        private Visibility _gradePictureAsc = Visibility.Hidden;
        private Visibility _gradePictureDesc = Visibility.Hidden;

        public string NameSorting
        { 
            get { return _nameSorting; }
            set 
            { 
                _nameSorting = value;
                OnPropertyChanged();
            }
        }
        public string GradeSorting
        {
            get { return _gradeSorting; }
            set
            {
                _gradeSorting = value;
                OnPropertyChanged();
            }
        }
        public Visibility NamePictureAsc
        {
            get { return _namePictureAsc; }
            set
            {
                _namePictureAsc = value;
                OnPropertyChanged();
            }
        }
        public Visibility NamePictureDesc
        {
            get { return _namePictureDesc; }
            set
            {
                _namePictureDesc = value;
                OnPropertyChanged();
            }
        }
        public Visibility GradePictureAsc
        {
            get { return _gradePictureAsc; }
            set
            {
                _gradePictureAsc = value;
                OnPropertyChanged();
            }
        }
        public Visibility GradePictureDesc
        {
            get { return _gradePictureDesc; }
            set
            {
                _gradePictureDesc = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<string> ApplySortingCommand { get; set; }

        public SortingTeachersViewModel(ActiveTeachersViewModel activeTeachersVM) 
        {
            activeTeachersViewModel = activeTeachersVM;

            NameSorting = "None";
            GradeSorting = "None";
            
            ApplySortingCommand = new RelayCommand<string>(ApplySorting, CanApplySorting);
        }

        private bool CanApplySorting(string sortingType) { return true; }
        private void ApplySorting(string sortingType) 
        {
            if (sortingType == "name")
            {
                NameSorting = SortingType(NameSorting, sortingType);

            }
            else if (sortingType == "grade")
            {
                GradeSorting = SortingType(GradeSorting, sortingType);
            }
            if (activeTeachersViewModel != null)
            {
                activeTeachersViewModel.SortList(NameSorting, GradeSorting);
            }
        }

        private string SortingType(string direction, string sortingType) 
        {
            string newDirection = "";
            if (direction == "None" && sortingType == "name")
            {
                NamePictureDesc = Visibility.Hidden;
                NamePictureAsc = Visibility.Visible;
                newDirection = "Ascending";
            }
            if (direction == "None" && sortingType == "grade")
            {
                GradePictureDesc = Visibility.Hidden;
                GradePictureAsc = Visibility.Visible;
                newDirection = "Ascending";
            }
            if (direction == "Descending" && sortingType == "name")
            {
                NamePictureDesc = Visibility.Hidden;
                NamePictureAsc = Visibility.Hidden;
                newDirection = "None";
            }
            if (direction == "Descending" && sortingType == "grade")
            {
                GradePictureDesc = Visibility.Hidden;
                GradePictureAsc = Visibility.Hidden;
                newDirection = "None";
            }
            if (direction == "Ascending" && sortingType == "name")
            {
                NamePictureDesc = Visibility.Visible;
                NamePictureAsc = Visibility.Hidden;
                newDirection = "Descending";
            }
            if (direction == "Ascending" && sortingType == "grade")
            {
                GradePictureDesc = Visibility.Visible;
                GradePictureAsc = Visibility.Hidden;
                newDirection = "Descending";
            }
            return newDirection;
        }
    }
}
