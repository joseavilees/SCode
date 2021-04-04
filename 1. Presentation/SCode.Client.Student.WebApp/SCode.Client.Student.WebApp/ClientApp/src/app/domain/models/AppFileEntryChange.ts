import {AppFileEntryChangeType} from "./AppFileEntryChangeType";
import {AppFileEntry} from "./AppFileEntry";

export class AppFileEntryChange
{
  changeType: AppFileEntryChangeType;

  appFileEntry : AppFileEntry;

  content: string | null;
}
