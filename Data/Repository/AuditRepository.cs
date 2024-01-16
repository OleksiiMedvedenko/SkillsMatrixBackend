using Data.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Tools.DataService;

namespace Data.Repository
{
    public class AuditRepository : DatabaseProviderController, IAuditRepository
    {
        public AuditRepository(IConfiguration? configuration) : base(configuration) { }

        public async Task<ExternalDataResultManager<IEnumerable<Audit>>> GetAuditsAsync()
        {
            var audits = new List<Audit>();

            var sqlQuery = @"SELECT a.[auditID]
	                            ,a.[auditName]
                                ,a.[areaID]
	                            ,da.departamentID
								,[importance]
                            FROM [CompetitiveMatrix].[dbo].[Audit] as a 
                            LEFT JOIN DepartamentArea as da ON da.areaID = a.areaID";

            var command = CreateCommand(sqlQuery);

            try
            {
                connection?.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var audit = new Audit(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1),
                                              new Area(reader.GetFieldValue<int>(2), null, new Department(reader.GetFieldValue<int>(3), null)),
                                              reader.IsDBNull(4) ? null : reader.GetFieldValue<decimal>(4));

                        audits.Add(audit);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<Audit>>(audits, ex.Message, ex);
            }
            finally 
            { 
                connection?.Close(); 
            }

            return new ExternalDataResultManager<IEnumerable<Audit>>(audits);
        }

        public async Task<ExternalDataResultManager<int?>> UpdateAuditInfoAsync(UpdateAuditModel? newCompetency)
        {
            int? saveAuditHistoryID = null;

            var sqlQuery = @"INSERT INTO [dbo].[AuditHistory] (auditID, employeeID, auditDate, lastAuditDate, auditLevel, lastAuditLevel)
								OUTPUT INSERTED.auditHistoryID
                            VALUES (@auditId, @employeeId, @date, @lastDate, @level, @lastLevel)";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@auditId", newCompetency?.AuditId);
            command.Parameters.AddWithValue("@employeeId", newCompetency?.EmployeeId);
#pragma warning disable CS8604 // Possible null reference argument.
            command.Parameters.AddWithValue("@date", DateTime.Parse(newCompetency?.CurrentDate));
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
            command.Parameters.AddWithValue("@lastDate", DateTime.Parse(newCompetency?.LastDate));
#pragma warning restore CS8604 // Possible null reference argument.
            command.Parameters.AddWithValue("@level", newCompetency?.CurrentLevel);
            command.Parameters.AddWithValue("@lastLevel", newCompetency?.LastLevel);

