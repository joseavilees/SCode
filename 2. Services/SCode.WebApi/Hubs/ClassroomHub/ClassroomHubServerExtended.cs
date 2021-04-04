using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SCode.Shared.Requests.FileContentRequests;
using SCode.WebApi.Application.Extensions;

namespace SCode.WebApi.Hubs.ClassroomHub
{
    /// <summary>
    /// ClassroomHubServer extendido con los clientes proxies
    /// </summary>
    public class ClassroomHubServerExtended : ClassroomHubServer, IClassroomHubServerExtended
    {
        private readonly IFileContentRequestClient _fileContentRequestClient;

        public ClassroomHubServerExtended(IClassroomRepository classroomRepository,
            ILogger<ClassroomHubServerExtended> logger,
            IFileContentRequestClient fileContentRequestClient) : base(classroomRepository, logger)
        {
            _fileContentRequestClient = fileContentRequestClient;
        }

        public async Task<FileContentResponse> GetFileContent(int fileId, string classroomName)
        {
            Logger.LogDebug("Solicitando archivo {File} {Classroom}",
                fileId, classroomName);

            try
            {
                if (!ClassroomRepository.TryGetClassroom(classroomName, out var classroom))
                    throw new HubException("Sala no encontrada");

                var request = new FileContentRequest(fileId,
                        nameof(FileContentRequestCallback));

                var result = await _fileContentRequestClient
                    .Request(request, classroom.TeacherConnectionId)
                    .TimeoutAfter(TimeSpan.FromSeconds(10));

                return result;
            }
            catch (TimeoutException)
            {
                Logger.LogWarning(
                    "La solicitud de contenido de archivo caducó {File} {Classroom}",
                    fileId, classroomName);

                throw new HubException("Solicitud de contenido de archivo caducó");
            }
            catch (Exception ex) when (ex is not HubException)
            {
                Logger.LogError(ex, "No fue posible tratar una solicitud de archivo" +
                                    " {FileId} {Classroom} ", fileId, classroomName);

                throw;
            }
        }

        /// <summary>
        /// Responde a la solicitud de traer archivo
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        [Authorize]
        public Task FileContentRequestCallback(FileContentResponse response)
        {
            try
            {
                Logger.LogDebug("Tratando callback {Callback} {@Response}",
                    nameof(FileContentRequestCallback), response);

                _fileContentRequestClient
                    .RequestFileCallback(response);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex,
                    "No fue posible tratar la callback {Callback} {@Response} ",
                    nameof(FileContentRequestCallback), response);

                throw;
            }
        }
    }

    public interface IClassroomHubServerExtended
    {
        /// <summary>
        /// Responde a la solicitud de traer archivo
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        Task FileContentRequestCallback(FileContentResponse response);

        Task<FileContentResponse> GetFileContent(int fileId, string classroomName);
    }
}