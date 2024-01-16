using Models.Model;

namespace Models.ViewModels
{
    public class TemplateFormViewModel
    {
        public TemplateForm? TemplateForm { get; private set; }

        public Employee? Auditor { get; private set; }
        public EmployeeViewModel? Audited { get; private set; }

        public string? Comment { get; private set; }
        public int? Value { get; private set; }
        public string? Line { get; private set; }

        /// <summary>
        /// Get template
        /// </summary>
        /// <param name="templateForm"></param>
        public TemplateFormViewModel(TemplateForm? templateForm)
        {
            TemplateForm = templateForm;
        }

        /// <summary>
        /// get completed form
        /// </summary>
        /// <param name="templateForm"></param>
        /// <param name="value"></param>
        /// <param name="comment"></param>
        /// <param name="line"></param>
        /// <param name="employee"></param>
        /// <param name="employeeViewModel"></param>
        public TemplateFormViewModel(TemplateForm? templateForm, int? value, string? comment, string? line, Employee? auditor, EmployeeViewModel? audited)
        {
            TemplateForm = templateForm;
            Value = value;
            Comment = comment;
            Line = line;
            Auditor = auditor;
            Audited = audited;
        }
    }
}
