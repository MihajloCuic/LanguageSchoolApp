using LanguageSchoolApp.core;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.service.Users.PenaltyPoints;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageSchoolApp.viewModel.Courses
{
    public class ActiveCoursePenaltyPointsViewModel : ObservableObject
    {
        private readonly IPenaltyPointService penaltyPointService;
        private ObservableCollection<PenaltyPoint> _penaltyPoints;
        public ObservableCollection<PenaltyPoint> PenaltyPoints
        {
            get { return _penaltyPoints; }
            set
            {
                _penaltyPoints = value;
                OnPropertyChanged();
            }
        }

        public ActiveCoursePenaltyPointsViewModel(Student student) 
        {
            penaltyPointService = App.ServiceProvider.GetService<IPenaltyPointService>();
            List<PenaltyPoint> penaltyPoints = penaltyPointService.GetAllPenaltyPointsByIds(student.PenaltyPoints);
            PenaltyPoints = new ObservableCollection<PenaltyPoint>(penaltyPoints);
        }
    }
}
