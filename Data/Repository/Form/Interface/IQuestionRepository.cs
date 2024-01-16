using Models.Model;
using Tools.DataService;

namespace Data.Repository.Form.Interface
{
    public interface IQuestionRepository
    {
        Task<ExternalDataResultManager<IEnumerable<Question>>> GetQuestionsAsync();

        Task<ExternalDataResultManager<bool>> CreateQuestionAsync(Question? question);
    }
}
