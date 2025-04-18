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

        private string _buttonContent;
        public string ButtonContent
        {
            get { return _buttonContent; }
            set
            {
                _buttonContent = value;
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

        public RelayCommand<object> Command { get; set; }

        public ActiveCourseDetailsViewModel(Student _student) 
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();

            student = _student;
            course = courseService.GetCourse(student.EnrolledCourseId);
            SetCourseInfo();

            ButtonContent = "Drop out";
            Command = new RelayCommand<object>(Dropout, CanDropout);
        }

        public ActiveCourseDetailsViewModel(Teacher teacher, int courseId)
        {
            courseService = App.ServiceProvider.GetService<ICourseService>();

            course = courseService.GetCourse(courseId);
            SetCourseInfo();
            ButtonContent = "End Course";
            Command = new RelayCommand<object>(FinishCourse, CanFinishCourse);
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

        public bool CanFinishCourse(object? parameter) 
        {
            return DateTime.Now >= course.BeginningDate.AddDays(7 * course.Duration);
        }
        public void FinishCourse(object? parameter) 
        { 
            //TODO: Add opening of special window or change this user control with new uc where teacher grades students and end course
        }
    }
}
