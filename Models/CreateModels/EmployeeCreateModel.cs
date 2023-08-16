namespace Models.CreateModels
{
    public class EmployeeCreateModel
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Login { get; set; }
        public int? AreaId { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public int? SupervisorId { get; set; }
    }
}