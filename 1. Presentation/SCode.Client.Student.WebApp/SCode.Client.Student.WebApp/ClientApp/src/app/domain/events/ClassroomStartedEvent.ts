import {AppFileEntry} from "../models/AppFileEntry";

export class ClassroomStartedEvent
{
  appFileEntries : Array<AppFileEntry>;

  constructor(appFileEntries: Array<AppFileEntry>) {
    this.appFileEntries = appFileEntries;
  }
}
