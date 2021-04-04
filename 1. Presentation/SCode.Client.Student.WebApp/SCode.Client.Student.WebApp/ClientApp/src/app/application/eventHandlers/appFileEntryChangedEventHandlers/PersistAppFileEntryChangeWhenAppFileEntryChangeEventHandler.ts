import {AppFileEntryChangedEvent} from "../../../domain/events/AppFileEntryChangedEvent";
import {AppFileEntryChangeType} from "../../../domain/models/AppFileEntryChangeType";
import {AppFileEntryRepository} from "../../../infrastructure/repositories/AppFileEntryRepository";
import {AppFileEntryChange} from "../../../domain/models/AppFileEntryChange";
import {NGXLogger} from "ngx-logger";
import {ClassroomHubClient} from "../../clients/ClassroomHubClient";
import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root',
})

export class PersistAppFileEntryChangeWhenAppFileEntryChangeEventHandler {

  constructor(private readonly appFileEntryRepository: AppFileEntryRepository,
              private readonly logger: NGXLogger,
              classroomHubClient : ClassroomHubClient) {

    classroomHubClient
      .onAppFileChangedEvent
      .subscribe(e => this.handle(e));
  }

  private async handle(event: AppFileEntryChangedEvent) {
    let change = event.change;

    try {
      this.persistChange(change);
    } catch (error) {
      this.logger.error("No fue posible procesar el cambio", error);
    }
  }

  private persistChange(change: AppFileEntryChange) {

    let appFileEntry = change.appFileEntry;
    let changeType = change.changeType;
    let id = appFileEntry.id;

    if (changeType === AppFileEntryChangeType.Changed ||
      changeType === AppFileEntryChangeType.Renamed) {

      this
        .appFileEntryRepository
        .update(id, appFileEntry.name, change.content);

    } else if (changeType === AppFileEntryChangeType.Created) {

      this
        .appFileEntryRepository
        .insert(appFileEntry);
    } else if (changeType === AppFileEntryChangeType.Deleted) {

      this
        .appFileEntryRepository
        .delete(id);
    }
  }
}
