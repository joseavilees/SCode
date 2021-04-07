import {Injectable} from "@angular/core";
import {NGXLogger} from "ngx-logger";
import {ClassroomHubClient} from "../../clients/ClassroomHubClient";
import {AppFileEntryChangedEvent} from "../../../domain/events/AppFileEntryChangedEvent";
import {AppFileEntryChange} from "../../../domain/models/AppFileEntryChange";
import {AppFileEntryChangeType} from "../../../domain/models/AppFileEntryChangeType";
import {FileTreeService} from "../../services/FileTreeService";
import {SessionService} from "../../services/SessionService";

@Injectable({
  providedIn: 'root',
})

export class ExpandAppFileEntryParentNodesOnTreeWhenAppFileEntryChangeEventHandler {
  constructor(private readonly logger: NGXLogger,
              private readonly fileTreeService: FileTreeService,
              private readonly sessionService: SessionService,
              classroomHubClient: ClassroomHubClient) {

    classroomHubClient
      .onAppFileChangedEvent
      .subscribe(e => this.handle(e));
  }

  private handle(event: AppFileEntryChangedEvent) {
    let change = event.change;

    try {
      this.expandAppFileEntryParentNodesOnTree(change);
    } catch (error) {
      this.logger.error("No fue posible procesar el cambio", error);
    }
  }

  private expandAppFileEntryParentNodesOnTree(change: AppFileEntryChange) {
    if (!this.sessionService.isAutoPilotEnabled)
      return;

    if (change.appFileEntry.isDirectory ||
      change.changeType == AppFileEntryChangeType.Deleted)
      return;

    this.expandNodes(change.appFileEntry.parentId);
  }

  // Debe de abrir todos los nodos padres
  // del nodo especificado recursivamente
  private expandNodes(parentId: number) {
    while (parentId != 0) {
      let node = this.fileTreeService
        .getNode(parentId);

      this.fileTreeService
        .expand(node, true);

      parentId = node.parentId;
    }
  }
}
