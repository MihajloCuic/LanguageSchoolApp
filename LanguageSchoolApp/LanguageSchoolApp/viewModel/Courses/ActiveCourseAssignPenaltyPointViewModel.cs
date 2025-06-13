using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.model;
using LanguageSchoolApp.service.Users.PenaltyPoints;
using LanguageSchoolApp.service.Users.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.view;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class ActiveCourseAssignPenaltyPointViewModel : ObservableObject
    {
        private readonly IPenaltyPointService penaltyPointService;
        private readonly IStudentService studentService;
        private readonly Course course;
        private List<Student> students;
        private Array reasons;
        private string selectedStudentId;
        private PenaltyReason selectedReason;

        public List<Student> Students
        {
            get { return students; }
            set
            {
                students = value;
                OnPropertyChanged();
            }
        }
        public Array Reasons
        {
            get { return reasons; }
            set
            {
                reasons = value;
                OnPropertyChanged();
            }
        }
        public string SelectedStudentId
        {
            get { return selectedStudentId; }
            set
            {
                selectedStudentId = value;
                OnPropertyChanged();
            }
        }
        public PenaltyReason SelectedReason
        {
            get { return selectedReason; }
            set
            {
                selectedReason = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> AssignPenaltyPointCommand { get; set; }

        public ActiveCourseAssignPenaltyPointViewModel(Course _course)
        {
            penaltyPointService = App.ServiceProvider.GetService<IPenaltyPointService>();
            studentService = App.ServiceProvider.GetService<IStudentService>();
            course = _course;
            students = studentService.GetAllStudentsByIds(course.ParticipantsIds);
            Reasons = Enum.GetValues(typeof(PenaltyReason));

            AssignPenaltyPointCommand = new RelayCommand<object>(AssignPenaltyPoint, CanAssignPenaltyPoint);
        }

        private bool CanAssignPenaltyPoint(object? parameter) 
        { 
            return DateTime.Now <= course.BeginningDate.AddDays(7 * course.Duration);
        }
        private void AssignPenaltyPoint(object? parameter)
        {
            if (string.IsNullOrEmpty(SelectedStudentId))
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", "You must select student !");
                errorPopup.Show();
                return;
            }
            if (string.IsNullOrEmpty(SelectedReason.ToString()))
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", "You must select a reason !");
                errorPopup.Show();
                return;
            }

            try
            {
                penaltyPointService.CreatePenaltyPoint(SelectedStudentId, course.Id, SelectedReason);
                PopupMessageView successPopup = new PopupMessageView("SUCCESS", "You assigned penalty point successfully !");
                successPopup.Show();
            }
            catch (PenaltyPointException ex)
            {
                PopupMessageView failedToCreate = new PopupMessageView("ERROR", ex.Text);
                failedToCreate.Show();
            }
        }

    }
}
