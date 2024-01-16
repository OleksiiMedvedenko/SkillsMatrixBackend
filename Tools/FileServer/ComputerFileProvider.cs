using System.IO;

namespace Tools.FileServer
{
    public class ComputerFileProvider
    {
        string rootPath = @"M:\";

        public static string GetFileByPath(string directoryPath, string searchPattern = "*.pdf")
        {
            try
            {
                string[] files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);

                if (files.Any())
                {
                    return files.FirstOrDefault();
                }
                else
                {
                    return "Nie znaleziono pliku!";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
