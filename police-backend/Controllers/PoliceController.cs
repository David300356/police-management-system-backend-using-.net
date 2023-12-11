using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using police_backend.DataAccess;
using police_backend.Models;

namespace police_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliceController : Controller
    {
        private readonly IDataAccess library;
        private readonly IConfiguration configuration;
        public PoliceController(IDataAccess library, IConfiguration configuration = null)
        {
            this.library = library;
            this.configuration = configuration;
        }

        [HttpPost("CreateAccount")]
        public IActionResult CreateAccount(User user)
        {
            if (!library.IsEmailAvailable(user.Email))
            {
                return Ok("Email is not available!");
            }
            user.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            user.UserType = UserType.USER;
            library.CreateUser(user);
            return Ok("Account created successfully!");
        }

        [HttpGet("Login")]
        public IActionResult Login(string email, string password)
        {
            if (library.AuthenticateUser(email, password, out User? user))
            {
                if (user != null)
                {
                    var jwt = new Jwt(configuration["Jwt:Key"], configuration["Jwt:Duration"]);
                    var token = jwt.GenerateToken(user);
                    return Ok(token);
                }
            }
            return Ok("Invalid");
        }
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = library.GetUsers();
            var result = users.Select(user => new
            {
                user.id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Mobile,
                user.Blocked,
                user.Active,
                user.CreatedOn,
                user.UserType,
                user.Fine
            });
            return Ok(result);
        }
        [HttpPost("InsertOB")]
        public IActionResult InsertOB(DateData dateData)
        {
            dateData.OB = DateTime.Now.ToString("dd/MM/yyyy");
            dateData.CreatedOn = DateTime.Now.ToString("dd/MM/yyyy");
            library.InsertOB(dateData);
            return Ok("Inserted");
        }
        [HttpPost("InsertStation")]
        public IActionResult InsertStation(Station station)
        {
            if (library.IsStationCodeAvailable(station.code))
            {
                return Ok("Code already exits!");
            }
            station.name = station.name.ToLower();
            station.phone = station.phone.ToLower();
            station.city = station.city.ToLower();
            station.address = station.address.ToLower();
            station.code = station.code.ToLower();
            station.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            library.InsertStation(station);
            return Ok("Inserted");
        }
        [HttpPost("InsertPolice")]
        public IActionResult InsertPolice(Police police)
        {
            police.name = police.name.ToLower();
            police.phone = police.phone;
            police.email = police.email.ToLower();
            police.serialNumber = police.serialNumber;
            police.idNumber = police.idNumber;
            police.RankId = police.RankId;
            police.StationId = police.StationId;
            police.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            library.InsertOfficer(police);
            return Ok("Inserted");
        }
        [HttpPost("InsertCase")]
        public IActionResult InsertCase(CaseOutcome caseOutcome)
        {
            caseOutcome.ReportId = caseOutcome.ReportId;
            caseOutcome.OutcomeId = caseOutcome.OutcomeId;
            caseOutcome.isClosed = caseOutcome.isClosed;
            caseOutcome.createdOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            library.InsertCase(caseOutcome);
            return Ok("Inserted");
        }
        [HttpPost("InsertCourt")]
        public IActionResult InsertCourt(Court court)
        {
            court.Name = court.Name;
            court.CourtDate = court.CourtDate;
            court.DocketNo = court.DocketNo;
            court.ReportId = court.ReportId;
            library.InsertCourt(court);
            return Ok("Inserted");
        }
        [HttpPost("InsertAssign")]
        public IActionResult InsertAssign(Assign assign)
        {
            assign.ReportId = assign.ReportId;
            assign.PoliceId = assign.PoliceId;
            assign.createdOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            library.InsertAssign(assign);
            return Ok("Inserted");
        }
        [HttpPost("InsertReport")]
        public IActionResult InsertReport(Report report)
        {
            report.ob = Guid.NewGuid().ToString();
            report.name = report.name.Trim();
            report.phone = report.phone.Trim();
            report.email = report.email.Trim();
            report.occupation = report.occupation.Trim();
            report.city = report.city.Trim();
            report.address = report.address.Trim();
            report.idNumber = report.idNumber.Trim();
            report.CreatedOn = DateTime.Now.ToString("dd/MM/yyyy");
            report.takeFingerprint = report.takeFingerprint;
            report.CasetypeId = report.CasetypeId;
            report.PoliceId = report.PoliceId;
            for(int i = 0; i < report.caseList.Count(); i++)
            {
                report.caseList[i] = report.caseList[i];
            }

            for (int i = 0; i < report.Witnesses.Count(); i++)
            {
                report.Witnesses[i].name = report.Witnesses[i].name.ToLower();
                report.Witnesses[i].idNumber = report.Witnesses[i].idNumber.ToLower();
                report.Witnesses[i].phone = report.Witnesses[i].phone.ToLower();
                report.Witnesses[i].city = report.Witnesses[i].city.ToLower();
                report.Witnesses[i].address = report.Witnesses[i].address.ToLower();
                report.Witnesses[i].Wstatement = report.Witnesses[i].Wstatement.ToLower();
                report.Witnesses[i].CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            for (int i = 0; i < report.Suspects.Count(); i++)
            {
                report.Suspects[i].name = report.Suspects[i].name.ToLower();
                report.Suspects[i].idNumber = report.Suspects[i].idNumber.ToLower();
                report.Suspects[i].phone = report.Suspects[i].phone.ToLower();
                report.Suspects[i].city = report.Suspects[i].city.ToLower();
                report.Suspects[i].address = report.Suspects[i].address.ToLower();
                report.Suspects[i].Sstatement = report.Suspects[i].Sstatement.ToLower();
                report.Suspects[i].CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            for (int i = 0; i < report.Statements.Count(); i++)
            {
                report.Statements[i].statement = report.Statements[i].statement.ToLower();
                report.Statements[i].CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            library.InsertReport(report);
            return Ok("Inserted");
        }
        [HttpPost("InsertFinding")]
        public IActionResult InsertFinding(Finding finding)
        {
            finding.ReportId = finding.ReportId;
            finding.description = finding.description.Trim();
            finding.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            

            for (int i = 0; i < finding.Evidencess.Count(); i++)
            {
                finding.Evidencess[i].name = finding.Evidencess[i].name.ToLower();
                finding.Evidencess[i].description = finding.Evidencess[i].description.ToLower();
                finding.Evidencess[i].CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            for (int i = 0; i < finding.Interviewss.Count(); i++)
            {
                finding.Interviewss[i].name = finding.Interviewss[i].name.ToLower();
                finding.Interviewss[i].idNumber = finding.Interviewss[i].idNumber.ToLower();
                finding.Interviewss[i].description = finding.Interviewss[i].description.ToLower();
                finding.Interviewss[i].CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            library.InsertFinding(finding);
            return Ok("Inserted");
        }
        [HttpPost("InsertArrest")]
        public IActionResult InsertArrest(Arrest arrest)
        {
            arrest.ReportId = arrest.ReportId;
            arrest.SuspectId = arrest.SuspectId;
            arrest.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            for (int i = 0; i < arrest.ArrestItem.Count(); i++)
            {
                arrest.ArrestItem[i].item = arrest.ArrestItem[i].item.ToLower();
            }
            library.InsertArrest(arrest);
            return Ok("Inserted");
        }
        [HttpGet("GetAllStations")]
        public IActionResult GetAllStations()
        {
            var stations = library.GetAllStations();
            var result = stations.Select(station => new
            {
                station.id,
                station.name,
                station.phone,
                station.city,
                station.address,
                station.code,
                station.CreatedOn,
            });
            return Ok(result); ;
        }
        [HttpGet("GetAllPolices")]
        public IActionResult GetAllPolices()
        {
            return Ok(library.GetAllPolices());
        }
        [HttpGet("GetAllCourts")]
        public IActionResult GetAllCourts()
        {
            return Ok(library.GetAllCourts());
        }
        [HttpGet("GetAllCaseOutcomes")]
        public IActionResult GetAllCaseOutcomes()
        {
            return Ok(library.GetAllCaseOutcomes());
        }
        [HttpGet("GetAllAssigns")]
        public IActionResult GetAllAssigns()
        {
            return Ok(library.GetAllAssigns());
        }
        [HttpGet("GetAllTest")]
        public IActionResult GetAllTest()
        {
            return Ok(library.GetAllTest());
        }
        [HttpGet("GetAllWitnesss")]
        public IActionResult GetAllWitnesss()
        {
            var witneses = library.GetAllWitnesss();
            var result = witneses.Select(witness => new
            {
                witness.id,
                witness.name,
                witness.phone,
                witness.city,
                witness.address,
                witness.idNumber,
                witness.CreatedOn,
            });
            return Ok(result); ;
        }
        [HttpGet("GetSpecificCourt")]
        public IActionResult GetSpecificCourt()
        {
            var witneses = library.GetSpecificCourt();
            var result = witneses.Select(witness => new
            {
                witness.id,
                witness.Report
            });
            return Ok(result); ;
        }
        [HttpGet("GetAllSuspects")]
        public IActionResult GetAllSuspects()
        {
            var witneses = library.GetAllSuspects();
            var result = witneses.Select(witness => new
            {
                witness.id,
                witness.name,
                witness.phone,
                witness.city,
                witness.address,
                witness.idNumber,
                witness.CreatedOn,
            });
            return Ok(result); ;
        }
        [HttpGet("GetAllReports")]
        public IActionResult GetAllReports()
        {
            var reports = library.GetAllReports();
            var result = reports.Select(report => new
            {
                report.id,
                report.ob,
                report.name,
                report.phone,
                report.email,
                report.occupation,
                report.city,
                report.address,
                report.idNumber,
                report.CreatedOn,
                report.takeFingerprint,
                report.Casetype,
                report.Police,
                report.Suspectss,
                report.Witnesss,
                report.Statementss,
                report.CaseListArrays,
                report.CaseListArrayss,
            });
            return Ok(result); ;
        }
        [HttpGet("GetAllFindings")]
        public IActionResult GetAllFindings()
        {
            var reports = library.GetAllFindings();
            var result = reports.Select(report => new
            {
                report.id,
                report.Report,
                report.description,
                report.CreatedOn,
                report.Evidencesss,
                report.Interviewsss,
            });
            return Ok(result); ;
        }
        [HttpGet("GetAllArrests")]
        public IActionResult GetAllArrests()
        {
            var arrestrs = library.GetAllArrests();
            var result = arrestrs.Select(arrest => new
            {
                arrest.id,
                arrest.Report,
                arrest.Suspect,
                arrest.CreatedOn,
                arrest.ArrestItemss,
            });
            return Ok(result); ;
        }
        [HttpGet("GetAllRanks")]
        public IActionResult GetAllRanks()
        {
            var ranks = library.GetAllRanks();
            var result = ranks.Select(rank => new
            {
                rank.id,
                rank.rankName,

            });
            return Ok(result); ;
        }
        [HttpGet("GetAllCasetypes")]
        public IActionResult GetAllCasetypes()
        {
            var casetypes = library.GetAllCasetypes();
            var result = casetypes.Select(casetype => new
            {
                casetype.id,
                casetype.name,

            });
            return Ok(result); ;
        }
        [HttpGet("GetAllCaselists")]
        public IActionResult GetAllCaselists()
        {
            var caselists = library.GetAllCaselists();
            var result = caselists.Select(caselist => new
            {
                caselist.id,
                caselist.CaseName,
                caselist.CasetypeId,

            });
            return Ok(result); ;
        }
        [HttpGet("GetRank/{id}")]
        public IActionResult GetRank(int id)
        {
            var result = library.GetRank(id);
            return Ok(result);
        }
        [HttpGet("GetStation/{id}")]
        public IActionResult GetStation(int id)
        {
            var result = library.GetStation(id);
            return Ok(result);
        }
        [HttpGet("GetPolice/{id}")]
        public IActionResult GetPolice(int id)
        {
            var result = library.GetPolice(id);
            return Ok(result);
        }
        [HttpGet("GetCasetype/{id}")]
        public IActionResult GetCasetype(int id)
        {
            var result = library.GetCasetype(id);
            return Ok(result);
        }
        [HttpGet("GetCaselist/{id}")]
        public IActionResult GetCaselist(int id)
        {
            var result = library.GetCaselist(id);
            return Ok(result);
        }
        [HttpGet("GetReport/{id}")]
        public IActionResult GetReport(int id)
        {
            var result = library.GetReport(id);
            return Ok(result);
        }
        [HttpGet("GetFinding/{id}")]
        public IActionResult GetFinding(int id)
        {
            var result = library.GetFinding(id);
            return Ok(result);
        }
        [HttpGet("GetArrest/{id}")]
        public IActionResult GetArrest(int id)
        {
            var result = library.GetArrest(id);
            return Ok(result);
        }
        [HttpGet("GetOneSuspect/{id}")]
        public IActionResult GetOneSuspect(int id)
        {
            var result = library.GetOneSuspect(id);
            return Ok(result);
        }
        [HttpGet("GetSpecificSuspects/{id}")]
        public IActionResult GetSpecificSuspects(int id)
        {
            var result = library.GetSpecificSuspects(id);
            return Ok(result);
        }
        [HttpGet("GetOneWitness/{id}")]
        public IActionResult GetOneWitness(int id)
        {
            var result = library.GetOneWitness(id);
            return Ok(result);
        }
        [HttpDelete("DeleteStation/{id}")]
        public IActionResult DeleteStation(int id)
        {
            var returnResult = library.DeleteStation(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteCaseOutcome/{id}")]
        public IActionResult DeleteCaseOutcome(int id)
        {
            var returnResult = library.DeleteCaseOutcome(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteAssign/{id}")]
        public IActionResult DeleteAssign(int id)
        {
            var returnResult = library.DeleteAssign(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeletePolice/{id}")]
        public IActionResult DeletePolice(int id)
        {
            var returnResult = library.DeletePolice(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteCourt/{id}")]
        public IActionResult DeleteCourt(int id)
        {
            var returnResult = library.DeleteCourt(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteReport/{id}")]
        public IActionResult DeleteReport(int id)
        {
            var returnResult = library.DeleteReport(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteArrest/{id}")]
        public IActionResult DeleteArrest(int id)
        {
            var returnResult = library.DeleteArrest(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteWitness/{id}")]
        public IActionResult DeleteWitness(int id)
        {
            var returnResult = library.DeleteWitness(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteInterview/{id}")]
        public IActionResult DeleteInterview(int id)
        {
            var returnResult = library.DeleteInterview(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteEvidence/{id}")]
        public IActionResult DeleteEvidence(int id)
        {
            var returnResult = library.DeleteEvidence(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteSuspect/{id}")]
        public IActionResult DeleteSuspect(int id)
        {
            var returnResult = library.DeleteSuspect(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteStatement/{id}")]
        public IActionResult DeleteStatement(int id)
        {
            var returnResult = library.DeleteStatement(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteItem/{id}")]
        public IActionResult DeleteItem(int id)
        {
            var returnResult = library.DeleteItem(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpDelete("DeleteFinding/{id}")]
        public IActionResult DeleteFinding(int id)
        {
            var returnResult = library.DeleteFinding(id) ? "success" : "fail";
            return Ok(returnResult);
        }
    }
}
