import {HubConnection} from "@microsoft/signalr";
import * as signalR from "@microsoft/signalr";
import {Injectable} from "@angular/core";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root',
})

export class ClassroomHubConnectionFactory {

  constructor() {
  }

  public getInstance(keepAliveIntervalMs: number = 10000): HubConnection {
    let connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.scodeApiUrl + "/hubs/classroom")
      .withAutomaticReconnect({
        // Reconectar en caso de desconexión cada 5 segundos
        nextRetryDelayInMilliseconds: () => 5000
      })
      .build();

    connection
      .keepAliveIntervalInMilliseconds = keepAliveIntervalMs;

    return connection;
  }
}
