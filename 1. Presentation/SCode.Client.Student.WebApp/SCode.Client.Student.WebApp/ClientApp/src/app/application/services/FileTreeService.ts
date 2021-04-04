import {TreeComponent} from "@circlon/angular-tree-component";
import {Injectable} from "@angular/core";
import {AppFileEntryRepository} from "../../infrastructure/repositories/AppFileEntryRepository";
import {AppFileEntry} from "../../domain/models/AppFileEntry";
import {AppFileEntryTreeFormatter} from "../formatters/AppFileEntryTreeFormatter";

@Injectable({
  providedIn: 'root',
})

export class FileTreeService {
  public tree: TreeComponent;

  public nodes: Array<AppFileEntry>;

  constructor(private readonly appFileEntryRepository: AppFileEntryRepository) {
  }

  public expand(node: AppFileEntry, value: boolean) {
    this.tree
      .treeModel
      .setExpandedNode(node, value);
  }

  public getNode(id) : AppFileEntry {
    return this.findNote(id, this.nodes);
  }

  private findNote(id, arr) {
    return arr.reduce((a, item) => {
      if (a) return a;
      if (item.id === id) return item;
      if (item.children) return this.findNote(id, item.children);
    }, null);
  }

  public update() {
    let items = this.appFileEntryRepository
      .getAll();

    this.nodes = AppFileEntryTreeFormatter
      .formatToTree(items);

    this.tree.treeModel.update();
  }
}
