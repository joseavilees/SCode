import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-status-indicator',
  templateUrl: './status-indicator.component.html',
  styleUrls: ['./status-indicator.component.css']
})
export class StatusIndicatorComponent {

  @Input() status : boolean;

}
