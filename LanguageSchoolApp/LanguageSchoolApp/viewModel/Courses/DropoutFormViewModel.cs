using LanguageSchoolApp.core;
using LanguageSchoolApp.exceptions.Courses;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.view;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class DropoutFormViewModel : ObservableObject
    {
        private readonly ICourseDropoutRequestService courseDropoutRequestService;
        private string studentId;
        private int courseId;
        private string _reasonDetails;
        public string ReasonDetails
        { 
            get { return _reasonDetails; } 
            set 
            { 
                _reasonDetails = value; 
                OnPropertyChanged();
            }
        }

        public DropoutReason Reason { get; set; }
        public RelayCommand<string> DropoutReasonCommand { get; set; }
        public RelayCommand<object> DropoutRequestCommand { get; set; }
        public Action CloseAction { get; set; }

        public DropoutFormViewModel(string _studentId, int _courseId) 
        { 
            courseDropoutRequestService = App.ServiceProvider.GetService<ICourseDropoutRequestService>();
            studentId = _studentId;
            courseId = _courseId;
            Reason = DropoutReason.CourseDifficult;
            DropoutReasonCommand = new RelayCommand<string>(SetReason, CanSetReason);
            DropoutRequestCommand = new RelayCommand<object>(SendDropoutRequest, CanSendDropoutRequest);
        }

        private bool CanSetReason(string reason) { return true; }
        private void SetReason(string reason)
        {
           Reason = Enum.Parse<DropoutReason>(reason);
        }

        private bool CanSendDropoutRequest(object? parameter) { return true; }
        private void SendDropoutRequest(object? parameter)
        {
            if (ReasonDetails == null) 
            {
                ReasonDetails = "";
            }
            try
            {
                courseDropoutRequestService.CreateDropoutRequest(studentId, courseId, Reason, ReasonDetails);
                PopupMessageView popup = new PopupMessageView("SUCCESS", "Dropout request submitted successfully !");
                popup.Show();
            }
            catch(CourseDropoutRequestException ex) 
            {
                PopupMessageView popup = new PopupMessageView("ERROR", ex.Text);
                popup.Show();
            }
            
            CloseAction();
        }
    }
}
