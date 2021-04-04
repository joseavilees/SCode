using System;
using System.IO;
using System.Linq;

namespace SCode.Client.Teacher.ConsoleApp.Application.Helpers
{
    public static class PathHelper
    {
        /// <summary>
        /// Comprueba si la ruta es un directorio
        /// independientemente de si esta existe
        /// o no en el sistema actual
        ///
        /// https://stackoverflow.com/a/19596821
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsDirectoryPath(string path)
        {
            if (path == null) 
                throw new ArgumentNullException(nameof(path));

            path = path.Trim();

            if (Directory.Exists(path))
                return true;

            if (File.Exists(path))
                return false;

            // neither file nor directory exists. guess intention

            // if has trailing slash then it's a directory
            if (new[] { "\\", "/" }.Any(x => path.EndsWith(x)))
                return true; // ends with slash

            // if has extension then its a file; directory otherwise
            return string.IsNullOrWhiteSpace(Path.GetExtension(path));
        }
        
        public static bool IsBackupFile(string path) 
        {
            if (IsDirectoryPath(path))
                return false;

            return path.EndsWith("~");
        }
    }
}