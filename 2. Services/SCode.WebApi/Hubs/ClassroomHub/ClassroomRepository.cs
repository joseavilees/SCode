using System.Collections.Concurrent;
using System.Collections.Generic;
using SCode.Shared.Dtos.AppFile;

namespace SCode.WebApi.Hubs.ClassroomHub
{
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly ConcurrentDictionary<string, Classroom> _classrooms;

        public ClassroomRepository()
        {
            _classrooms = new ConcurrentDictionary<string, Classroom>();
        }

        public Classroom AddOrUpdate(string name, string teacherConnectionId,
            List<AppFileEntryDto> appFileEntries)
        {
            var classroom = _classrooms
                .GetOrAdd(name, new Classroom(name));

            classroom
                .Start(teacherConnectionId, appFileEntries);

            return classroom;
        }

        public bool TryGetClassroom(string classroomName, out Classroom classroom)
        {
            classroom = null;
            
            if (string.IsNullOrEmpty(classroomName))
                return false;

            classroom = _classrooms
                .GetValueOrDefault(classroomName);
            
            return classroom != null;
        }

    }
    
    /// <summary>
    /// Almacen en memoria de las clases contenidas
    /// en el concentrador
    /// </summary>
    public interface IClassroomRepository
    {
        /// <summary>
        /// AddOrUpdate
        /// </summary>
        /// <param name="name"></param>
        /// <param name="teacherConnectionId"></param>
        /// <param name="appFileEntries"></param>
        /// <returns>Sala creada o actualizada</returns>
        Classroom AddOrUpdate(string name, string teacherConnectionId,
            List<AppFileEntryDto> appFileEntries);

        /// <summary>
        /// TryGetClassroom
        /// </summary>
        /// <param name="classroomName"></param>
        /// <param name="classroom"></param>
        bool TryGetClassroom(string classroomName, out Classroom classroom);
    }
}