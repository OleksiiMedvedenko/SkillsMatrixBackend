using Models.Model;

namespace Models.ViewModels
{
    public class PersonalPurposeViewModel
    {
        public Supervisor? Supervisor { get; private set; }
        public Audit? Audit { get; private set; }
        public int? Purpose { get; private set; }
        public int? EmployeesWithLevelTwo { get; private set; }
        public int? Difference { get; private set; }

        public PersonalPurposeViewModel(Supervisor? supervisor, Audit? audit, int? purpose, int? employeesWithLevelTwo, int? difference)
        {
            Supervisor = supervisor;
            Audit = audit;
            Purpose = purpose;
            EmployeesWithLevelTwo = employeesWithLevelTwo;
            Difference = difference;
        }

        //ctor only for GetDepartmentAuditsWithPurpose method 
        public PersonalPurposeViewModel(Audit? audit, int? purpose)
        {
            Audit = audit;
            Purpose = purpose;
        }
    }
}
