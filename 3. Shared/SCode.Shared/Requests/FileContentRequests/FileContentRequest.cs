using System;

namespace SCode.Shared.Requests.FileContentRequests
{
    /// <summary>
    /// Solicitud de contenido de un archivo
    /// </summary>
    public class FileContentRequest
    {
        public Guid Id { get; set; }

        public int FileId { get; set; }
        
        public string TargetMethod { get; set; }

        public FileContentRequest(int fileId, string targetMethod)
        {
            Id = Guid.NewGuid();
            
            FileId = fileId;
            TargetMethod = targetMethod;
        }
    }
}
