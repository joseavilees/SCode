import {AppFileEntryChange} from "../models/AppFileEntryChange";

export class AppFileEntryChangedEvent {

  change: AppFileEntryChange;

  constructor(change: AppFileEntryChange) {
    this.change = change;
  }
}
