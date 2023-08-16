namespace Models.Model
{
    public class Department
    {
        public int? DepartmentId { get; private set; }
        public string? Name { get; private set; }


        public Department(int? departmentId, string? name)
        {
            DepartmentId = departmentId;
            Name = name;
        }
    }
}
