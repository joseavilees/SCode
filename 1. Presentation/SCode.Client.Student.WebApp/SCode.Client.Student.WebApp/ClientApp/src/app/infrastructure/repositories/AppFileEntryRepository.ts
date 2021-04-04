import {AppFileEntry} from "../../domain/models/AppFileEntry";
import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root',
})

export class AppFileEntryRepository {

  private data: Array<AppFileEntry>;


  constructor() {
    this.data = new Array<AppFileEntry>();
  }

  public populate(data: Array<AppFileEntry>) {
    this.data = data;
  }

  public get(id: number): AppFileEntry {
    return this.data.find(x => x.id == id);
  }

  public getAll(): Array<AppFileEntry> {
    return this.data;
  }

  public insert(entry: AppFileEntry) {
    this.data.push(entry);
  }

  public update(id: number, name: string, content: string): AppFileEntry {
    let oldEntry = this.get(id);

    if(oldEntry == null)
      return ;

    oldEntry.name = name;

    if (content != null)
      oldEntry.content = content;

    return oldEntry;
  }

  public delete(id: number) {
    // Eliminar padre e hijos
    this.data = this.data
      .filter(x => x.id != id && x.parentId != id);
  }
}
