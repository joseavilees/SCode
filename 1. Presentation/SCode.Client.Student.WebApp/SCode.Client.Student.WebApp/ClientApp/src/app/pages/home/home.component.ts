import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {SessionService} from "../../application/services/SessionService";
import {ClassroomHubClient} from "../../application/clients/ClassroomHubClient";
import {EventHandlerBootstrapper} from "../../application/eventHandlers/EventHandlerBootstrapper";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {


  // noinspection JSUnusedLocalSymbols
  constructor(private readonly sessionService: SessionService,
              private readonly classroomHubClient: ClassroomHubClient,
              private readonly route: ActivatedRoute,
              private readonly eventHandlerBootstrapper : EventHandlerBootstrapper) {
  }


  async ngOnInit() {
    // Capturar el nombre de la sala
    const classroomName = this.route
      .snapshot
      .paramMap
      .get('hubRoomName');

    this
      .sessionService
      .classroomName = classroomName;

    await this
      .classroomHubClient
      .connect();

    await this
      .classroomHubClient
      .joinClassroom();
  }
}
