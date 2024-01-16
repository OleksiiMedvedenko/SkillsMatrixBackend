using Data.LoggerRepository.Interface;
using Data.Repository.Form.Interface;
using Models.Model;
using Services.ServiceAsync.Form.Interface;

namespace Services.ServiceAsync.Form
{
    public class QuestionAsyncService : IQuestionAsyncService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ILogger _logger;

        public QuestionAsyncService(IQuestionRepository questionRepository, ILogger logger)
        {
            _questionRepository = questionRepository;
            _logger = logger;
        }

        public async Task<bool> CreateQuestionAsync(Question? question, int? userId)
        {
            if (question == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie pytania do formularza", $"Parametr przekazany do metody ({nameof(CreateQuestionAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(question));
            }

            var result = await _questionRepository.CreateQuestionAsync(question);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie pytania do formularza", $"Pytanie: {question?.Topic}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie pytania do formularza", $"Występuje błąd w metodzie ({nameof(CreateQuestionAsync)})", "Pytanie nie zostało utworzone"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }

        public async Task<IEnumerable<Question>> GetQuestionsAsync(int? userId)
        {
            var questions = await _questionRepository.GetQuestionsAsync();

            if (questions.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie pytań do tworzenia/edytowania formularza", $"Występuje błąd w metodzie: {nameof(GetQuestionsAsync)}", questions.ErrorMassage));
                throw new Exception(questions.ErrorMassage);
            }

            if (questions?.Result?.Count() == 0)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie pytań z bazy", $"Występuje błąd w metodzie: {nameof(GetQuestionsAsync)}", "Nie udało się pobrać pytań z bazy danych"));
                throw new Exception("Error while getting data from table");
            }

            return questions.Result;
        }
    }
}
