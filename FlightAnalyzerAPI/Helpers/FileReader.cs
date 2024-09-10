using FlightAnalyzerAPI.Interface;
using System.IO;

namespace FlightAnalyzerAPI.Helpers
{
    public class FileReader : IFileReader
    {
        public string[] ReadFile(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}
