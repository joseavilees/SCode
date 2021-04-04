import { Component, Input, OnInit } from '@angular/core';
import {EditorService} from "../../application/services/EditorService";
import {StringHelper} from "../../application/helpers/StringHelper";

@Component({
  selector: 'app-file-nav-item',
  templateUrl: './file-nav-item.component.html',
  styleUrls: ['./file-nav-item.component.css']
})
export class FileNavItemComponent  {

  @Input() fileTreeItem;

  isSelected() {
    return this.editorService
      .selectedAppFile.id == this.fileTreeItem.id;
  }

  constructor(private editorService : EditorService)
  {

  }

  getFileNameExtension(fileName: string) {
    return StringHelper.getFileNameExtension(fileName);
  }

  close() {
    this.editorService.closeFile(this.fileTreeItem.id);
  }

  open()
  {
    this.editorService.openFile(this.fileTreeItem);
  }
}
