using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CreateModels
{
    public class AuditDataCreateModel
    {
        public int? AuditDocumentId { get; set; }
        public int? AuditHistoryId { get; set; }
        public int? AuditDocumentTemplateId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? AuditorId { get; set; }
    }
}
