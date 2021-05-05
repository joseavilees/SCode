import {PersistAppFileEntryChangeWhenAppFileEntryChangeEventHandler} from "./appFileEntryChangedEventHandlers/PersistAppFileEntryChangeWhenAppFileEntryChangeEventHandler";
import {PopulateAppFileEntryRepositoryWhenClassroomStartedEventHandler} from "./classroomStartedEventHandlers/PopulateAppFileEntryRepositoryWhenClassroomStartedEventHandler";
import {Injectable} from "@angular/core";
import {OpenCloseAppFileEntryOnEditorWhenAppFileEntryChangeEventHandler} from "./appFileEntryChangedEventHandlers/OpenCloseAppFileEntryOnEditorWhenAppFileEntryChangeEventHandler";
import {ExpandAppFileEntryParentNodesOnTreeWhenAppFileEntryChangeEventHandler} from "./appFileEntryChangedEventHandlers/ExpandAppFileEntryParentNodesOnTreeWhenAppFileEntryChangeEventHandler";
import {ResetEditorTopBarWhenClassroomStarted} from "./classroomStartedEventHandlers/ResetEditorTopBarWhenClassroomStarted";

// noinspection JSUnusedLocalSymbols
@Injectable({
  providedIn: 'root',
})

export class EventHandlerBootstrapper {
  constructor(eventHandler1: PersistAppFileEntryChangeWhenAppFileEntryChangeEventHandler,
              eventHandler2: PopulateAppFileEntryRepositoryWhenClassroomStartedEventHandler,
              eventHandler3: OpenCloseAppFileEntryOnEditorWhenAppFileEntryChangeEventHandler,
              eventHandler4: ExpandAppFileEntryParentNodesOnTreeWhenAppFileEntryChangeEventHandler,
              eventHandler5 : ResetEditorTopBarWhenClassroomStarted
  ) {
  }
}
