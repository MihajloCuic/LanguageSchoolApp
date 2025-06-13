using LanguageSchoolApp.core;
using LanguageSchoolApp.exceptions.Courses;
using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.service.Users.PenaltyPoints;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.view;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls.Primitives;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class HandleDropoutRequestViewModel : ObservableObject
    {
        private readonly ICourseDropoutRequestService dropoutRequestService;
        private readonly IPenaltyPointService penaltyPointService;
        private readonly IStudentService studentService;

        private CourseDropoutRequest request;
        private string _name;
        private DropoutReason _reason;
        private string _details;

        public string Name
        { 
            get {  return _name; }
            set 
            { 
                _name = value; 
                OnPropertyChanged();
            }
        }
        public DropoutReason Reason
        { 
            get { return _reason; }
            set
            {
                _reason = value; 
                OnPropertyChanged();
            }
        }
        public string Details
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged();
            }

        }

        public RelayCommand<object> AcceptCommand { get; set; }
        public RelayCommand<object> RejectCommand { get; set; }

        public Action CloseAction { get; set; }
        public Action UpdateRequestList { get; set; }

        public HandleDropoutRequestViewModel(int requestId) 
        { 
            dropoutRequestService = App.ServiceProvider.GetService<ICourseDropoutRequestService>();
            penaltyPointService = App.ServiceProvider.GetService<IPenaltyPointService>();
            studentService = App.ServiceProvider.GetService<IStudentService>();

            request = dropoutRequestService.GetRequest(requestId);
            Student student = studentService.GetStudent(request.StudentId);
            Name = student.Name + " " + student.Surname;
            Reason = request.Reason;
            Details = request.Details;

            AcceptCommand = new RelayCommand<object>(AcceptRequest, CanAcceptRequest);
            RejectCommand = new RelayCommand<object>(RejectRequest, CanRejectRequest);
        }

        private bool CanAcceptRequest(object? parameter) { return true; }
        private void AcceptRequest(object? parameter) 
        {
            try 
            {
                dropoutRequestService.ProcessDropoutRequest(request.Id);
                studentService.WithdrawStudentFromCourse(request.StudentId);

                PopupMessageView successPopup = new PopupMessageView("SUCCESS", "Student withdrawn from course successfully !");
                successPopup.Show();
            }
            catch (CourseDropoutRequestException ex)
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", ex.Text);
                errorPopup.Show();
            }
            catch (UserException ex)
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", ex.Text);
                errorPopup.Show();
            }

            UpdateRequestList();
            CloseAction();
        }

        private bool CanRejectRequest(object? parameter) { return true; }
        private void RejectRequest(object? parameter) 
        {
            try
            {
                dropoutRequestService.ProcessDropoutRequest(request.Id);
                studentService.WithdrawStudentFromCourse(request.StudentId);
                penaltyPointService.CreatePenaltyPoint(request.StudentId, request.CourseId, PenaltyReason.CourseDropout);

                PopupMessageView successPopup = new PopupMessageView("SUCCESS", "Student withdrawn from course successfully !");
                successPopup.Show();
            } 
            catch (CourseDropoutRequestException ex)
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", ex.Text);
                errorPopup.Show();
            }
            catch (UserException ex)
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", ex.Text);
                errorPopup.Show();
            }
            catch (PenaltyPointException ex)
            {
                PopupMessageView errorPopup = new PopupMessageView("ERROR", ex.Text);
                errorPopup.Show();
            }

            UpdateRequestList();
            CloseAction();
        }
    }
}
