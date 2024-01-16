using Models.Model;

namespace Models.ViewModels
{
    public class EmployeeViewModel
    {
        public int? id { get; private set; }
        public Employee? Employee { get; private set; }
        public Supervisor? Supervisor { get; private set; }
        public Position? Position { get; private set; }
        public Department? Department { get; private set; }
        public Permission? Permission { get; set; }
        public List<Area>? Areas { get; set; }

        public EmployeeViewModel()
        {
            Areas = new List<Area>();
        }

        public EmployeeViewModel(int? id, Employee employee, Supervisor? supervisor, Position? positions, IEnumerable<Area>? area, Department? department, Permission? permission = null)
        {
            this.id = id;
            Employee = employee;
            Supervisor = supervisor;
            Position = positions;
            Areas = new List<Area>();
            Department = department;
            Permission = permission;
        }
    }
}
