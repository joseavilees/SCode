using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SCode.Shared.Dtos.AppFile;
using SCode.Shared.Requests;

namespace SCode.WebApi.Hubs.ClassroomHub
{
    public class ClassroomHubServer : Hub<IClassroomHubClient>
    {
        protected readonly IClassroomRepository ClassroomRepository;
        protected readonly ILogger<ClassroomHubServer> Logger;

        public ClassroomHubServer(IClassroomRepository classroomRepository, ILogger<ClassroomHubServer> logger)
        {
            ClassroomRepository = classroomRepository;
            Logger = logger;
        }


        [Authorize]
        public bool StartClassroom(StartClassroomRequest request)
        {
            if (request == null)
                return false;

            var classroomName = request.ClassroomName;
            var appFileEntries = request.AppFileEntries;

            try
            {
                var teacherConnectionId = Context.ConnectionId;

                ClassroomRepository.AddOrUpdate(classroomName,
                    teacherConnectionId, appFileEntries);

                // Notificar a los estudiantes del inicio de la clase
                Clients
                    .Group(classroomName)
                    .ClassroomHasStarted();

                Logger.LogInformation("Clase iniciada {Classroom}",
                    classroomName);

                return true;
            }
            catch (Exception ex) when (ex is not HubException)
            {
                Logger.LogError(ex,
                    "No fue posible iniciar la clase {Classroom}",
                    classroomName);

                return false;
            }
        }


        /// <summary>
        /// Notifica a los estudiantes de los cambios de
        /// entidades de archivos producidos
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        public async Task NotifyAppFileEntryChange(AppFileEntryChangeRequest request)
        {
            if (request == null)
                throw new HubException("Solicitud no válida");

            var classRoomName = request.ClassRoomName;
            var changes = request.Changes;

            try
            {
                if (!ClassroomRepository.TryGetClassroom(classRoomName, out var classroom))
                    throw new HubException("Sala no encontrada");

                // Notificar a los clientes de la clase el cambio
                await Clients
                    .Group(classroom.Name)
                    .NewAppFileEntryChanges(changes);

                // Actualiza las entidades de archivo de la clase
                classroom
                    .UpdateAppFileEntries(changes);

                Logger.LogDebug("Registrado cambio {Classroom} {@Changes} ",
                    classRoomName, changes);
            }
            catch (Exception ex) when (ex is not HubException)
            {
                Logger.LogError(ex, "No fue posible registar el cambio {Classroom} {@Changes} ",
                    classRoomName, changes);
            }
        }

        /// <summary>
        /// Subscribirse a recibir eventos de una clase
        /// </summary>
        /// <param name="classRoomName"></param>
        /// <returns>Entidades de archivos existentes</returns>
        public async Task<IEnumerable<AppFileEntryDto>> JoinClassroom(string classRoomName)
        {
            try
            {
                if (!ClassroomRepository.TryGetClassroom(classRoomName, out var classroom))
                    throw new HubException("No se encontró la clase");

                // Unirse
                await Groups
                    .AddToGroupAsync(Context.ConnectionId, classRoomName);

                Logger.LogInformation("Usuario conectado a {Classroom}",
                    classRoomName);

                return classroom.AppFileEntries;
            }
            catch (Exception ex) when (ex is not HubException)
            {
                Logger.LogError(ex, "No fue posible unirse a la clase {Classroom} ",
                    classRoomName);

                throw new HubException("Fallo desconocido");
            }
        }
    }
}