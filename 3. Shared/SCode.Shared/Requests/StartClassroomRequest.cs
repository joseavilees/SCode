using System.Collections.Generic;
using SCode.Shared.Dtos.AppFile;

namespace SCode.Shared.Requests
{
    public class StartClassroomRequest
    {
        public string ClassroomName { get; set; }
        public List<AppFileEntryDto> AppFileEntries { get; set; }

        public StartClassroomRequest(string classroomName, List<AppFileEntryDto> appFileEntries)
        {
            ClassroomName = classroomName;
            AppFileEntries = appFileEntries;
        }
    }
}