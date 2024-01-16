namespace Models.Model
{
    public class LevelDescription
    {
        public int? LevelDescriptionId { get; private set; }
        public int? AuditId { get; private set; }
        public int? Level { get; private set; }
        public string? Description { get; private set; }
        public string? AuditName { get; private set; }

        public LevelDescription(int? levelDescriptionId, int? auditId, int? level, string? description)
        {
            LevelDescriptionId = levelDescriptionId;
            AuditId = auditId;
            Level = level;
            Description = description;
        }

        public LevelDescription(int? auditId, string? auditName, int? level, string? description)
        {
            AuditId = auditId;
            AuditName = auditName;
            Level = level;
            Description = description;
        }
    }
}
