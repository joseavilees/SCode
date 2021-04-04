import {Component} from '@angular/core';
import {ClassroomHubClient} from "../../../application/clients/ClassroomHubClient";


@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  isClassroomConnectionAlive = () => this.classroomHubClient
    .isConnected();

  constructor(private classroomHubClient: ClassroomHubClient) {

  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
