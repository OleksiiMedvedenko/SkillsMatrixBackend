namespace Models.Model
{
    public class Employee
    {
        public int? EmployeeId { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public bool? IsActive { get; }
        public int? DepartmentId { get; }

        public string? Login { get; }

        public string? FullName { get { return FirstName + " " + LastName; } }

        public Employee()
        {
                
        }

        public Employee(int? id, string? firstName, string? lastName, bool? isActive, int? departmentId = null, string? login = null)
        {
            EmployeeId = id;
            FirstName = firstName;
            LastName = lastName;
            IsActive = isActive;
            DepartmentId = departmentId;
            Login = login;
        }

        /// <summary>
        /// Create
        /// </summary>

    }
}