            try
            {
                connection?.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            saveAuditHistoryID = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<int?>(saveAuditHistoryID, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<int?>(saveAuditHistoryID);
        }

        public async Task<ExternalDataResultManager<PersonalCompetencyViewModel>> GetPersonalCompetenceAsync(int? employeeId, int? departmentId)
        {
            var personalCompetence = new PersonalCompetencyViewModel();

            var sqlQuery = @"SELECT vl.employeeID, a.auditID, a.auditName, DATEADD(MONTH, 6, vl.auditDate) as nextAudit , vl.auditLevel , a.areaID, ar.areaName 
	                         FROM Audit a 
	                         INNER JOIN DepartamentArea as da ON a.areaID = da.areaID
	                         INNER JOIN Area as ar ON a.areaID = ar.areaID 
	                         LEFT JOIN vLastAudit as vl ON a.auditID = vl.auditID and vl.employeeID = @employeeId
	                         where da.departamentID = @departmentId
	                         ORDER BY a.areaID";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@departmentId", departmentId);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var list = new Audit(reader.IsDBNull(1) ? null : reader.GetFieldValue<int>(1), 
                                             reader.IsDBNull(2) ? null : reader.GetFieldValue<string>(2), 
                                               new Area(reader.IsDBNull(5) ? null : reader.GetFieldValue<int>(5),
                                                        reader.IsDBNull(6) ? null : reader.GetFieldValue<string>(6), null), 
                                                       (reader.IsDBNull(3) ? null : reader.GetFieldValue<DateTime>(3),
                                                        reader.IsDBNull(4) ? null : reader.GetFieldValue<Int16>(4)));

                        personalCompetence?.AuditsInfo?.Add(list);
                    }
                }

            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<PersonalCompetencyViewModel>(personalCompetence, ex.Message, ex);
            }
            finally 
            { 
                connection?.Close(); 
            }

            return new ExternalDataResultManager<PersonalCompetencyViewModel>(personalCompetence);
        }

        public async Task<ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>> GetActualAreaAuditsDatesAsync(int? areaId)
        {
            var competencies = new List<PersonalCompetencyViewModel>();

            string procedurDate = @"[dbo].[datesRaportByArea]";

            var dateCommand = CreateCommand(procedurDate, true);
            dateCommand.Parameters.AddWithValue("@areaID", areaId);

            try
            {
                connection?.Open();
                using (var dateRaportReader = dateCommand.ExecuteReader())
                {
                    while (await dateRaportReader.ReadAsync()) 
                    {
                        var competency = new PersonalCompetencyViewModel(
                            new Employee(dateRaportReader.GetFieldValue<int>(0), dateRaportReader.GetFieldValue<string>(1).Split(null)[0], dateRaportReader.GetFieldValue<string>(1).Split(null)[1], null),
                            new Position(null, dateRaportReader.GetFieldValue<string>(2)),
                            new Supervisor(null, dateRaportReader.GetFieldValue<string>(3).Split(null)[0], dateRaportReader.GetFieldValue<string>(3).Split(null)[1])); 

                        for (int i = 4; i < dateRaportReader.FieldCount; i++)
                        {
                            var name = dateRaportReader.GetName(i);
                            var date = dateRaportReader.IsDBNull(i) ? (DateTime?)null : dateRaportReader.GetFieldValue<DateTime>(i);
                            var audit = new Audit(null, dateRaportReader.GetName(i), null, (date, 0));

                            competency?.AuditsInfo?.Add(audit);
                        }

#pragma warning disable CS8604 // Possible null reference argument.
                        competencies.Add(competency);
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>(competencies, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>(competencies);
        }

        public async Task<ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>> GetActualAreaAuditsLevelAsync(int? areaId)
        {
            var competencies = new List<PersonalCompetencyViewModel>();

            string procedurDate = @"[dbo].[levelRaportByArea]";

            var command = CreateCommand(procedurDate, true);
            command.Parameters.AddWithValue("@areaID", areaId);

            try
            {
                connection?.Open();
                using (var levelRaportReader = command.ExecuteReader())
                {
                    while (await levelRaportReader.ReadAsync())
                    {
                        var competency = new PersonalCompetencyViewModel(
                            new Employee(levelRaportReader.GetFieldValue<int>(0), levelRaportReader.GetFieldValue<string>(1).Split(null)[0], levelRaportReader.GetFieldValue<string>(1).Split(null)[1], null),
                            new Position(null, levelRaportReader.GetFieldValue<string>(2)),
                            new Supervisor(null, levelRaportReader.GetFieldValue<string>(3).Split(null)[0], levelRaportReader.GetFieldValue<string>(3).Split(null)[1]));

                        for (int i = 4; i < levelRaportReader.FieldCount; i++)
                        {
                            var name = levelRaportReader.GetName(i);
                            var date = levelRaportReader.IsDBNull(i) ? (DateTime?)null : DateTime.MinValue;
                            var level = levelRaportReader.IsDBNull(i) ? (short?)null : levelRaportReader.GetFieldValue<short>(i);
                            var audit = new Audit(null, levelRaportReader.GetName(i), null, (date, level));

                            competency?.AuditsInfo?.Add(audit);
                        }

#pragma warning disable CS8604 // Possible null reference argument.
                        competencies.Add(competency);
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>(competencies, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>(competencies);
        }

        public async Task<ExternalDataResultManager<bool>> ReadAuditNotificationAsync(int? auditId)
        {
            var commandResult = false;

            var sqlQuery = @"UPDATE [dbo].[AuditHistory] set isRead = 1 WHERE auditHistoryID = @id";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@id", auditId);

            try
            {
                connection?.Open();
                if (await command.ExecuteNonQueryAsync() > 0)
                {
                    commandResult = true;
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<bool>(commandResult, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<bool>(commandResult);
        }



        public async Task<ExternalDataResultManager<IEnumerable<CompetencyViewModel>>> GetDepartmentSoonAuditsAsync(int? departmentId)
        {
            var audits = new List<CompetencyViewModel>();

            var sqlQuery = @"SELECT auditHistoryID, a.auditID, audit.auditName ,a.employeeID, e.firstName, e.lastName, e.departamentID, auditDate, auditLevel,isRead,  DATEADD(MONTH, 12, auditDate) as nextAudit
                            FROM [CompetitiveMatrix].[dbo].[vLastAudit] as a 
                            LEFT JOIN [dbo].[Employee] as e ON e.employeeID = a.employeeID
                            LEFT JOIN [CompetitiveMatrix].[dbo].[Audit] as audit ON audit.auditID = a.auditID
                            WHERE e.departamentID = @depId AND auditLevel = 0 OR e.departamentID = @depId AND DATEADD(MONTH, 12, auditDate) <= DATEADD(MONTH, 1, GETDATE()) AND isRead = 0";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@depId", departmentId);

            try
            {
                connection?.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var audit = new CompetencyViewModel(
                            reader.GetFieldValue<int>(0), 
                            new Employee(reader.GetFieldValue<int>(3), reader.GetFieldValue<string>(4), reader.GetFieldValue<string>(5), null, reader.GetFieldValue<int>(6)),
                            new Audit(reader.GetFieldValue<int>(1), reader.GetFieldValue<string>(2), null, 
                            (reader.IsDBNull(7) ? null : reader.GetFieldValue<DateTime>(10),
                            reader.IsDBNull(8) ? null : reader.GetFieldValue<Int16>(8)), null, null, reader.GetFieldValue<int>(0)),
                            reader.GetFieldValue<bool>(9));

                        audits.Add(audit);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<CompetencyViewModel>>(audits, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<CompetencyViewModel>>(audits);
        }

        public async Task<ExternalDataResultManager<Audit>> GetAuditCompetenceForEmployeeAsync(int? employeeId, int? auditId)
        {
            var audit = new Audit();

            var sqlQuery = @"SELECT auditID, employeeID, auditDate, auditLevel, auditHistoryID FROM [dbo].[vLastAudit] WHERE employeeID = @employeeId AND auditID = @auditId";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@auditId", auditId);

            try
            {
                connection?.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var _audit = new Audit(
                            reader.GetFieldValue<int>(0),
                            (reader.IsDBNull(2) ? null : reader.GetFieldValue<DateTime>(2),
                            reader.IsDBNull(3) ? null : reader.GetFieldValue<Int16>(3)),
                            reader.GetFieldValue<int>(4));

                        audit = _audit;
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<Audit>(audit, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<Audit>(audit);
        }

        public async Task<ExternalDataResultManager<int>> CreateAuditTypeAsync(ManagerAuditTypeModel? audit)
        {
            int insertedId = 0; // Initialize with a default value

            var sqlQuery = @"INSERT INTO [dbo].[Audit] (auditName, areaID, importance)
                                OUTPUT INSERTED.auditID
                                VALUES (@auditName, @areaId, @importance)";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@auditName", audit?.AuditName);
            command.Parameters.AddWithValue("@areaId", audit?.AreaId);
            command.Parameters.AddWithValue("@importance", ((object?)audit?.Importance) ?? DBNull.Value);

            try
            {
                connection?.Open();
                var result = await command.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    insertedId = Convert.ToInt32(result); // Get the inserted ID
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<int>(insertedId, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<int>(insertedId);
        }

        public async Task<ExternalDataResultManager<bool>> UpdateAuditTypeAsync(ManagerAuditTypeModel? audit) 
        {
            var commandResult = false;

            var sqlQuery = @"UPDATE [dbo].[Audit] SET auditName = @name, importance = @impportance WHERE auditID = @auditId";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@name", audit?.AuditName);
            command.Parameters.AddWithValue("@impportance", ((object?)audit?.Importance) ?? DBNull.Value);
            command.Parameters.AddWithValue("@auditId", audit?.AuditId);

            try
            {
                connection?.Open();
                if (await command.ExecuteNonQueryAsync() > 0)
                {
                    commandResult = true;
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<bool>(commandResult, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<bool>(commandResult);
        }

        public async Task<ExternalDataResultManager<IEnumerable<CompetencyViewModel>>> GetDepartmentFutureAuditsAsync(int? departmentId)
        {
            var audits = new List<CompetencyViewModel>();

            var sqlQuery = @"SELECT auditHistoryID, a.auditID, audit.auditName ,a.employeeID, e.firstName, e.lastName, e.departamentID, auditDate, auditLevel,isRead,  DATEADD(MONTH, 12, auditDate) as nextAudit
                            FROM [CompetitiveMatrix].[dbo].[vLastAudit] as a 
                            LEFT JOIN [dbo].[Employee] as e ON e.employeeID = a.employeeID
                            LEFT JOIN [CompetitiveMatrix].[dbo].[Audit] as audit ON audit.auditID = a.auditID
                            WHERE e.departamentID = @depId";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@depId", departmentId);

            try
            {
                connection?.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var audit = new CompetencyViewModel(
                            reader.GetFieldValue<int>(0),
                            new Employee(reader.GetFieldValue<int>(3), reader.GetFieldValue<string>(4), reader.GetFieldValue<string>(5), null, reader.GetFieldValue<int>(6)),
                            new Audit(reader.GetFieldValue<int>(1), reader.GetFieldValue<string>(2), null,
                            (reader.IsDBNull(7) ? null : reader.GetFieldValue<DateTime>(10),
                            reader.IsDBNull(8) ? null : reader.GetFieldValue<Int16>(8)), null),
                            reader.GetFieldValue<bool>(9));

                        audits.Add(audit);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<CompetencyViewModel>>(audits, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<CompetencyViewModel>>(audits);
        }

        public async Task<ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>> GetFutureAreaAuditsDatesAsync(int? areaId)
        {
            var competencies = new List<PersonalCompetencyViewModel>();

            string procedurDate = @"[dbo].[futureAuditsDatesRaportByArea]";

            var dateCommand = CreateCommand(procedurDate, true);
            dateCommand.Parameters.AddWithValue("@areaID", areaId);

            try
            {
                connection?.Open();
                using (var dateRaportReader = dateCommand.ExecuteReader())
                {
                    while (await dateRaportReader.ReadAsync())
                    {
                        var competency = new PersonalCompetencyViewModel(
                            new Employee(dateRaportReader.GetFieldValue<int>(0), dateRaportReader.GetFieldValue<string>(1).Split(null)[0], dateRaportReader.GetFieldValue<string>(1).Split(null)[1], null),
                            new Position(null, dateRaportReader.GetFieldValue<string>(2)),
                            new Supervisor(null, dateRaportReader.GetFieldValue<string>(3).Split(null)[0], dateRaportReader.GetFieldValue<string>(3).Split(null)[1]));

                        for (int i = 4; i < dateRaportReader.FieldCount; i++)
                        {
                            var name = dateRaportReader.GetName(i);
                            var date = dateRaportReader.IsDBNull(i) ? (DateTime?)null : dateRaportReader.GetFieldValue<DateTime>(i);
                            var audit = new Audit(null, dateRaportReader.GetName(i), null, (date, 0));

                            competency?.AuditsInfo?.Add(audit);
                        }

#pragma warning disable CS8604 // Possible null reference argument.
                        competencies.Add(competency);
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>(competencies, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>(competencies);
        }

        public async Task<ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>> GetValuationAreaReportAsync(int? areaId)
        {
            var competencies = new List<PersonalCompetencyViewModel>();

            string procedurDate = @"[dbo].[valuationReportByArea]";

            var command = CreateCommand(procedurDate, true);
            command.Parameters.AddWithValue("@areaID", areaId);

            try
            {
                connection?.Open();
                using (var levelRaportReader = command.ExecuteReader())
                {
                    while (await levelRaportReader.ReadAsync())
                    {
                        var competency = new PersonalCompetencyViewModel(
                            new Employee(levelRaportReader.GetFieldValue<int>(0), levelRaportReader.GetFieldValue<string>(1).Split(null)[0], levelRaportReader.GetFieldValue<string>(1).Split(null)[1], null),
                            new Position(null, levelRaportReader.GetFieldValue<string>(2)),
                            new Supervisor(null, levelRaportReader.GetFieldValue<string>(3).Split(null)[0], levelRaportReader.GetFieldValue<string>(3).Split(null)[1]));

                        for (int i = 5; i < levelRaportReader.FieldCount; i++)
                        {
                            var name = levelRaportReader.GetName(i).Split("|")[0];
                            var date = levelRaportReader.IsDBNull(i) ? (DateTime?)null : DateTime.MinValue;
                            var level = levelRaportReader.IsDBNull(i) ? (short?)null : levelRaportReader.GetFieldValue<short>(i);

                            decimal? importance = null;
                            if (levelRaportReader.GetName(i).Split("|").Length > 1)
                            {
                                decimal parsedValue;
                                if (decimal.TryParse(levelRaportReader.GetName(i).Split("|")[1], out parsedValue))
                                {
                                    importance = parsedValue;
                                }
                            }

                            var audit = new Audit(null, name, null, (date, level), null, importance);

                            competency?.AuditsInfo?.Add(audit);
                        }

                        competency.Valuation = levelRaportReader.IsDBNull(4) ? 0 : (double?)levelRaportReader.GetFieldValue<decimal?>(4);

#pragma warning disable CS8604 // Possible null reference argument.
                        competencies.Add(competency);
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>(competencies, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>(competencies);
        }
    }
}
