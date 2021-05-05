using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SCode.Shared.Dtos.AppFile;

namespace SCode.WebApi.Hubs.ClassroomHub
{
    public class Classroom
    {
        public string Name { get; }

        public string TeacherConnectionId { get; private set; }

        private List<AppFileEntryDto> _appFileEntries;

        /// <summary>
        /// Colección actualizada en tiempo real de la relación de
        /// entidades de archivos
        /// </summary>
        public ReadOnlyCollection<AppFileEntryDto> AppFileEntries =>
            _appFileEntries.AsReadOnly();

        public Classroom(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Inicia la clase.
        /// Se debe de indicar la conexión del docente que la inicia y la colección
        /// de entradas de archivos actual
        /// </summary>
        /// <param name="teacherConnectionId"></param>
        /// <param name="appFileEntryDtos"></param>
        public void Start(string teacherConnectionId, List<AppFileEntryDto> appFileEntryDtos)
        {
            TeacherConnectionId = teacherConnectionId;
            _appFileEntries = appFileEntryDtos;
        }

        /// <summary>
        /// Actualiza la colección de entrada de archivos con los
        /// cambios producidos en <param name="changes"></param>
        /// </summary>
        /// <param name="changes"></param>
        public void UpdateAppFileEntries(List<AppFileEntryChangeDto> changes)
        {
            foreach (var change in changes)
                UpdateAppFileEntries(change);
        }

        private void UpdateAppFileEntries(AppFileEntryChangeDto change)
        {
            var appFileEntry = change.AppFileEntry;
            var changeType = change.ChangeType;

            if (changeType == AppFileEntryChangeTypeDto.Created)
            {
                _appFileEntries.Add(appFileEntry);
            }
            else if (changeType == AppFileEntryChangeTypeDto.Deleted)
            {
                _appFileEntries.Remove(appFileEntry);
            }
            else if (changeType == AppFileEntryChangeTypeDto.Renamed)
            {
                _appFileEntries
                    .First(x => x.Id == appFileEntry.Id)
                    .Name = appFileEntry.Name;
            }
        }

        #region Equality Comparers

        protected bool Equals(Classroom other)
        {
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Classroom) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
        }

        public static bool operator ==(Classroom left, Classroom right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Classroom left, Classroom right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}