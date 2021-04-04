using System.Collections.Generic;
using SCode.Shared.Dtos.AppFile;

namespace SCode.Shared.Requests
{
    public class AppFileEntryChangeRequest
    {
        public string ClassRoomName { get; set; }

        public List<AppFileEntryChangeDto> Changes { get; set; }
    }
}
