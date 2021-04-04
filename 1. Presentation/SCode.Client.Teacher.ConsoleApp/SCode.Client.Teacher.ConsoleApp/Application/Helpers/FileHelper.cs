using System.IO;
using System.Text;

namespace SCode.Client.Teacher.ConsoleApp.Application.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// Obtener el contenido de un archivo independientemente
        /// de si esta siendo usado por otros procesos
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileContent(string path)
        {
            if (!File.Exists(path))
                return null;
            
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.UTF8);

            var content = sr.ReadToEnd();

            return content;
        }
    }
}