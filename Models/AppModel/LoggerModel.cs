namespace Models.AppModel
{
    public class LoggerModel
    {
        public int? LogId { get; set; }
        public int? UserId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }


        public LoggerModel(int? userId = null, string? title = null, string? message = null, string? error = null)
        {
            UserId = userId;
            Title = title;
            Message = message;
            Error = error;
        }
    }
}
