import {Injectable} from "@angular/core";
import {NGXLogger} from "ngx-logger";
import {ClassroomHubConnectionFactory} from "../factories/ClassroomHubConnectionFactory";
import {HubConnection} from "@microsoft/signalr";
import * as signalR from "@microsoft/signalr";
import {SessionService} from "../services/SessionService";
import {AppFileEntry} from "../../domain/models/AppFileEntry";
import {ClassroomStartedEvent} from "../../domain/events/ClassroomStartedEvent";
import {SimpleEventDispatcher} from "ste-simple-events";
import {AppFileEntryChangedEvent} from "../../domain/events/AppFileEntryChangedEvent";
import {AppFileEntryChange} from "../../domain/models/AppFileEntryChange";
import {FileTreeService} from "../services/FileTreeService";
import {delay} from "../helpers/AngularHelper";
import {FileContentResponse} from "../requests/fileContentRequests/FileContentResponse";

@Injectable({
  providedIn: 'root',
})

export class ClassroomHubClient {
  private readonly _classroomStartedEvent = new SimpleEventDispatcher<ClassroomStartedEvent>();

  public get onClassroomStartedEvent() {
    return this._classroomStartedEvent.asEvent();
  }

  private readonly _onAppFileChangedEvent = new SimpleEventDispatcher<AppFileEntryChangedEvent>();
  public get onAppFileChangedEvent() {
    return this._onAppFileChangedEvent.asEvent();
  }

  private readonly connection: HubConnection;

  constructor(private readonly logger: NGXLogger,
              private readonly sessionService: SessionService,
              private readonly factory: ClassroomHubConnectionFactory,
              private readonly fileTreeService: FileTreeService) {

    this.connection = factory.getInstance();

    this.connection
      .onreconnected(() => this.logger.info(async () => {
        this.logger.info("Sala: Reconectado");
        await this.joinClassroom();
      }));

    this.connection
      .onreconnecting(() => this.logger.info("Sala: Reconectando..."));

    // Se ejecuta si la clase se inicia después de que el cliente se haya conectado
    this.connection.on("ClassroomHasStarted",
      async () => await this.joinClassroom());

    this.connection.on("NewAppFileEntryChanges",
      (changes: Array<AppFileEntryChange>) => this.onNotifyAppFileEntryChanges(changes));
  }

  isConnected = () => this.connection?.state ===
    signalR.HubConnectionState.Connected;

  public async connect() {
    try {
      await this.connection.start();

      console.assert(this.connection.state === signalR.HubConnectionState.Connected);

      this.logger.info("Conectado al Hub");
    } catch (error) {

      this.logger.error("No fue posible conectar al Hub. Reintentar...", error);

      // Reintentar cada 5 segundos en caso de fallo
      setTimeout(() => this.connect(), 5000);
    }
  }

  public async joinClassroom() {
    try {
      let classroomName = this.sessionService
        .classroomName;

      let appFileEntries = await this
        .connection
        .invoke<Array<AppFileEntry>>("JoinClassroom", classroomName);

      await this
        ._classroomStartedEvent
        .dispatch(new ClassroomStartedEvent(appFileEntries))

      this.logger.info("Unido a la clase");

      this.fileTreeService.update();

    } catch (error) {
      this.logger.error("No fue posible unirse a la clase", error);

      // Reintentar cada 5 segundos en caso de fallo
      setTimeout(() => this.joinClassroom(), 5000);
    }
  }

  public requestFileContent(id: number) {
    let classroomName = this.sessionService
      .classroomName;

    return this.connection
      .invoke<FileContentResponse>("GetFileContent",
        id, classroomName);
  }

  private async onNotifyAppFileEntryChanges(changes: Array<AppFileEntryChange>) {
    this.logger.debug("Obtenidos cambios: ", changes);

    for (const change of changes) {
      await this
        ._onAppFileChangedEvent
        .dispatchAsync(new AppFileEntryChangedEvent(change));
    }

    // TODO: Cambiar. Esperar a que los eventos
    //  se propaguen
    await delay(100);

    this.fileTreeService.update();
  }
}
