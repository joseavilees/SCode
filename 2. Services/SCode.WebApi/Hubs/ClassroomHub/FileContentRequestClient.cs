using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SCode.Shared.Requests.FileContentRequests;

namespace SCode.WebApi.Hubs.ClassroomHub
{
    /// <summary>
    /// Solicitud para solicitar a un cliente (maestro) que envíe 
    /// al servidor el contenido de un archivo
    /// 
    /// https://stackoverflow.com/a/61205844/12642114
    /// </summary>
    public class FileContentRequestClient : IFileContentRequestClient
    {
        private readonly ConcurrentDictionary<Guid, TaskCompletionSource<FileContentResponse>> _pendingTasks;

        private readonly IHubContext<ClassroomHubServerExtended, IClassroomHubClient> _hubContext;

        public FileContentRequestClient(IHubContext<ClassroomHubServerExtended, IClassroomHubClient> hubContext)
        {
            _hubContext = hubContext;

            _pendingTasks = new ConcurrentDictionary<Guid, TaskCompletionSource<FileContentResponse>>();
        }

        public async Task<FileContentResponse> Request(FileContentRequest request, string teacherConnectionId)
        {
            var source = new TaskCompletionSource<FileContentResponse>();
            _pendingTasks[request.Id] = source;

            await _hubContext
                .Clients
                .Client(teacherConnectionId)
                .RequestFile(request);

            return await source.Task;
        }

        public void RequestFileCallback(FileContentResponse response)
        {
            // TODO: Probar a quitar source?
            if (_pendingTasks.TryRemove(response.RequestId, out var obj) &&
                obj is { } source)
            {
                source.SetResult(response);
            }
        }
    }

    public interface IFileContentRequestClient
    {
        Task<FileContentResponse> Request(FileContentRequest request, string teacherConnectionId);

        void RequestFileCallback(FileContentResponse response);
    }
}