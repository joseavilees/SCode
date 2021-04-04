using System.Diagnostics.CodeAnalysis;

namespace SCode.Client.Teacher.ConsoleApp
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class AppSettings
    {
        public string ApiKey { get; set; }
        public string SCodeApiUrl { get; set; }
        public string SCodeStudentWebApp { get; set; }
    }
}