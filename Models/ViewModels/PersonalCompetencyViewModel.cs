using Models.Model;

namespace Models.ViewModels
{
    public class PersonalCompetencyViewModel // audit id - is artificial
    {
        public int? id { get; private set; }
        public Employee? Employee { get; private set; }
        public Position? Position { get; private set; }
        public Supervisor? Supervisor { get; private set; }
        public List<Audit>? AuditsInfo { get; set; }
        public double? Valuation { get; set; }

        public PersonalCompetencyViewModel(Employee? employee, Position? position, Supervisor? supervisor, double? valuation = null)
        {
            id = employee?.EmployeeId;
            Employee = employee;
            Position = position;
            Supervisor = supervisor;
            AuditsInfo = new List<Audit>();
            Valuation = valuation;
        }

        public PersonalCompetencyViewModel() { AuditsInfo = new List<Audit>(); }
    }
}
