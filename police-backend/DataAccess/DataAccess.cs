using police_backend.Models;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Net;
using System.Reflection.Metadata;
using static System.Collections.Specialized.BitVector32;
using static System.Reflection.Metadata.BlobBuilder;
using System.Diagnostics.Eventing.Reader;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
using police_backend.ViewModel;
using Microsoft.AspNetCore.Http;
//using police_backend.Migrations;

namespace police_backend.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration configuration;
        private readonly string DbConnection;

        public DataAccess(IConfiguration _configuration)
        {
            configuration = _configuration;
            DbConnection = configuration["connectionStrings:conn"] ?? "";
        }

        public int CreateUser(User user)
        {
            var result = 0;
            using (var connection = new SqlConnection(DbConnection))
            {
                var parameters = new
                {
                    fn = user.FirstName,
                    ln = user.LastName,
                    em = user.Email,
                    mb = user.Mobile,
                    pwd = user.Password,
                    blk = user.Blocked,
                    act = user.Active,
                    con = user.CreatedOn,
                    type = user.UserType.ToString(),
                };
                var sql = "insert into Users (FirstName, LastName, Email, Mobile, Password, Blocked, Active, CreatedOn, UserType) values (@fn, @ln, @em, @mb, @pwd, @blk, @act, @con, @type);";
                result = connection.Execute(sql, parameters);
            }
            return result;
        }
        public bool IsStationCodeAvailable(string Code)
        {
            var result = false;

            using (var connection = new SqlConnection(DbConnection))
            {
                result = connection.ExecuteScalar<bool>("select count(*) from Station where code=@code;", new { Code });
            }
            Console.WriteLine(result);
            return result;
        }

        public bool IsEmailAvailable(string email)
        {
            var result = false;

            using (var connection = new SqlConnection(DbConnection))
            {
                result = connection.ExecuteScalar<bool>("select count(*) from Users where Email=@email;", new { email });
            }

            return !result;
        }

        public bool AuthenticateUser(string email, string password, out User? user)
        {
            var result = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                result = connection.ExecuteScalar<bool>("select count(1) from Users where email=@email and password=@password;", new { email, password });
                if (result)
                {
                    user = connection.QueryFirst<User>("select * from Users where email=@email;", new { email });
                }
                else
                {
                    user = null;
                }
            }
            return result;
        }

        public IList<Arrest> GetAllArrests()
        {
            IEnumerable<Arrest> arrests = null;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = "select * from Arrest;";
                arrests = connection.Query<Arrest>(sql);

                foreach (var book in arrests)
                {
                    sql = "select * from Report where id=" + book.ReportId;
                    book.Report = connection.QuerySingle<Report>(sql);
                    sql = "select * from Suspect where id=" + book.SuspectId;
                    book.Suspect = connection.QuerySingle<Suspect>(sql);
                    sql = "select * from CellList where id=" + book.CellListId;
                    book.CellList = connection.QuerySingle<CellList>(sql);
                    sql = "select * from ArrestItem where ArrestId=" + book.id;
                    book.ArrestItemss = connection.Query<ArrestItem>(sql); 
                }
            }
            return arrests.ToList();
        }

        public IList<Police> GetAllPolices()
        {
            IEnumerable <Police> officers;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = "select * from police";
                
                officers = connection.Query<Police>(sql);
                
            }
            return officers.ToList();
        }
        public IList<Test> GetAllTest()
        {
            IEnumerable<Test> tests;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = @"select p.name as OfficerName,s.name as StationName,r.rankName as RankName from Station as s LEFT JOIN Police as p On p.StationId=s.id INNER JOIN Rank as r On p.RankId=r.id;";
                tests = connection.Query<Test>(sql);
                Console.WriteLine(tests);
            }
            
            return tests.ToList();
        }

        public IList<Rank> GetAllRanks()
        {
            IEnumerable<Rank> ranks;

            using (var connection = new SqlConnection(DbConnection))
            {
                ranks = connection.Query<Rank>("select * from Rank;");
            }

            return ranks.ToList();
        }

        public IList<Report> GetAllReports()
        {
            IEnumerable<Report> reports = null;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = "select * from Report;";
                reports = connection.Query<Report>(sql);

                foreach (var book in reports)
                {
                    sql = "select * from Casetype where id=" + book.CasetypeId;
                    book.Casetype = connection.QuerySingle<Casetype>(sql);
                    //sql = "select * from Caselist where id=" + book.CaselistId;
                    //book.Caselist = connection.QuerySingle<Caselist>(sql);
                    sql = "select * from Police where id=" + book.PoliceId;
                    book.Police = connection.QuerySingle<Police>(sql);
                    sql = "select * from Suspect where ReportId=" + book.id;
                    book.Suspectss = connection.Query<Suspect>(sql);
                    sql = "select * from Witness where ReportId=" + book.id;
                    book.Witnesss = connection.Query<Witness>(sql);
                    sql = "select * from Statement where ReportId=" + book.id;
                    book.Statementss = connection.Query<Statement>(sql);
                    sql = "select * from CaseListArray where ReportId=" + book.id;
                    book.CaseListArrayss = connection.Query<CaseListArray>(sql);
                }
               
                
            }
            return reports.ToList();
        }

        public IList<Station> GetAllStations()
        {
            IEnumerable<Station> stations;

            using (var connection = new SqlConnection(DbConnection))
            {
                stations = connection.Query<Station>("select * from Station;");
            }

            return stations.ToList();
        }

        public Rank GetRank(int id)
        {
            var rank = new Rank();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Rank WHERE Id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    rank.id = (int)r["id"];
                    rank.rankName = (string)r["rankName"];
                }
            }
            return rank;
        }

        public Station GetStation(int id)
        {
            var station = new Station();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Station WHERE id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    station.id = (int)r["id"];
                    station.name = (string)r["name"];
                    station.phone = (string)r["phone"];
                    station.city = (string)r["city"];
                    station.address = (string)r["address"];
                    station.code = (string)r["code"];
                    station.CreatedOn = (string)r["CreatedOn"];
                }
            }
            return station;
        }

        public Police GetPolice(int id)
        {
            var police = new Police();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Police WHERE id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    police.id = (int)reader["id"];
                    police.name = (string)reader["name"];
                    police.phone = (string)reader["phone"];
                    police.email = (string)reader["email"];
                    police.serialNumber = (string)reader["serialNumber"];
                    police.idNumber = (string)reader["idNumber"];
                    police.CreatedOn = (string)reader["CreatedOn"];
                    police.StationId = (int)reader["StationId"];
                    police.RankId = (int)reader["RankId"];

                    var stationid = (int)reader["StationId"];
                    police.Station = GetStation(stationid);

                    var rankId = (int)reader["RankId"];
                    police.Rank = GetRank(rankId);
                }
            }
            return police;
        }

        public void InsertArrest(Arrest arrest)
        {
            var results = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = "select id from Arrest where id=@cat";
                var parameter21 = new
                {
                    cat = arrest.id
                };
                results = connection.ExecuteScalar<bool>(sql, parameter21);
                if (results)
                {

                    sql = $"update Arrest set ReportId=@ReportID,SuspectId=@SuspectID,CreatedOn=@Created,CellListId=@CellListID where id={arrest.id}";
                    var parameter = new
                    {
                        ReportID = arrest.ReportId,
                        SuspectID = arrest.SuspectId,
                        Created = arrest.CreatedOn,
                        CellListID = arrest.CellListId,
                    };
                    var inserted = connection.Execute(sql, parameter) == 1;
                    if (inserted)
                    {
                        sql = "select id from Arrest where CreatedOn=@cat";
                        var parameter1 = new
                        {
                            cat = arrest.CreatedOn
                        };
                        var repId = connection.ExecuteScalar<int>(sql, parameter1);
                        
                        for (int i = 0; i < arrest.ArrestItem.Count(); i++)
                        {
                            sql = "select id from ArrestItem where id=@cat";
                            var parameter23 = new
                            {
                                cat = arrest.ArrestItem[i].id
                            };
                            results = connection.ExecuteScalar<bool>(sql, parameter23);

                            var parameter3 = new
                            {
                                Name = arrest.ArrestItem[i].item,
                                reportID = repId,
                            };

                            if (results)
                            {
                                sql = $"update ArrestItem set item=@Name,ArrestId=@reportID where id={arrest.ArrestItem[i].id}";
                            }
                            else
                            {
                                sql = "insert into ArrestItem (item,ArrestId) values (@Name,@reportID);";
                            }

                            connection.Execute(sql, parameter3);

                            
                        }
                    }

                }
                else
                {

                    sql = "insert into Arrest (ReportId,SuspectId,CreatedOn,CellListId) values (@ReportID,@SuspectID,@Created,@CellListID);";
                    var parameter = new
                    {
                        ReportID = arrest.ReportId,
                        SuspectID = arrest.SuspectId,
                        Created = arrest.CreatedOn,
                        CellListID = arrest.CellListId,
                    };
                    var inserted = connection.Execute(sql, parameter) == 1;
                    if (inserted)
                    {
                        sql = "select id from Arrest where CreatedOn=@cat";
                        var parameter1 = new
                        {
                            cat = arrest.CreatedOn
                        };
                        var repId = connection.ExecuteScalar<int>(sql, parameter1);
                        sql = "insert into ArrestItem (item,ArrestId) values (@Name,@reportID);";
                        for (int i = 0; i < arrest.ArrestItem.Count(); i++)
                        {
                            var parameter3 = new
                            {
                                Name = arrest.ArrestItem[i].item,
                                reportID = repId,
                            };
                            connection.Execute(sql, parameter3);
                        }
                    }

                }
             }
        }

        public void InsertOfficer(Police police)
        {
            var results = false;
            using var connection = new SqlConnection(DbConnection);
            var sql = "select id from Police where id=@cat";
            var parameter1 = new
            {
                cat = police.id
            };
            results = connection.ExecuteScalar<bool>(sql, parameter1);
            if (results)
            {
                var parameter2 = new
                {
                    Name = police.name,
                    Phone = police.phone,
                    Email = police.email,
                    SerialNumber = police.serialNumber,
                    IDNumber = police.idNumber,
                    StationID = police.StationId,
                    Date = police.CreatedOn,
                    Ranks = police.RankId,

                };
                sql = $"update Police set name=@Name,phone=@Phone,email=@Email,serialNumber=@SerialNumber,idNumber=@IDNumber,stationId=@StationID,CreatedOn=@Date,RankId=@Ranks where id={police.id}";
                connection.Execute(sql, parameter2);
            }
            else
            {
                var parameter = new
                {
                    Name = police.name,
                    Phone = police.phone,
                    Email = police.email,
                    SerialNumber = police.serialNumber,
                    IDNumber = police.idNumber,
                    StationID = police.StationId,
                    Date = police.CreatedOn,
                    Ranks = police.RankId,

                };
                connection.Execute("insert into Police (name, phone,email,serialNumber,idNumber,StationId,CreatedOn,RankId) values (@Name, @Phone,@Email,@SerialNumber,@IDNumber,@StationID,@Date,@Ranks);", parameter);
            }

        }

        public void InsertReport(Report report)
        {
            var results = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = "select COUNT(OB) from Report where CreatedOn=@cat";
                var parameter111 = new
                {
                    cat = report.CreatedOn
                };
                var NoOfRows = connection.ExecuteScalar<int>(sql, parameter111) + 1;
                sql = "select id from Report where id=@cat";
                var parameter11 = new
                {
                    cat = report.id
                };
                results = connection.ExecuteScalar<bool>(sql, parameter11);
                if (results)
                {
                    sql = $"update Report set ob=@OB,name=@Name,phone=@Phone,email=@Email,occupation=@Occupation,city=@City,address=@Address,idNumber=@IdNumber,CreatedOn=@CreateDOn,takeFingerprint=@FingerPrint,CasetypeId=@CaseTypeId,PoliceId=@PoliceID,GenderId=@GenderID where id={report.id}";
                    var parameter = new
                    {
                        OB = NoOfRows + "/" + report.CreatedOn,
                        Name = report.name,
                        Phone = report.phone,
                        Email = report.email,
                        Occupation = report.occupation,
                        City = report.city,
                        Address = report.address,
                        IdNumber = report.idNumber,
                        CreateDOn = report.CreatedOn,
                        FingerPrint = report.takeFingerprint,
                        CaseTypeId = report.CasetypeId,
                        PoliceID = report.PoliceId,
                        GenderID = report.GenderId,
                    };
                    var inserted = connection.Execute(sql, parameter) == 1;
                    if (inserted)
                    {
                        sql = "select id from Report where idNumber=@cat";
                        var parameter1 = new
                        {
                            cat = report.idNumber
                        };
                        var repId = connection.ExecuteScalar<int>(sql, parameter1);

                        for (int i = 0; i < report.caseList.Count(); i++)
                        {
                            sql = $"select CaselistId from CaseListArray where CaselistId=@cat and ReportId={report.id}";
                            var parameter12 = new
                            {
                                cat = report.caseList[i]
                            };
                            results = connection.ExecuteScalar<bool>(sql, parameter12);
                            var parameter33 = new
                            {
                                Name = report.caseList[i],
                                reportID = repId
                            };
                            if (results)
                            {
                                sql = $"delete CaseListArray where CaselistId != {report.caseList[i]} and ReportId={report.id}";
                            }
                            else
                            {
                                sql = "insert into CaseListArray (CaselistId,ReportId) values (@Name,@reportID)";
                            }
                            connection.Execute(sql, parameter33);
                        }
                        for (int i = 0; i < report.Witnesses.Count(); i++)
                        {

                            sql = "select id from Witness where id=@cat";
                            var parameter12 = new
                            {
                                cat = report.Witnesses[i].id
                            };
                            results = connection.ExecuteScalar<bool>(sql, parameter12);

                            var parameter3 = new
                            {
                                Name = report.Witnesses[i].name,
                                IDnumber = report.Witnesses[i].idNumber,
                                Phone = report.Witnesses[i].phone,
                                City = report.Witnesses[i].city,
                                Address = report.Witnesses[i].address,
                                Created = report.Witnesses[i].CreatedOn,
                                Statement = report.Witnesses[i].Wstatement,
                                reportID = repId,
                            };

                            if (results)
                            {
                                sql = $"update Witness set name=@Name,idNumber=@IDnumber,phone=@Phone,city=@City,address=@Address,CreatedOn=@Created,ReportId=@reportID,Wstatement=@Statement where id={report.Witnesses[i].id}";
                            }
                            else
                            {
                                sql = "insert into Witness (name, idNumber, phone, city, address,CreatedOn,ReportId,Wstatement) values (@Name, @IDnumber, @Phone, @City, @Address, @Created, @reportID,@Statement);";
                            }

                            connection.Execute(sql, parameter3);
                        }

                        for (int i = 0; i < report.Suspects.Count(); i++)
                        {

                            sql = "select id from Suspect where id=@cat";
                            var parameter13 = new
                            {
                                cat = report.Suspects[i].id
                            };
                            results = connection.ExecuteScalar<bool>(sql, parameter13);

                            var parameter4 = new
                            {
                                SUName = report.Suspects[i].name,
                                SUIDnumber = report.Suspects[i].idNumber,
                                SUPhone = report.Suspects[i].phone,
                                SUCity = report.Suspects[i].city,
                                SUAddress = report.Suspects[i].address,
                                SUCreated = report.Suspects[i].CreatedOn,
                                Statement = report.Suspects[i].Sstatement,
                                SUreportID = repId,
                            };
                            if (results)
                            {
                                sql = $"update Suspect set name=@SUName,idNumber=@SUIDnumber,phone=@SUPhone,city=@SUCity,address=@SUAddress,CreatedOn=@SUCreated,ReportId=@SUreportID,Sstatement=@Statement where Id={report.Suspects[i].id}";
                            }
                            else
                            {
                                sql = "insert into Suspect (name, idNumber, phone, city, address,CreatedOn,ReportId,Sstatement) values (@SUName, @SUIDnumber, @SUPhone, @SUCity, @SUAddress, @SUCreated, @SUreportID,@Statement);";
                            }
                            connection.Execute(sql, parameter4);
                        }

                        for (int i = 0; i < report.Statements.Count(); i++)
                        {
                            sql = "select id from Statement where id=@cat";
                            var parameter14 = new
                            {
                                cat = report.Statements[i].id
                            };
                            results = connection.ExecuteScalar<bool>(sql, parameter14);

                            var parameter5 = new
                            {
                                SName = report.Statements[i].statement,
                                SCreated = report.Statements[i].CreatedOn,
                                SreportID = repId,
                            };
                            if (results)
                            {
                                sql = $"update Statement set statement=@SName,CreatedOn=@SCreated,ReportId=@SreportID where Id={report.Statements[i].id}";
                            }
                            else
                            {
                                sql = "insert into Statement (statement, CreatedOn, ReportId) values (@SName, @SCreated, @SreportID);";
                            }
                            connection.Execute(sql, parameter5);
                        }
                    }
                }
                else
                {             
                    sql = "insert into Report (ob,name, phone, email, occupation, city,address,idNumber,CreatedOn,takeFingerprint,CasetypeId,PoliceId,GenderId) values (@OB,@Name, @Phone, @Email, @Occupation, @City, @Address, @IdNumber, @CreateDOn,@FingerPrint,@CaseTypeId,@PoliceID,@GenderID);";
                    var parameter = new
                    {
                        OB = NoOfRows + "/" + report.CreatedOn,
                        Name = report.name,
                        Phone = report.phone,
                        Email = report.email,
                        Occupation = report.occupation,
                        City = report.city,
                        Address = report.address,
                        IdNumber = report.idNumber,
                        CreateDOn = report.CreatedOn,
                        FingerPrint = report.takeFingerprint,
                        CaseTypeId = report.CasetypeId,
                        PoliceID = report.PoliceId,
                        GenderID = report.GenderId,
                    };
                    
                    var inserted = connection.Execute(sql, parameter) == 1;
                    if (inserted)
                    {
                        sql = "select id from Report where idNumber=@cat";
                        var parameter1 = new
                        {
                            cat = report.idNumber
                        };
                        var repId = connection.ExecuteScalar<int>(sql, parameter1);

                        sql = "insert into CaseListArray (CaselistId, ReportId) values (@Name, @reportID);";
                        for (int i = 0; i < report.caseList?.Count(); i++)
                        {
                            var parameter6 = new
                            {
                                Name = report.caseList[i],
                                reportID = repId,
                            };
                            connection.Execute(sql, parameter6);
                        }

                        sql = "insert into Witness (name, idNumber, phone, city, address,CreatedOn,ReportId,Wstatement) values (@Name, @IDnumber, @Phone, @City, @Address, @Created, @reportID,@Statement);";
                        for (int i = 0; i < report.Witnesses.Count(); i++)
                        {
                            var parameter3 = new
                            {
                                Name = report.Witnesses[i].name,
                                IDnumber = report.Witnesses[i].idNumber,
                                Phone = report.Witnesses[i].phone,
                                City = report.Witnesses[i].city,
                                Address = report.Witnesses[i].address,
                                Created = report.Witnesses[i].CreatedOn,
                                Statement = report.Witnesses[i].Wstatement,
                                reportID = repId,
                            };
                            connection.Execute(sql, parameter3);
                        }
                        sql = "insert into Suspect (name, idNumber, phone, city, address,CreatedOn,ReportId,Sstatement) values (@SUName, @SUIDnumber, @SUPhone, @SUCity, @SUAddress, @SUCreated, @SUreportID,@Statement);";
                        for (int i = 0; i < report.Suspects.Count(); i++)
                        {
                            var parameter4 = new
                            {
                                SUName = report.Suspects[i].name,
                                SUIDnumber = report.Suspects[i].idNumber,
                                SUPhone = report.Suspects[i].phone,
                                SUCity = report.Suspects[i].city,
                                SUAddress = report.Suspects[i].address,
                                SUCreated = report.Suspects[i].CreatedOn,
                                Statement = report.Suspects[i].Sstatement,
                                SUreportID = repId,
                            };
                            connection.Execute(sql, parameter4);
                        }
                        sql = "insert into Statement (statement, CreatedOn, ReportId) values (@SName, @SCreated, @SreportID);";
                        for (int i = 0; i < report.Statements.Count(); i++)
                        {
                            var parameter5 = new
                            {
                                SName = report.Statements[i].statement,
                                SCreated = report.Statements[i].CreatedOn,
                                SreportID = repId,
                            };
                            connection.Execute(sql, parameter5);
                        }
                    }

                }


            }
        }

        public void InsertOB(DateData dateData)
        {
            using var connection = new SqlConnection(DbConnection);
            var sql= "select COUNT(OB) from DateData where CreatedOn=@cat";
            var parameter1 = new
            {
                cat = dateData.OB
            };
            var NoOfRows = connection.ExecuteScalar<int>(sql, parameter1)+1;
            var parameter = new
            {
                OBNo = NoOfRows + "/" + dateData.OB,
                Date = dateData.CreatedOn

            };
            connection.Execute("insert into DateData (OB,CreatedOn) values (@OBNo,@Date);", parameter);
        }
        public void InsertStation(Station station)
        {
            var results = false;
            using var connection = new SqlConnection(DbConnection);
            var sql = "select id from Station where id=@cat";
            var parameter1 = new
            {
                cat = station.id
            };
            results = connection.ExecuteScalar<bool>(sql,parameter1);
            if (results)
            {
                var parameter2 = new
                {
                    Name = station.name,
                    Phone = station.phone,
                    City = station.city,
                    Address = station.address,
                    Code = station.code,
                    Date = station.CreatedOn,

                };
                sql = $"update Station set name=@Name,phone=@Phone,city=@City,address=@Address,code=@Code,CreatedOn=@Date where Id={station.id}";
                connection.Execute(sql, parameter2) ;
            }
           
            else
            {
                var parameter = new
                {
                    Name = station.name,
                    Phone = station.phone,
                    City = station.city,
                    Address = station.address,
                    Code = station.code,
                    Date = station.CreatedOn,

                };
                connection.Execute("insert into Station (name, phone,city,address,code,CreatedOn) values (@Name, @Phone,@City,@Address,@Code,@Date);", parameter);

            }
        }

        public Casetype GetCasetype(int id)
        {
            var casetype = new Casetype();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Casetype WHERE id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    casetype.id = (int)r["id"];
                    casetype.name = (string)r["name"];
                }
            }
            return casetype;
        }

        public Caselist GetCaselist(int id)
        {
            var caselist = new Caselist();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Caselist WHERE id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    caselist.id = (int)r["id"];
                    caselist.CaseName = (string)r["CaseName"];
                    caselist.CasetypeId = (int)r["CasetypeId"];

                    var casetypeid = (int)r["CasetypeId"];
                    caselist.Casetype = GetCasetype(casetypeid);
                }
            }
            return caselist;
        }

        public Report GetReport(int id)
        {
            var report = new Report();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Report WHERE id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    report.id = (int)r["id"];
                    report.ob = (string)r["ob"];
                    report.name = (string)r["name"];
                    report.phone = (string)r["phone"];
                    report.email = (string)r["email"];
                    report.occupation = (string)r["occupation"];
                    report.city = (string)r["city"];
                    report.address = (string)r["address"];
                    report.idNumber = (string)r["idNumber"];
                    report.CreatedOn = (string)r["CreatedOn"];
                    report.takeFingerprint = (bool)r["takeFingerprint"];
                    report.CasetypeId = (int)r["CasetypeId"];
                    report.PoliceId = (int)r["PoliceId"];


                    var casetypeid = (int)r["CasetypeId"];
                    report.Casetype = GetCasetype(casetypeid);

                    var policeId = (int)r["PoliceId"];
                    report.Police = GetPolice(policeId);

                    var testId = (int)r["id"];
                    report.Witnesses = GetWitness(testId);

                    var test1Id = (int)r["id"];
                    report.Suspects = GetSuspect(test1Id);

                    var test2Id = (int)r["id"];
                    report.Statements = GetStatement(test2Id);

                    var test3Id = (int)r["id"];
                    report.CaseListArrays = GetCaseListArray(test3Id);
                }
            }
            return report;
        }

        public List<Witness> GetWitness(int id)
        {
            var witnesss = new List<Witness>();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Witness WHERE ReportId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    var witness = new Witness()
                    {
                        id = (int)r["id"],
                        name = (string)r["name"],
                        idNumber = (string)r["idNumber"],
                        phone = (string)r["phone"],
                        city = (string)r["city"],
                        address = (string)r["address"],
                        CreatedOn = (string)r["CreatedOn"],
                        Wstatement = (string)r["Wstatement"],
                    };

                    witnesss.Add(witness);

                }
            }
            return witnesss;
        }

        public List<Suspect> GetSuspect(int id)
        {
            var suspectss = new List<Suspect>();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Suspect WHERE ReportId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    var suspect = new Suspect()
                    {
                        id = (int)r["id"],
                        name = (string)r["name"],
                        idNumber = (string)r["idNumber"],
                        phone = (string)r["phone"],
                        city = (string)r["city"],
                        address = (string)r["address"],
                        CreatedOn = (string)r["CreatedOn"],
                        Sstatement = (string)r["Sstatement"],
                    };

                    suspectss.Add(suspect);

                }
            }
            return suspectss;
        }

        public List<Statement> GetStatement(int id)
        {
            var statementss = new List<Statement>();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Statement WHERE ReportId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    var statement = new Statement()
                    {
                        id = (int)r["id"],
                        statement = (string)r["statement"],
                        CreatedOn = (string)r["CreatedOn"],
                    };

                    statementss.Add(statement);

                }
            }
            return statementss;
        }

        public Arrest GetArrest(int id)
        {
            var arrest = new Arrest();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Arrest WHERE id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    arrest.id = (int)r["id"];
                    arrest.CreatedOn = (string)r["CreatedOn"];
                    arrest.ReportId = (int)r["ReportId"];
                    arrest.SuspectId = (int)r["SuspectId"];

                    var reportid = (int)r["ReportId"];
                    arrest.Report = GetReport(reportid);

                    var suspectid = (int)r["SuspectId"];
                    arrest.Suspect = GetOneSuspect(suspectid);


                    var test2Id = (int)r["id"];
                    arrest.ArrestItem = GetArrestItem(test2Id);
                }
            }
            return arrest;
        }

        public List<ArrestItem> GetArrestItem(int id)
        {
            var arrestItemss = new List<ArrestItem>();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM ArrestItem WHERE ArrestId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    var Item = new ArrestItem()
                    {
                        id = (int)r["Id"],
                        item = (string)r["item"],
                    };

                    arrestItemss.Add(Item);

                }
            }
            return arrestItemss;
        }

        public Suspect GetOneSuspect(int id)
        {
            var suspect = new Suspect();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Suspect WHERE id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    suspect.id = (int)r["id"];
                    suspect.name = (string)r["name"];
                    suspect.idNumber = (string)r["idNumber"];
                    suspect.phone = (string)r["phone"];
                    suspect.city = (string)r["city"];
                    suspect.address = (string)r["address"];
                    suspect.CreatedOn = (string)r["CreatedOn"];
                    //suspect.Re= (string)r["CreatedOn"];

                    var reportId = (int)r["ReportId"];
                    suspect.Report = GetReport(reportId);
                }
            }
            return suspect;
        }

        public Witness GetOneWitness(int id)
        {
            var witness = new Witness();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Witness WHERE id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    witness.id = (int)r["id"];
                    witness.name = (string)r["name"];
                    witness.idNumber = (string)r["idNumber"];
                    witness.phone = (string)r["phone"];
                    witness.city = (string)r["city"];
                    witness.address = (string)r["address"];
                    witness.CreatedOn = (string)r["CreatedOn"];
                    //suspect.Re= (string)r["CreatedOn"];

                    var reportId = (int)r["ReportId"];
                    witness.Report = GetReport(reportId);
                }
            }
            return witness;
        }

        public IList<Witness> GetAllWitnesss()
        {
            IEnumerable<Witness> witnesss;

            using (var connection = new SqlConnection(DbConnection))
            {
                witnesss = connection.Query<Witness>("select * from Witness;");
            }

            return witnesss.ToList();
        }

        public IList<Suspect> GetAllSuspects()
        {
            IEnumerable<Suspect> suspectss;

            using (var connection = new SqlConnection(DbConnection))
            {
                suspectss = connection.Query<Suspect>("select * from Suspect;");
            }

            return suspectss.ToList();
        }

        public IList<Casetype> GetAllCasetypes()
        {
            IEnumerable<Casetype> casetypes;

            using (var connection = new SqlConnection(DbConnection))
            {
                casetypes = connection.Query<Casetype>("select * from Casetype;");
            }

            return casetypes.ToList();
        }

        public IList<Caselist> GetAllCaselists()
        {
            IEnumerable<Caselist> caselists;

            using (var connection = new SqlConnection(DbConnection))
            {
                caselists = connection.Query<Caselist>("select * from Caselist;");
            }

            return caselists.ToList();
        }

        public bool DeleteStation(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Station where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public bool DeletePolice(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Police where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public bool DeleteReport(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Report where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public bool DeleteArrest(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Arrest where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public bool DeleteWitness(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Witness where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public bool DeleteSuspect(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Suspect where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public bool DeleteStatement(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Statement where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public IList<Suspect> GetSpecificSuspects(int id)
        {
            IEnumerable<Suspect> witnesses;

            using (var connection = new SqlConnection(DbConnection))
            {
                witnesses = connection.Query<Suspect>("SELECT * FROM Suspect WHERE ReportId=" + id + ";");
            }

            return witnesses.ToList();
        }

        public IList<Court> GetSpecificCourt()
        {
            IEnumerable<Court> courts = null;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = "select * from Court";
                courts = connection.Query<Court>(sql);

                foreach (var book in courts)
                {
                    sql = "select * from Report where id=" + book.ReportId;
                    book.Report = connection.QuerySingle<Report>(sql);
                }
            }
            return courts.ToList();
        }

        public bool DeleteItem(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete ArrestItem where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public IList<User> GetUsers()
        {
            IEnumerable<User> users;

            using (var connection = new SqlConnection(DbConnection))
            {
                users = connection.Query<User>("select * from Users;");
            }

            return users.ToList();
        }

        public List<CaseListArray> GetCaseListArray(int id)
        {
            var casearrayss = new List<CaseListArray>();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM CaseListArray WHERE ReportId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    var casearrays = new CaseListArray()
                    {
                        id = (int)r["id"],
                        CaselistId = (int)r["CaselistId"],
                        Caselist = GetCaselist((int)r["CaselistId"]),

                    };

                    casearrayss.Add(casearrays);

                }
            }
            return casearrayss;
        }

        public void InsertFinding(Finding finding)
        {
            var results = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = "select id from Finding where id=@cat";
                var parameter11 = new
                {
                    cat = finding.id
                };
                results = connection.ExecuteScalar<bool>(sql, parameter11);
                if (results)
                {
                    sql = $"update Finding set ReportId=@ReportID,description=@Description,CreatedOn=@Date where id={finding.id};";
                    var parameter = new
                    {
                        ReportID = finding.ReportId,
                        Description = finding.description,
                        Date = finding.CreatedOn,
                    };

                    var inserted = connection.Execute(sql, parameter) == 1;
                    if (inserted)
                    {
                        sql = "select id from Finding where id=@cat";
                        var parameter1 = new
                        {
                            cat = finding.id
                        };
                        var repId = connection.ExecuteScalar<int>(sql, parameter1);

                        for (int i = 0; i < finding.Evidencess?.Count(); i++)
                        {
                            sql = "select id from Evidence where id=@cat";
                            var parameter13 = new
                            {
                                cat = finding.Evidencess[i].id
                            };
                            results = connection.ExecuteScalar<bool>(sql, parameter13);

                            var parameter6 = new
                            {
                                Name = finding.Evidencess[i].name,
                                Description = finding.Evidencess[i].description,
                                Date = finding.Evidencess[i].CreatedOn,
                                reportID = repId,
                            };
                            if (results)
                            {
                                sql = $"update Evidence set name=@Name,description=@Description,CreatedOn=@Date,FindingId=@reportID where id={finding.Evidencess[i].id}";
                            }
                            else
                            {
                                sql = "insert into Evidence (name, description,CreatedOn,FindingId) values (@Name,@Description,@Date,@reportID);";
                            }
                            connection.Execute(sql, parameter6);
                        }
                        for (int i = 0; i < finding.Interviewss?.Count(); i++)
                        {

                            sql = "select id from Interview where id=@cat";
                            var parameter13 = new
                            {
                                cat = finding.Interviewss[i].id
                            };
                            results = connection.ExecuteScalar<bool>(sql, parameter13);

                            var parameter6 = new
                            {
                                Name = finding.Interviewss[i].name,
                                IDNumber = finding.Interviewss[i].idNumber,
                                Description = finding.Interviewss[i].description,
                                Date = finding.Interviewss[i].CreatedOn,
                                reportID = repId,
                            };
                            if (results)
                            {
                                sql = $"update Interview set name=@Name,idNumber=@IDNumber,description=@Description,CreatedOn=@Date,FindingId=@reportID where id={finding.Interviewss[i].id}";
                            }
                            else
                            {
                                sql = "insert into Interview (name,idNumber, description,CreatedOn,FindingId) values (@Name,@IDNumber,@Description,@Date,@reportID);";
                            }
                            connection.Execute(sql, parameter6);
                        }


                    }
                }
                else
                {
                    sql = "insert into Finding (ReportId,description,CreatedOn) values (@ReportID,@Description,@Date);";
                    var parameter = new
                    {
                        ReportID = finding.ReportId,
                        Description = finding.description,
                        Date = finding.CreatedOn,
                    };

                    var inserted = connection.Execute(sql, parameter) == 1;
                    if (inserted)
                    {
                        sql = "select id from Finding where CreatedOn=@cat";
                        var parameter1 = new
                        {
                            cat = finding.CreatedOn
                        };
                        var repId = connection.ExecuteScalar<int>(sql, parameter1);

                        sql = "insert into Evidence (name,description,CreatedOn,FindingId) values (@Name,@Description,@Date,@findingID);";
                        for (int i = 0; i < finding.Evidencess.Count(); i++)
                        {
                            var parameter6 = new
                            {
                                Name = finding.Evidencess[i].name,
                                Description = finding.Evidencess[i].description,
                                Date = finding.Evidencess[i].CreatedOn,
                                findingID = repId,
                            };
                            connection.Execute(sql, parameter6);
                        }
                        sql = "insert into Interview (name,idNumber, description,CreatedOn,FindingId) values (@Name,@IDNumber,@Description,@Date,@reportID);";
                        for (int i = 0; i < finding.Interviewss.Count(); i++)
                        {
                            var parameter6 = new
                            {
                                Name = finding.Interviewss[i].name,
                                IDNumber = finding.Interviewss[i].idNumber,
                                Description = finding.Interviewss[i].description,
                                Date = finding.Interviewss[i].CreatedOn,
                                reportID = repId,
                            };
                            connection.Execute(sql, parameter6);
                        }


                    }

                }

            }
        }

        public IList<Finding> GetAllFindings()
        {
            IEnumerable<Finding> findings = null;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = "select * from Finding;";
                findings = connection.Query<Finding>(sql);

                foreach (var book in findings)
                {
                    sql = "select * from Report where id=" + book.ReportId;
                    book.Report = connection.QuerySingle<Report>(sql);
                    sql = "select * from Evidence where FindingId=" + book.id;
                    book.Evidencesss = connection.Query<Evidence>(sql);
                    sql = "select * from Interview where FindingId=" + book.id;
                    book.Interviewsss = connection.Query<Interview>(sql);
                }


            }
            return findings.ToList();
        }

        public bool DeleteFinding(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Finding where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public Finding GetFinding(int id)
        {
            var finding = new Finding();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Finding WHERE id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    finding.id = (int)r["id"];
                    finding.ReportId = (int)r["ReportId"];
                    finding.description = (string)r["description"]; 
                    finding.CreatedOn = (string)r["CreatedOn"];
 
                    var reportId = (int)r["ReportId"];
                    finding.Report = GetReport(reportId);

                    var testId = (int)r["id"];
                    finding.Evidencess = GetEvidence(testId);

                    var test1Id = (int)r["id"];
                    finding.Interviewss = GetInterview(test1Id);
                }
            }
            return finding;
        }

        public bool DeleteInterview(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Interview where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public bool DeleteEvidence(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Evidence where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public List<Evidence> GetEvidence(int id)
        {
            var evidencess = new List<Evidence>();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Evidence WHERE FindingId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    var evidence = new Evidence()
                    {
                        id = (int)r["id"],
                        name = (string)r["name"],
                        description = (string)r["description"],
                        CreatedOn = (string)r["CreatedOn"],
                    };

                    evidencess.Add(evidence);

                }
            }
            return evidencess;
        }

        public List<Interview> GetInterview(int id)
        {
            var interviewss = new List<Interview>();
            using (SqlConnection connection = new(DbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Interview WHERE FindingId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    var interviews = new Interview()
                    {
                        id = (int)r["id"],
                        name = (string)r["name"],
                        description = (string)r["description"],
                        idNumber = (string)r["idNumber"],
                        CreatedOn = (string)r["CreatedOn"],
                    };

                    interviewss.Add(interviews);

                }
            }
            return interviewss;
        }

        public void InsertCase(CaseOutcome caseOutcome)
        {
            var results = false;
            using var connection = new SqlConnection(DbConnection);
            var sql = "select id from CaseOutcome where id=@cat";
            var parameter1 = new
            {
                cat = caseOutcome.id
            };
            results = connection.ExecuteScalar<bool>(sql, parameter1);
            if (results)
            {
                var parameter2 = new 
                {
                    ReportID = caseOutcome.ReportId,
                    OutComeID = caseOutcome.OutcomeId,
                    ISClosed = caseOutcome.isClosed,
                    Date = caseOutcome.createdOn
                };
                sql = $"update CaseOutcome set ReportId=@ReportID,OutcomeId=@OutComeID,isClosed=@ISClosed,createdOn=@Date where id={caseOutcome.id}";
                connection.Execute(sql, parameter2);
            }
            else
            {
                var parameter = new
                {
                    ReportID = caseOutcome.ReportId,
                    OutComeID = caseOutcome.OutcomeId,
                    ISClosed = caseOutcome.isClosed,
                    Date = caseOutcome.createdOn
                };
                connection.Execute("insert into CaseOutcome (ReportId, OutcomeId,isClosed,createdOn) values (@ReportID, @OutComeID,@ISClosed,@Date);", parameter);
            }
        }

        public IList<CaseOutcomeVM> GetAllCaseOutcomes()
        {
            IEnumerable<CaseOutcomeVM> caseOutcomes;

            using (var connection = new SqlConnection(DbConnection))
            {
                //caseOutcomes = connection.Query<CaseOutcome>("select * from CaseOutcome;");
                caseOutcomes = connection.Query<CaseOutcomeVM>("select r.ob as oBNo,o.OutcomeName as caseOutcomeName,c.isClosed as closed  from CaseOutcome AS c INNER JOIN Report as r On c.ReportId=r.id INNER JOIN Outcome as o On c.OutcomeId=o.id;");
            }

            return caseOutcomes.ToList();
        }

        public bool DeleteCaseOutcome(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete CaseOutcome where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public void InsertAssign(Assign assign)
        {
            var results = false;
            using var connection = new SqlConnection(DbConnection);
            var sql = "select id from Assign where id=@cat";
            var parameter1 = new
            {
                cat = assign.id
            };
            results = connection.ExecuteScalar<bool>(sql, parameter1);
            if (results)
            {
                var parameter2 = new
                {
                    ReportID = assign.ReportId,
                    PoliceID = assign.PoliceId,
                    Date = assign.createdOn
                };
                sql = $"update Assign set ReportId=@ReportID,PoliceId=@PoliceID,createdOn=@Date where id={assign.id}";
                connection.Execute(sql, parameter2);
            }
            else
            {
                var parameter = new
                {
                    ReportID = assign.ReportId,
                    PoliceID = assign.PoliceId,
                    Date = assign.createdOn
                };
                connection.Execute("insert into Assign (ReportId, PoliceId,createdOn) values (@ReportID, @PoliceID,@Date);", parameter);
            }
        }

        public IList<AssignVM> GetAllAssigns()
        {
            IEnumerable<AssignVM> assigns;

            using (var connection = new SqlConnection(DbConnection))
            {
                //assigns = connection.Query<Assign>("select * from Assign;");
                assigns = connection.Query<AssignVM>("select r.name as OBNo,p.name as OfficerName from Assign AS a INNER JOIN Report as r On a.ReportId=r.id INNER JOIN Police as p On a.PoliceId=p.id;");
            }

            return assigns.ToList();
        }

        public bool DeleteAssign(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Assign where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public void InsertCourt(Court court)
        {
            var results = false;
            using var connection = new SqlConnection(DbConnection);
            var sql = "select id from Court where id=@cat";
            var parameter1 = new
            {
                cat = court.id
            };
            results = connection.ExecuteScalar<bool>(sql, parameter1);
            if (results)
            {
                var parameter2 = new
                {
                    CName = court.Name,
                    Date = court.CourtDate,
                    Docket = court.DocketNo,
                    ReportID = court.ReportId
                };
                sql = $"update Court set Name=@CName,CourtDate=@Date,DocketNo=@Docket,ReportId=@ReportID where id={court.id}";
                connection.Execute(sql, parameter2);
            }
            else
            {
                var parameter = new
                {
                    CName = court.Name,
                    Date = court.CourtDate,
                    Docket = court.DocketNo,
                    ReportID = court.ReportId
                };
                connection.Execute("insert into Court (Name, CourtDate,DocketNo,ReportId) values (@CName,@Date,@Docket,@ReportID);", parameter);
            }
        }

        public IList<CourtVM> GetAllCourts()
        {
            IEnumerable<CourtVM> courts;

            using (var connection = new SqlConnection(DbConnection))
            {
                //courts = connection.Query<CourtVM>("select * from Court;");
                courts = connection.Query<CourtVM>("select c.Name as Name,c.CourtDate as CourtDate,c.DocketNo as DocketName,r.name as ComplainantName from Court AS c INNER JOIN Report as r On c.ReportId=r.id;");
            }

            return courts.ToList();
        }

        public bool DeleteCourt(int id)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Court where Id={id}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }
    }
}