import {AppFileEntryRepository} from "../../../infrastructure/repositories/AppFileEntryRepository";
import {NGXLogger} from "ngx-logger";
import {ClassroomHubClient} from "../../clients/ClassroomHubClient";
import {ClassroomStartedEvent} from "../../../domain/events/ClassroomStartedEvent";
import {AppFileEntry} from "../../../domain/models/AppFileEntry";
import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root',
})

export class PopulateAppFileEntryRepositoryWhenClassroomStartedEventHandler
{
  constructor(private readonly appFileEntryRepository: AppFileEntryRepository,
              private readonly logger: NGXLogger,
              classroomHubClient : ClassroomHubClient) {

    classroomHubClient
      .onClassroomStartedEvent
      .subscribe(e => this.handle(e));
  }

  private handle(event: ClassroomStartedEvent) {
    let appFileEntries = event.appFileEntries;

    try {
      this.populateRepository(appFileEntries);
    } catch (error) {
      this.logger.error("No fue posible poblar el repositorio de datos", error);
    }
  }

  private populateRepository(appFileEntries: Array<AppFileEntry>) {
    this
      .appFileEntryRepository
      .populate(appFileEntries);
  }
}
