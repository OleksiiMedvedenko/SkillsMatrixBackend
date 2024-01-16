namespace Models.CreateModels
{
    public class CreateAuditDocumentModel
    {
        public int? AuditHistoryId { get; set; }
        public int? AuditTemplateId { get; set; }
        public int? AuditorId { get; set; }
        public string? Line { get; set; }
    }
}
