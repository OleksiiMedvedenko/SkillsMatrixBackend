using Data.Repository.Form.Interface;
using Microsoft.Extensions.Configuration;
using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using System.Text.RegularExpressions;
using Tools.DataService;

namespace Data.Repository.Form
{
    public class FormRepository : DatabaseProviderController, IFormRepository
    {
        public FormRepository(IConfiguration? configuration) : base(configuration) { }

        public async Task<ExternalDataResultManager<HeaderTemplate>> GetTemplateHeaderAsync(int? templateId)
        {
            var header = new HeaderTemplate();

            var sqlQuery = @"SELECT [templateHeaderId]
                              ,[templateId]
                              ,[uniqueIdentifier]
                              ,[drafted]
                              ,[checked]
                              ,[approved]
                              ,[dateChange]
                              ,[pointScaleDivision]
                          FROM [CompetitiveMatrix].[dbo].[TemplateHeader] WHERE templateId = @Id";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@Id", templateId);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        header.HeaderId = reader.GetFieldValue<int>(0);
                        header.TemplateId = reader.GetFieldValue<int>(1);
                        header.UniqueIdentifier = reader.GetFieldValue<string>(2);
                        header.Drafted = reader.GetFieldValue<string>(3);
                        header.Checked = reader.GetFieldValue<string>(4);
                        header.Approved = reader.GetFieldValue<string>(5);
                        header.DateChange = reader.GetFieldValue<DateTime>(6).ToString("dd.MM.yyyy");
                        header.Division = reader.IsDBNull(7) ? null : reader.GetFieldValue<int>(7);
                    }
                }

            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<HeaderTemplate>(header, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<HeaderTemplate>(header);
        }

        public async Task<ExternalDataResultManager<bool>> CreateTemplateHeaderAsync(HeaderTemplate? headerForm)
        {
            var commandResult = false;
            int? lastTemplateId = await GetIdLastTemplate();
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            var sqlQuery = @"INSERT INTO [CompetitiveMatrix].[dbo].[TemplateHeader] (templateId, uniqueIdentifier, drafted, checked, approved, dateChange, pointScaleDivision) 
                             VALUES (@templateId, @uniqueIdentifier, @drafted, @checked, @approved, @dateChange, @pointScaleDivision) ";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@templateId", lastTemplateId);
            command.Parameters.AddWithValue("@uniqueIdentifier", headerForm?.UniqueIdentifier);
            command.Parameters.AddWithValue("@drafted", headerForm?.Drafted);
            command.Parameters.AddWithValue("@checked", headerForm?.Checked);
            command.Parameters.AddWithValue("@approved", headerForm?.Approved);
            command.Parameters.AddWithValue("@dateChange", currentDate);
            command.Parameters.AddWithValue("@pointScaleDivision", headerForm?.Division == null ? DBNull.Value : headerForm?.Division);

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

        public async Task<ExternalDataResultManager<bool>> CreateTemplateAsync(CreateTemplateModel[]? questionsData)
        {
            var commandResult = false;
            int? lastTemplateId = await GetIdLastTemplate() + 1;

            var currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            var sqlQuery = @"INSERT INTO [dbo].[AuditDocumentTemplate] (auditDocumentTemplateId, auditID, questionId, minValue, idUser, datetime) 
                             VALUES (@auditDocumentTemplateId, @auditID, @questionId, @minValue, @idUser, @date)";

            try
            {
                connection?.Open();

                foreach (var data in questionsData)
                {
                    var command = CreateCommand(sqlQuery);
                    command.Parameters.AddWithValue("@auditDocumentTemplateId", lastTemplateId);
                    command.Parameters.AddWithValue("@auditID", data?.AuditId);
                    command.Parameters.AddWithValue("@questionId", data?.QuestionId);
                    command.Parameters.AddWithValue("@minValue", data?.MinValue);
                    command.Parameters.AddWithValue("@idUser", data?.AuthorId);
                    command.Parameters.AddWithValue("@date", currentDate);

                    if (await command.ExecuteNonQueryAsync() > 0)
                    {
                        commandResult = true;
                    }
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

        public async Task<ExternalDataResultManager<IEnumerable<TemplateFormViewModel>>> GetCurrentTemplateFormAsync(int? auditId)
        {
            var template = new List<TemplateFormViewModel>();

            var sqlQuery = @"SELECT a.areaID, a.areaName, da.departamentID, d.departamentName, q.groupName, vLT.questionId ,q.question, vLT.minValue, vLT.datetime, vLT.auditDocumentTemplateId
                                FROM [dbo].[vLastAuditDocumentTemplate] as vLT 
                                INNER JOIN [dbo].[Questions] as q ON q.questionId = vLT.questionId
                                INNER JOIN [dbo].[Audit] as au ON au.auditID = vLT.auditID
                                INNER JOIN [dbo].[Area] as a ON a.areaID = au.areaID
                                INNER JOIN [dbo].[DepartamentArea] da ON da.areaID = au.areaID
                                INNER JOIN [dbo].[Departament] d ON d.departamentID = da.departamentID
                            WHERE au.auditID = @auditId";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@auditId", auditId);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var _template = new TemplateFormViewModel(
                                new TemplateForm(
                                    reader.GetFieldValue<int>(9),
                                    new Area(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1), new Department(reader.GetFieldValue<int>(2), reader.GetFieldValue<string>(3))),
                                    new Question(reader.GetFieldValue<int>(5), reader.GetFieldValue<string>(6), reader.GetFieldValue<string>(4), reader.GetFieldValue<int>(7)),
                                    reader.IsDBNull(8) ? null : reader.GetFieldValue<DateTime>(8))
                            );

                        template.Add(_template);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<TemplateFormViewModel>>(template, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<TemplateFormViewModel>>(template);
        }

        private async Task<int?> GetIdLastTemplate()
        {
            int? lastId = 0;

            var sqlQuery = @"SELECT MAX(auditDocumentTemplateId) FROM [dbo].[AuditDocumentTemplate]";
            var command = CreateCommand(sqlQuery);

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection?.Open();
                }

                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            lastId = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch 
            {
                return null;
            }
            finally
            {
                connection?.Close();
            }

            return lastId;
        }

        public async Task<ExternalDataResultManager<IEnumerable<TemplateFormViewModel>>> GetCompletedTemplateFormAsync(int? auditHistoryId)
        {
            var template = new List<TemplateFormViewModel>();

            var firstSqlQuery = @"SELECT ad.auditDocumentTemplateId, adv.questionId, q.question, 
                                    q.groupName, ad.datetime, adv.value, adv.comment, ad.Line, CONCAT(e.firstName , ' ', e.lastName) as auditorName, d.departamentName,  CONCAT(eU.firstName, ' ', eU.lastName) as audited, p.positionName
                                    FROM [CompetitiveMatrix].[dbo].[AuditDocument] as ad 
                                    LEFT JOIN [dbo].[AuditDocumentValues] as adv ON adv.auditDocumentId = ad.auditDocumentId
                                    LEFT JOIN Questions as q ON q.questionId = adv.questionId
                                    LEFT JOIN Employee as e ON e.employeeID = ad.auditorId
                                    LEFT JOIN Departament as d ON d.departamentID = e.departamentID
                                    LEFT JOIN [dbo].[vLastAudit] as vl ON vl.auditHistoryID = ad.auditHistoryID
                                    LEFT JOIN Employee as eU ON eU.employeeID = vl.employeeID
                                    LEFT JOIN Positions as ps ON ps.employeeID = eU.employeeID
                                    LEFT JOIN Position as p ON p.positionID = ps.positionID
                                    WHERE ad.auditHistoryID = @auditHistoryId";

            var secondSqlQuery = @"SELECT adt.auditDocumentTemplateId, adt.minValue 
                                    FROM [dbo].[AuditDocumentTemplate] as adt 
                                    WHERE adt.auditDocumentTemplateId = (SELECT ad.auditDocumentTemplateId FROM [CompetitiveMatrix].[dbo].[AuditDocument] as ad WHERE ad.auditHistoryID = @auditHistoryId)";

            var firstCommand = CreateCommand(firstSqlQuery);
            firstCommand.Parameters.AddWithValue("@auditHistoryId", auditHistoryId);

            var secondCommand = CreateCommand(secondSqlQuery);
            secondCommand.Parameters.AddWithValue("@auditHistoryId", auditHistoryId);

            try
            {
                connection?.Open();
                using (var reader = firstCommand.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var _template = new TemplateFormViewModel(
                                new TemplateForm(reader.GetFieldValue<int>(0), new Question(reader.GetFieldValue<int>(1), reader.GetFieldValue<string>(2), reader.GetFieldValue<string>(3), null), reader.IsDBNull(4) ? null : reader.GetFieldValue<DateTime>(4)),
                                reader.GetFieldValue<int>(5), reader.IsDBNull(6) ? null : reader.GetFieldValue<string>(6), reader.IsDBNull(7) ? null : reader.GetFieldValue<string>(7),
                                new Employee(null, reader.GetFieldValue<string>(8).Split(' ')[0], reader.GetFieldValue<string>(8).Split(' ')[1], null, null),
                                new EmployeeViewModel(null,
                                    new Employee(null, reader.GetFieldValue<string>(10).Split(' ')[0], reader.GetFieldValue<string>(10).Split(' ')[1], null),
                                    null,
                                    new Position(null, reader.GetFieldValue<string>(11)),
                                    null,
                                    new Department(null, reader.GetFieldValue<string>(9))));

                        template.Add(_template);
                    }
                }

                using (var reader = secondCommand.ExecuteReader())
                {
                    int index = 0;
                    while (await reader.ReadAsync())
                    {
                        template[index].TemplateForm.Question.MinValue = reader.GetFieldValue<int>(1);
                        ++index;
                    }
                }

            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<TemplateFormViewModel>>(template, ex.Message, ex);
            }
            finally
            {
                connection.Close();
            }

            return new ExternalDataResultManager<IEnumerable<TemplateFormViewModel>>(template);
        }
    }
}
