import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root',
})

export class SessionService
{
  public classroomName : string;
  public isAutoPilotEnabled: boolean;

  constructor() {
    this.isAutoPilotEnabled = true;
  }
}
