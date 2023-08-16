using Models.Model;

namespace Models.ViewModels
{
    public class CompetencyViewModel
    {
        public int? Id { get; private set; }
        public Employee? Employee { get; private set; } 
        public Audit? Audit { get; private set; }
        public bool? IsRead { get; private set; }

        public int? id { get => Id; }
        public string? title { get => Audit?.Name ; }
        public string? date { get => Audit?.CurrentAuditInfo?.Item1; }


        public CompetencyViewModel(){  }

        public CompetencyViewModel(int? id, Employee? employee, Audit? audit, bool? isRead)
        {
            Id = id;
            Employee = employee;
            Audit = audit;
            IsRead = isRead;
        }
    }
}
