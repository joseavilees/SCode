import {ClassroomHubClient} from "../../clients/ClassroomHubClient";
import {Injectable} from "@angular/core";
import {EditorService} from "../../services/EditorService";

@Injectable({
  providedIn: 'root',
})

export class ResetEditorTopBarWhenClassroomStarted {
  constructor(private readonly editorService: EditorService,
              classroomHubClient: ClassroomHubClient) {

    classroomHubClient
      .onClassroomStartedEvent
      .subscribe(() => this.handle());
  }

  private handle() {
    this.editorService.closeAll();
  }
}
