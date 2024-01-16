namespace Models.Model
{
    public class Supervisor
    {
        public int? SupervisorId { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }

        public string? FullName { get { return FirstName + " " + LastName; } }

        public Supervisor(int? id, string? firstName, string? lastName)
        {
            SupervisorId = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
