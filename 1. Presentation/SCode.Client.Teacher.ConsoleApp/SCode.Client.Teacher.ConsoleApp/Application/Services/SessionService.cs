namespace SCode.Client.Teacher.ConsoleApp.Application.Services
{
    public class SessionService : ISessionService
    {
        public string BaseDirectoryPath { get; private set; }

        public string ClassroomName { get; private set; }

        public void SetBaseDirectoryPath(string baseDirectoryPath)
        {
            BaseDirectoryPath = baseDirectoryPath;
        }

        public void SetClassroomName(string classRoomName)
        {
            ClassroomName = classRoomName;
        }
    }
    
    /// <summary>
    /// Almacena los detalles de la sesión actual
    /// </summary>
    public interface ISessionService
    {
        string BaseDirectoryPath { get; }
        
        string ClassroomName { get; }
        
        void SetBaseDirectoryPath(string baseDirectoryPath);
        
        void SetClassroomName(string classRoomName);
    }
}