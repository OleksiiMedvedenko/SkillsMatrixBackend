namespace Tools.DataService
{
    public class ExternalDataResultManager<T>
    {
        public string? ErrorMassage { get; set; }
        public bool ErrorExist { get; set; }
        public T? Result { get; set; }
        public Exception? Error { get; set; }

        public ExternalDataResultManager(T? result, string? message = null, Exception? e = null)
        {
            Result = result;
            Error = e;
            ErrorMassage = message;
            ErrorExist = message == null && e == null ? false : true;
        }
    }

    public class ExternalDataResultManager
    {
        public string? ErrorMassage { get; set; }
        public bool ErrorExist { get; set; }
        public Exception? Error { get; set; }

        public ExternalDataResultManager(string? message = null, Exception? e = null)
        {
            Error = e;
            ErrorMassage = message;
            ErrorExist = message == null && e == null ? false : true;
        }
    }
}
