using Models.AppModel;
using Tools.DataService;

namespace Data.LoggerRepository.Interface
{
    public interface ILogger
    {
        Task<ExternalDataResultManager> DeclareErrorAsync(LoggerModel loggerModel);

    }
}
