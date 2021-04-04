import {Component, OnInit, ViewChild} from '@angular/core';
import {ITreeOptions, TreeComponent} from '@circlon/angular-tree-component';
import {ITreeNode} from '@circlon/angular-tree-component/lib/defs/api';
import {EditorService} from "../../application/services/EditorService";
import {StringHelper} from "../../application/helpers/StringHelper";
import {FileTreeService} from "../../application/services/FileTreeService";

@Component({
  selector: 'app-file-tree-component',
  templateUrl: './file-tree.component.html',
  styleUrls: ['./file-tree.component.css']
})

export class FileTreeComponent implements OnInit {

  @ViewChild(TreeComponent)
  private tree: TreeComponent;

  options: ITreeOptions =
    {
      hasChildrenField: 'isDirectory'
    };

  constructor(private readonly editorService: EditorService,
              readonly fileTreeService: FileTreeService) {
  }

  ngOnInit(): void {

  }

  ngAfterViewInit() {
    this.fileTreeService.tree = this.tree;
  }

  onNodeClicked(event) {
    let fileTreeItemNode: ITreeNode = event.node;

    // Si es un directorio expandir el nodo
    if (fileTreeItemNode.hasChildren)
      fileTreeItemNode
        .toggleExpanded();

    // Si es un archivo abrirlo en el editor
    if (!fileTreeItemNode.data.isDirectory)
      this.editorService
        .openFile(fileTreeItemNode.data);
  }

  getFileNameExtension(fileName) {
    return StringHelper.getFileNameExtension(fileName);
  }
}
