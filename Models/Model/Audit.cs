using System.Runtime.CompilerServices;

namespace Models.Model
{
    public class Audit
    {
        public int? id { get; set; }
        public int? AuditId { get; private set; }
        public string? Name { get; private set; }
        public decimal? Importance { get; private set; }
        public int? AuditHistoryId { get; private set; }

        public Area? Area { get; private set; }

        public Tuple<string?, short?>? CurrentAuditInfo { get; private set; } // <date, level>
        public Tuple<string?, short?>? LastAuditInfo { get; private set; }

        public Audit()
        {
                
        }

        public Audit(int? auditId, (DateTime? date, short? level)? currentAudit, int? auditHistoryId)
        {
            AuditId = auditId;
            CurrentAuditInfo = new Tuple<string?, short?>(currentAudit?.date?.ToString("dd-MMM-yyyy"), currentAudit?.level);
            AuditHistoryId = auditHistoryId;
        }

        /// <summary>
        /// ctor for get all audits in DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="area"></param>
        public Audit(int? id, string? name, Area? area, decimal? importance)
        {
            AuditId = id;
            Name = name;
            Area = area;
            Importance = importance;
        }

        public Audit(int? id, string? name, Area? area, (DateTime? date, short? level)? currentAudit, (DateTime? date, short? level)? lastAudit = null, decimal? importance = null, int? auditHistoryId = null)
        {
            this.id = id;
            AuditId = id;
            Name = name;
            Area = area;
            CurrentAuditInfo = new Tuple<string?, short?>(currentAudit?.date?.ToString("dd-MM-yyyy"), currentAudit?.level);
            LastAuditInfo = new Tuple<string?, short?>(lastAudit?.date?.ToString("dd-MMM-yyyy"), lastAudit?.level);
            Importance = importance;
            AuditHistoryId = auditHistoryId;
        }
    }
}
