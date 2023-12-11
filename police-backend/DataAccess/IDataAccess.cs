using police_backend.Models;
using police_backend.ViewModel;

namespace police_backend.DataAccess
{
    public interface IDataAccess
    {
        int CreateUser(User user);
        bool IsEmailAvailable(string email);
        bool IsStationCodeAvailable(string Code);
        bool AuthenticateUser(string email, string password, out User? user);
        IList<User> GetUsers();
        void InsertReport(Report report);
        void InsertStation(Station station);
        void InsertOB(DateData dateData);
        void InsertOfficer(Police police);
        void InsertArrest(Arrest arrest);
        void InsertFinding(Finding finding);
        void InsertCase(CaseOutcome caseOutcome);
        void InsertAssign(Assign assign);
        void InsertCourt(Court court);
        IList<Report> GetAllReports();
        IList<Finding> GetAllFindings();
        IList<Station> GetAllStations();
        IList<CaseOutcomeVM> GetAllCaseOutcomes();
        IList<AssignVM> GetAllAssigns();
        IList<CourtVM> GetAllCourts();
        IList<Rank> GetAllRanks();
        IList<Casetype> GetAllCasetypes();
        IList<Caselist> GetAllCaselists();
        IList<Police> GetAllPolices();
        IList<Test> GetAllTest();
        IList<Arrest> GetAllArrests();
        IList<Witness> GetAllWitnesss();
        IList<Court> GetSpecificCourt();
        IList<Suspect> GetSpecificSuspects(int id);
        IList<Suspect> GetAllSuspects();
        Police GetPolice(int id);
        Rank GetRank(int id);
        Station GetStation(int id);
        Casetype GetCasetype(int id);
        Caselist GetCaselist(int id);
        Report GetReport(int id);
        Arrest GetArrest(int id);
        Finding GetFinding(int id);
        Suspect GetOneSuspect(int id);
        Witness GetOneWitness(int id);
        List<Witness> GetWitness(int id);
        List<Evidence> GetEvidence(int id);
        List<Interview> GetInterview(int id);
        List<Suspect> GetSuspect(int id);
        List<Statement> GetStatement(int id);
        List<CaseListArray> GetCaseListArray(int id);
        List<ArrestItem> GetArrestItem(int id);
        bool DeleteStation(int id);
        bool DeletePolice(int id);
        bool DeleteReport(int id);
        bool DeleteArrest(int id);
        bool DeleteWitness(int id);
        bool DeleteSuspect(int id);
        bool DeleteStatement(int id);
        bool DeleteInterview(int id);
        bool DeleteEvidence(int id);
        bool DeleteFinding(int id);
        bool DeleteItem(int id);
        bool DeleteCaseOutcome(int id);
        bool DeleteAssign(int id);
        bool DeleteCourt(int id);
    }
}
