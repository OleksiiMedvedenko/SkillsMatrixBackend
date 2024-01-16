using Data.Repository.Form.Interface;
using Microsoft.Extensions.Configuration;
using Models.Model;
using Tools.DataService;

namespace Data.Repository.Form
{
    public class QuestionRepository : DatabaseProviderController, IQuestionRepository
    {
        public QuestionRepository(IConfiguration? configuration) : base(configuration) { }

        public async Task<ExternalDataResultManager<bool>> CreateQuestionAsync(Question? question)
        {
            var commandResult = false;

            var sqlQuery = @"INSERT INTO [dbo].[Questions] (question, groupName) VALUES (@topic, @group)";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@topic", question?.Topic);
            command.Parameters.AddWithValue("@group", question?.GroupName);

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

        public async Task<ExternalDataResultManager<IEnumerable<Question>>> GetQuestionsAsync()
        {
            var questions = new List<Question>();

            var sqlQuery = @"SELECT * FROM [dbo].[Questions]";

            var command = CreateCommand(sqlQuery);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        Question question = new Question(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1), reader.GetFieldValue<string>(2), null);

                        questions.Add(question);
                    }
                }

            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<Question>>(questions, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<Question>>(questions);
        }
    }
}
