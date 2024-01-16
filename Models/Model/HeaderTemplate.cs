namespace Models.Model
{
    public class HeaderTemplate
    {
        public int? HeaderId { get; set; }
        public int? TemplateId { get; set; }
        public string? UniqueIdentifier { get; set; }
        public string? Drafted { get; set; }
        public string? Checked { get; set; }
        public string? Approved { get; set; }
        public string? DateChange { get; set; }
        public int? Division { get; set; }
    }
}
