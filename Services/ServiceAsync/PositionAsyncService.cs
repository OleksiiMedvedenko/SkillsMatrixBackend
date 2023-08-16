using Data.Repository.Interface;
using Models.CreateModels;
using Models.Model;
using Models.Status;
using Services.ServiceAsync.Interface;

namespace Services.ServiceAsync
{
    public class PositionAsyncService : IPositionAsyncService
    {
        private readonly IPositionRepositry _positionRepositry;

        public PositionAsyncService(IPositionRepositry positionRepositry)
        {
            _positionRepositry = positionRepositry;
        }

        public async Task<bool> CreatePositionAsync(PositionCreateModel? position)
        {
            if (position == null)
            {
                throw new ArgumentNullException(nameof(position));
            }

            var result = await _positionRepositry.CreatePositionAsync(position);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<bool> ChangeStatusEmployeePositionAsync(int? areaId, int? employeeId, ActivationStatus? positionStatus)
        {
            if (areaId == null || employeeId == null)
            {
                throw new ArgumentNullException(nameof(areaId) + nameof(employeeId));
            }



            var result = await _positionRepositry.ChangeStatusEmployeePositionAsync(areaId, employeeId, positionStatus);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<IEnumerable<Position>> GetPositionsAsync()
        {
            var positions = await _positionRepositry.GetPositionsAsync();

            if (positions.ErrorExist)
            {
                throw new Exception(positions.ErrorMassage);
            }

            return positions.Result ?? throw new Exception("Error while getting data from table (Positions)");
        }

        public async Task<IEnumerable<Position>> GetPositionsByAreaAsync(int? areaId)
        {
            if (areaId == null)
            {
                throw new ArgumentNullException(nameof(areaId));
            }

            var positions = await GetPositionsAsync();

            return positions.Where(p => p.AreaId == areaId) ?? throw new Exception("Error while getting data from DB!");
        }

        public async Task<bool> DeactivateAllEmployeePositions(int? employeeId)
        {
            if (employeeId == null || employeeId == null)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            var result = await _positionRepositry.DeactivateAllEmployeePositions(employeeId);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<bool> SetAnEmployeeNewPosition(int? positionId, int? employeeId)
        {
            if (positionId == null || employeeId == null)
            {
                throw new ArgumentNullException(nameof(positionId) + nameof(employeeId));
            }

            var result = await _positionRepositry.SetAnEmployeeNewPosition(positionId, employeeId);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }
    }
}
