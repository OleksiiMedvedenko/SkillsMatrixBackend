namespace Models.Model
{
    public class Question
    {
        public int? QuestionId { get; private set; }
        public string? Topic { get; private set; }
        public string? GroupName { get; private set; }
        public int? MinValue { get; set; }

        public Question(int? questionId, string? topic, string? groupName, int? minValue)
        {
            QuestionId = questionId;
            Topic = topic;
            GroupName = groupName;
            MinValue = minValue;
        }
    }
}
