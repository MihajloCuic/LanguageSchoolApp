using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.view.Courses;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class ActiveCourseDropoutRequestViewModel : ObservableObject
    {
        private readonly ICourseDropoutRequestService courseDropoutRequestService;

        private List<CourseDropoutRequest> courseDropoutRequests;
        private ObservableCollection<CourseDropoutRequest> dropoutRequests;
        private int courseId;

        public ObservableCollection<CourseDropoutRequest> DropoutRequests
        { 
            get { return dropoutRequests; }
            set
            { 
                dropoutRequests = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<int> HandleDropoutRequestCommand { get; set; }

        public ActiveCourseDropoutRequestViewModel(int _courseId) 
        { 
            courseDropoutRequestService = App.ServiceProvider.GetService<ICourseDropoutRequestService>();
            courseId = _courseId;
            courseDropoutRequests = courseDropoutRequestService.GetAllPendingRequestsByCourseId(courseId);
            DropoutRequests = new ObservableCollection<CourseDropoutRequest>(courseDropoutRequests);
            HandleDropoutRequestCommand = new RelayCommand<int>(HandleDropoutRequest, CanHandleDropoutRequest);
        }

        private void UpdateDropoutRequests()
        { 
            courseDropoutRequests = courseDropoutRequestService.GetAllPendingRequestsByCourseId(courseId);
            DropoutRequests.Clear();
            foreach (var course in courseDropoutRequests) 
            { 
                DropoutRequests.Add(course);
            }
        }

        private bool CanHandleDropoutRequest(int requestId) { return true; }
        private void HandleDropoutRequest(int requestId) 
        {
            HandleDropoutRequestView window = new HandleDropoutRequestView();
            HandleDropoutRequestViewModel handleDropoutRequestVM = new HandleDropoutRequestViewModel(requestId);
            handleDropoutRequestVM.UpdateRequestList = UpdateDropoutRequests;
            window.DataContext = handleDropoutRequestVM;
            window.SetAction();
            window.Show();
        }
    }
}
