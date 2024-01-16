namespace Models.Model
{
    public class Area
    {
        public int? AreaId { get; private set; }
        public string? Name { get; private set; }
        public int? PositionId { get; private set; }
        public bool? IsActive { get; set; }
        public Department? Department { get; private set; }


        public Area(int? id, string? name, Department? department)
        {
            AreaId = id;
            Name = name;
            Department = department;
        }

        public Area(int? id, string? name, Department? department, int? positionId, bool? isActive)
        {
            AreaId = id;
            Name = name;
            Department = department;
            PositionId = positionId;
            IsActive = isActive;
        }
    }
}
