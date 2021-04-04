import {Component, OnInit} from '@angular/core';
import {EditorService} from "../../application/services/EditorService";

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.css']
})

export class EditorComponent implements OnInit {

  private monacoEditor;

  public getActiveFileTreeItems = () =>
    this.editorService.activeAppFiles;

  public selectedAppFile = () =>
    this.editorService.selectedAppFile;

  public options = () =>
    this.editorService.options;

  constructor(private readonly editorService: EditorService) {
  }

  onEditorInit(editor) {

    this.monacoEditor = editor;
    this.monacoEditor.focus();
  }

  async ngOnInit() {
  }

}
