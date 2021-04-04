using System.Collections.Generic;
using System.Threading.Tasks;
using SCode.Shared.Dtos.AppFile;
using SCode.Shared.Requests.FileContentRequests;

namespace SCode.WebApi.Hubs.ClassroomHub
{
    public interface IClassroomHubClient
    {
        /// <summary>
        /// Notifica de la clase ha comenzado
        /// </summary>
        Task ClassroomHasStarted();


        /// <summary>
        /// Notifica de un conjuntos de cambios
        /// </summary>
        /// <param name="changes"></param>
        Task NewAppFileEntryChanges(List<AppFileEntryChangeDto> changes);

        /// <summary>
        /// Solicita el contenido de un archivo a un
        /// cliente (maestro) y lo retoma al
        /// concentrador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task RequestFile(FileContentRequest request);
    }
}