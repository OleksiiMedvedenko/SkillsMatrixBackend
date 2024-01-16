using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CreateModels
{
    public class UpdateEmpoyeeModel
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Login { get; set; }
        public List<int?>? AreasId { get; set; }
        public int? VacancyId { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public string? PositionName { get; set; }
        public int? SupervisorId { get; set; }
        public int? PermissionId { get; set; }
    }
}
