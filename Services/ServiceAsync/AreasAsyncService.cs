using Data.Repository;
using Data.Repository.Interface;
using Models.CreateModels;
using Models.Model;
using Services.ServiceAsync.Interface;

namespace Services.ServiceAsync
{
    public class AreasAsyncService : IAreasAsyncService
    {
        private readonly IAreaRepository _areaRepository;

        public AreasAsyncService(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }

        public async Task<bool> CreateAreaAsync(AreaCreateModel? area)
        {
            if (area == null)
            {
                throw new ArgumentNullException(nameof(area));
            }

            var result = await _areaRepository.CreateAreaAsync(area);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }


        public async Task<IEnumerable<Area>> GetAreasAsync()
        {
            var areas = await _areaRepository.GetAreasAsync();

            if (areas.ErrorExist)
            {
                throw new Exception(areas.ErrorMassage);
            }

            return areas.Result ?? throw new Exception("Error while getting data from table (Areas)");
        }

        public async Task<IEnumerable<Area>> GetEmployeeAreasAsync(int? employeeId)
        {
            if (employeeId == null)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            var areas = await _areaRepository.GetEmployeeAreasAsync(employeeId);

            if (areas.ErrorExist)
            {
                throw new Exception(areas.ErrorMassage);
            }

            return areas.Result ?? throw new Exception("Error while getting data from table (Department)");
        }
    }
}
