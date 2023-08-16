namespace Models.CreateModels
{
    public class UpdateAuditModel
    {
        public int? AuditId { get; set; }
        public int? EmployeeId { get; set; }
        public string? LastDate { get; set; }
        public string? CurrentDate { get; set; }
        public short? LastLevel { get; set; }
        public short? CurrentLevel { get; set; }
    }
}
