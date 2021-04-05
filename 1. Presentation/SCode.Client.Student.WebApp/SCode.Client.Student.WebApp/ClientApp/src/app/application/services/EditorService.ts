import {Injectable} from "@angular/core";
import {NGXLogger} from "ngx-logger";
import {AppFileEntry} from "../../domain/models/AppFileEntry";
import {ArrayHelper} from "../helpers/ArrayHelper";
import {ClassroomHubClient} from "../clients/ClassroomHubClient";
import {MonacoHelper} from "../helpers/MonacoHelper";

@Injectable({
  providedIn: 'root',
})


export class EditorService {

  // Archivos abiertos en la barra del editor
  public activeAppFiles = new Array<AppFileEntry>();

  public selectedAppFile: AppFileEntry;

  public options = {
    theme: 'vs-dark',
    language: "java",
    automaticLayout: true,
    readOnly: true
  };

  constructor(private readonly logger: NGXLogger,
              private readonly classroomHubClient: ClassroomHubClient) {

  }

  public openFile(appFileEntry: AppFileEntry) {
    // Desactivar el archivo anterior del arbol
    if (this.selectedAppFile != null)
      this.selectedAppFile
        .isActive = false;

    // Marcar el nuevo archivo actual
    this.selectedAppFile = appFileEntry;

    // Activar el archivo actual en el arbol
    this.selectedAppFile
      .isActive= true;

    // Si el archivo no existe en el editor, añadirlo
    if (!this.existFile(appFileEntry))
      this.insertFile(appFileEntry);


    this.updateEditorOptions(appFileEntry);
  }

  public closeFile(id : number) {
    let appFileEntry = this.activeAppFiles
      .find(x=> x.id == id);

    if (appFileEntry != null)
    {
      appFileEntry.isActive = false;

      ArrayHelper.removeItem(this.activeAppFiles, appFileEntry);

      // Si es el archivo actual
      if (appFileEntry == this.selectedAppFile)
        this.selectedAppFile = null; // Quitarlo
    }
  }

  // public closeAll() {
  //   this.activeAppFiles = new Array<AppFileEntry>();
  // }

  public existFile(appFile: AppFileEntry): boolean {
    const containsAppFilePredicate = (element) =>
      element.id === appFile.id;

    return this.activeAppFiles.some(containsAppFilePredicate);
  }

  private insertFile(appFile: AppFileEntry) {
    this.activeAppFiles.push(appFile);

    // Si no tiene contenido, solicitarlo
    if (appFile.content == null) {
      this
        .classroomHubClient
        .requestFileContent(appFile.id)
        .then((fileContent) => {
          appFile.content = fileContent.content;
        })
        .catch(err => this.logger.error("No fue posible conseguir el contenido del archivo", err));
    }
  }

  private updateEditorOptions(appFileEntry: AppFileEntry) {
    const appFileLanguage = MonacoHelper
      .getLanguageByFileName(appFileEntry.name);

    if (this.options.language != appFileLanguage) {
      this.options = {
        theme: 'vs-dark',
        language: appFileLanguage,
        automaticLayout: true,
        readOnly: true
      };
    }
  }
}
