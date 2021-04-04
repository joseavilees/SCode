using System;

namespace SCode.Shared.Requests.FileContentRequests
{
    /// <summary>
    /// Respuesta solicitud contenido de un archivo
    /// </summary>
    public class FileContentResponse
    {
        public Guid RequestId { get; set; }
        public string Content { get; set; }


        public FileContentResponse()
        {
        }

        public FileContentResponse(Guid requestId, string content)
        {
            RequestId = requestId;
            Content = content;
        }
    }
}
