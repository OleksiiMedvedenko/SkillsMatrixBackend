using Data.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Tools.DataService;

namespace Data.Repository
{
    public class PersonalPurposeRepository : DatabaseProviderController, IPersonalPurposeRepository
    {
        public PersonalPurposeRepository(IConfiguration? configuration) : base(configuration) { }

        public async Task<ExternalDataResultManager<IEnumerable<PersonalPurposeViewModel>>> GetDepartmentAuditsWithPurposeAsync(int? departmentId)
        {
            var purposes = new List<PersonalPurposeViewModel>();

            var sqlQuery = @"SELECT a.auditID, a.auditName, pp.purpose FROM Audit as a 
                            INNER JOIN DepartamentArea as da ON da.areaID = a.areaID
                            LEFT JOIN PersonalPurpose as pp ON pp.auditID = a.auditID
                            WHERE da.departamentID = @departmentId";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@departmentId", departmentId);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        PersonalPurposeViewModel purpose = new PersonalPurposeViewModel(
                            new Audit(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1), null, null),
                            reader.IsDBNull(2) ? null : reader.GetFieldValue<int>(2));

                        purposes.Add(purpose);
                    }
                }

            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<PersonalPurposeViewModel>>(purposes, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<PersonalPurposeViewModel>>(purposes);
        }

        public async Task<ExternalDataResultManager<IEnumerable<PersonalPurposeViewModel>>> GetDepartmentPersonalPurposeAsync(int? departmentId)
        {
            var purposes = new List<PersonalPurposeViewModel>();

            var sqlQuery = @"SELECT sup.employeeID AS SupervisorID,
                                    sup.firstName, sup.lastName,
                                    a.auditID,
                                    a.auditName,
                                    ISNULL(p.purpose, 0) AS Purpose, -- Handle NULL values
                                    SUM(CASE WHEN vLA.auditLevel = 2 THEN 1 ELSE 0 END) AS EmployeesWithAuditLevel2,
                                    (SUM(CASE WHEN vLA.auditLevel = 2 THEN 1 ELSE 0 END) - ISNULL(p.purpose, 0)) AS Difference
                                FROM Employee AS sup
                                INNER JOIN Employee AS emp ON emp.supervisorID = sup.employeeID
                                INNER JOIN [CompetitiveMatrix].[dbo].[vLastAudit] AS vLA ON emp.employeeID = vLA.employeeID
                                LEFT JOIN [CompetitiveMatrix].[dbo].[Audit] AS a ON vLA.auditID = a.auditID
                                LEFT JOIN [dbo].[PersonalPurpose] AS p ON p.auditID = a.auditID
                                WHERE sup.departamentID = @departmentId
                                GROUP BY
                                    sup.employeeID,
                                    sup.firstName,
                                    sup.lastName,
                                    a.auditID,
                                    a.auditName,
                                    ISNULL(p.purpose, 0) 
                                ORDER BY sup.employeeID, a.auditID;";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@departmentId", departmentId);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        PersonalPurposeViewModel purpose = new PersonalPurposeViewModel(
                            new Supervisor(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1), reader.GetFieldValue<string>(2)),
                            new Audit(reader.GetFieldValue<int>(3), reader.GetFieldValue<string>(4), null, null),
                            reader.GetFieldValue<int>(5), reader.GetFieldValue<int>(6), reader.GetFieldValue<int>(7));

                        purposes.Add(purpose);
                    }
                }

            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<PersonalPurposeViewModel>>(purposes, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<PersonalPurposeViewModel>>(purposes);
        }

        public async Task<ExternalDataResultManager<bool>> UpdatePersonalPurposeAsync(CreatePersonalPurpose? createPersonalPurposes)
        {
            bool commandResult = false;

            var sqlquery = @"UPDATE PersonalPurpose SET purpose = @purpose, modifyDate = GETDATE() WHERE auditID = @auditId";

            var command = CreateCommand(sqlquery);
            command.Parameters.AddWithValue("@purpose", createPersonalPurposes?.Purpose);
            command.Parameters.AddWithValue("@auditId", createPersonalPurposes?.AuditId);

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

        public async Task<ExternalDataResultManager<bool>> CreatePersonalPurposeAsync(CreatePersonalPurpose? createPersonalPurposes)
        {
            bool commandResult = false;

            var sqlquery = @"INSERT INTO PersonalPurpose (auditID, purpose, modifyDate) VALUES (@auditId, @purpose, GETDATE())";

            var command = CreateCommand(sqlquery);
            command.Parameters.AddWithValue("@auditId", createPersonalPurposes?.AuditId);
            command.Parameters.AddWithValue("@purpose", createPersonalPurposes?.Purpose);

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
    }
}
