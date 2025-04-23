using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.view.Courses;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class ActiveCourseDropoutRequestViewModel : ObservableObject
    {
        private readonly ICourseDropoutRequestService courseDropoutRequestService;
        private readonly ICourseService courseService;
        private List<CourseDropoutRequest> courseDropoutRequests;
        private ObservableCollection<CourseDropoutRequest> dropoutRequests;
        private readonly Course course;

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

        public ActiveCourseDropoutRequestViewModel(int courseId) 
        { 
            courseDropoutRequestService = App.ServiceProvider.GetService<ICourseDropoutRequestService>();
            courseService = App.ServiceProvider.GetService<ICourseService>();
            course = courseService.GetCourse(courseId);
            courseDropoutRequests = courseDropoutRequestService.GetAllPendingRequestsByCourseId(courseId);
            DropoutRequests = new ObservableCollection<CourseDropoutRequest>(courseDropoutRequests);
            HandleDropoutRequestCommand = new RelayCommand<int>(HandleDropoutRequest, CanHandleDropoutRequest);
        }

        private void UpdateDropoutRequests()
        { 
            courseDropoutRequests = courseDropoutRequestService.GetAllPendingRequestsByCourseId(course.Id);
            DropoutRequests.Clear();
            foreach (var course in courseDropoutRequests) 
            { 
                DropoutRequests.Add(course);
            }
        }

        private bool CanHandleDropoutRequest(int requestId)
        { 
            return DateTime.Now <= course.BeginningDate.AddDays(7 * course.Duration);
        }
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
