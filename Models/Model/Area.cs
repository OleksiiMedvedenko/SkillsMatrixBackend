namespace Models.Model
{
    public class Area
    {
        public int? AreaId { get; private set; }
        public string? Name { get; private set; }

        public Department? Department { get; private set; }


        public Area(int? id, string? name, Department? department)
        {
            AreaId = id;
            Name = name;
            Department = department;
        }
    }
}
