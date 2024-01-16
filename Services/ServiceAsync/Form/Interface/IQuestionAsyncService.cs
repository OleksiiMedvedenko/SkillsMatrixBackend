using Models.Model;

namespace Services.ServiceAsync.Form.Interface
{
    public interface IQuestionAsyncService
    {
        Task<IEnumerable<Question>> GetQuestionsAsync(int? userId);

        Task<bool> CreateQuestionAsync(Question? question, int? userId);
    }
}
