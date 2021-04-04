import { Component, OnInit } from '@angular/core';
import {SessionService} from "../../application/services/SessionService";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  constructor(readonly sessionService : SessionService)
  {

  }

  ngOnInit(): void {
  }

}
