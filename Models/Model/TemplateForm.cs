namespace Models.Model
{
    public class TemplateForm
    {
        public int? TemplateId { get; private set; }
        public Area? Area { get; private set; }
        public Question? Question { get; private set; }

        public DateTime? CreatedDate { get; private set; }

        /// <summary>
        /// get template
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="area"></param>
        /// <param name="question"></param>
        /// <param name="createdDate"></param>
        public TemplateForm(int? templateId, Area? area, Question? question, DateTime? createdDate)
        {
            TemplateId = templateId;
            Area = area;
            Question = question;
            CreatedDate = createdDate;
        }

        /// <summary>
        /// get completed form
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="question"></param>
        /// <param name="createdDate"></param>
        public TemplateForm(int? templateId, Question? question, DateTime? createdDate)
        {
            TemplateId = templateId;
            Question = question;
            CreatedDate = createdDate;
        }
    }
}
