using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.view.Courses;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class ActiveCourseDetailsViewModel : ObservableObject
    {
        private readonly ICourseService courseService;
        private readonly Course course;
        private readonly Student student;

        private string _languageProficiency;
        private DateTime _beginningDate;
        private string _duration;
        private string _courseType;

        private Visibility _buttonVisibility = Visibility.Visible;
        public Visibility ButtonVisibility
        { 
            get { return _buttonVisibility; }
            set
            {
                _buttonVisibility = value; 
                OnPropertyChanged();
            }
        }

        public string LanguageProficiency
        {
            get { return _languageProficiency; }
            set
            {
                _languageProficiency = value;
                OnPropertyChanged();
            }
        }
        public DateTime BeginningDate
        {
            get { return _beginningDate; }
            set
            {
                _beginningDate = value;
                OnPropertyChanged();
            }
        }
        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }
        public string CourseType
        {
            get { return _courseType; }
            set
            {
                _courseType = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> DropoutCommand { get; set; }

        public ActiveCourseDetailsViewModel(Student _student) 
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();

            student = _student;
            course = courseService.GetCourse(student.EnrolledCourseId);
            SetCourseInfo();

            DropoutCommand = new RelayCommand<object>(Dropout, CanDropout);
        }

        public ActiveCourseDetailsViewModel(Teacher teacher, int courseId)
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();

            course = courseService.GetCourse(courseId);
            SetCourseInfo();
            ButtonVisibility = Visibility.Hidden;
        }

        private void SetCourseInfo()
        {
            LanguageProficiency = course.LanguageProficiency.LanguageName + " " + course.LanguageProficiency.LanguageLevel.ToString();
            BeginningDate = course.BeginningDate;
            Duration = course.Duration.ToString() + " weeks";
            CourseType = course.CourseType.ToString();
        }

        public bool CanDropout(object? parameter)
        {
            return (DateTime.Now - course.BeginningDate).TotalDays >= 7;
        }
        public void Dropout(object? parameter)
        {
            DropoutFormView dropoutForm = new DropoutFormView(student.Email, course.Id);
            dropoutForm.Show();
        }
    }
}
