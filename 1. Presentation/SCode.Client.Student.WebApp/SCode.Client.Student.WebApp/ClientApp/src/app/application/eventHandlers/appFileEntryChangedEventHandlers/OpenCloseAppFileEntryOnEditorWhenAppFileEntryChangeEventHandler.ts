import {Injectable} from "@angular/core";
import {AppFileEntryRepository} from "../../../infrastructure/repositories/AppFileEntryRepository";
import {NGXLogger} from "ngx-logger";
import {EditorService} from "../../services/EditorService";
import {ClassroomHubClient} from "../../clients/ClassroomHubClient";
import {AppFileEntryChangedEvent} from "../../../domain/events/AppFileEntryChangedEvent";
import {AppFileEntryChange} from "../../../domain/models/AppFileEntryChange";
import {AppFileEntryChangeType} from "../../../domain/models/AppFileEntryChangeType";
import {FileTreeService} from "../../services/FileTreeService";
import {SessionService} from "../../services/SessionService";

@Injectable({
  providedIn: 'root',
})

export class OpenCloseAppFileEntryOnEditorWhenAppFileEntryChangeEventHandler {
  constructor(private readonly appFileEntryRepository: AppFileEntryRepository,
              private readonly logger: NGXLogger,
              private readonly editorService: EditorService,
              private readonly sessionService: SessionService,
              private readonly fileTreeService: FileTreeService,
              classroomHubClient: ClassroomHubClient) {

    classroomHubClient
      .onAppFileChangedEvent
      .subscribe(e => this.handle(e));
  }

  private handle(event: AppFileEntryChangedEvent) {
    let change = event.change;

    try {
      this.openCloseAppFileEntryOnEditor(change);
    } catch (error) {
      this.logger.error("No fue posible procesar el cambio", error);
    }
  }

  private openCloseAppFileEntryOnEditor(change: AppFileEntryChange) {

    if (!this.sessionService.isAutoPilotEnabled)
      return;

    if (change.appFileEntry.isDirectory)
      return;

    let appFileEntry = change.appFileEntry;
    let changeType = change.changeType;
    let id = appFileEntry.id;

    if (changeType === AppFileEntryChangeType.Changed) {
      appFileEntry = this
        .appFileEntryRepository
        .get(id);

      this.editorService.openFile(appFileEntry);

    } else if (changeType === AppFileEntryChangeType.Created) {
      this.editorService.openFile(appFileEntry);
    } else if (changeType === AppFileEntryChangeType.Deleted) {
      this.editorService.closeFile(id);
    }
  }
}
