namespace Tools.Serialization
{
    public class JSONSerialization
    {
        public static string ConvertToJSON<T>(T data)
        {
            return System.Text.Json.JsonSerializer.Serialize(data);
        }
    }
}
