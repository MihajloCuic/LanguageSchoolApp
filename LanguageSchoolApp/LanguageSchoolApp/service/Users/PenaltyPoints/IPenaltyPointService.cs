using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.service.Users.PenaltyPoints
{
    public interface IPenaltyPointService
    {
        Dictionary<int, PenaltyPoint> GetAllPenaltyPoints();
        List<PenaltyPoint> GetAllPenaltyPointsByIds(List<int> penaltyPointsIds);
        PenaltyPoint GetPenaltyPoint(int id);
        bool PenaltyPointExists(int id);
        void CreatePenaltyPoint(string studentId, PenaltyReason reason);
        void DeletePenaltyPoint(string studentId, int id);
        int GenerateId(string studentId, DateTime receivedDate, PenaltyReason reason);
    }
}
