import {Component, OnInit} from '@angular/core';
import {EditorService} from "../../application/services/EditorService";

@Component({
  selector: 'app-editor-topbar',
  templateUrl: './editor-topbar.component.html',
  styleUrls: ['./editor-topbar.component.css']
})
export class EditorTopbarComponent implements OnInit {

  public getActiveFileTreeItems = () =>
    this.editorService.activeAppFiles;

  constructor(private readonly editorService: EditorService) {
  }

  ngOnInit(): void {
  }

}
