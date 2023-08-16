namespace Models.Model
{
    public class Position
    {
        public int? PositionId { get; private set; }
        public string? Name { get; private set; }

        public int? AreaId { get; private set; }

        public Position(int? id, string? name, int? areaId = null)
        {
            PositionId = id;
            Name = name;
            AreaId = areaId;
        }
    }
}
