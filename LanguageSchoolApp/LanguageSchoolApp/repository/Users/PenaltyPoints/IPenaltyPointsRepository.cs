using LanguageSchoolApp.model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.repository.Users.PenaltyPoints
{
    public interface IPenaltyPointsRepository
    {
        Dictionary<int, PenaltyPoint> GetAllPenaltyPoints();
        List<PenaltyPoint> GetAllPenaltyPointsByIds(List<int> penaltyPointsIds);
        PenaltyPoint GetPenaltyPoint(int id);
        bool PenaltyPointExists(int id);
        void CreatePenaltyPoint(PenaltyPoint penaltyPoint);
        void UpdatePenaltyPoint(PenaltyPoint penaltyPoint);
        void WriteToFile();
    }
}
