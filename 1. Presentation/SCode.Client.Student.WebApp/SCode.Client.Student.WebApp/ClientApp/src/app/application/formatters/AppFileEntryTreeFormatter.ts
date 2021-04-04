import {AppFileEntry} from "../../domain/models/AppFileEntry";

export class AppFileEntryTreeFormatter {

  // Dado un conjunto de entradas de archivos devolverlo con forma
  // de árbol al igual que en el sistema de archivos tradicional
  // (Los directorios contienen a sus hijos)
  public static formatToTree(src: Array<AppFileEntry>): Array<AppFileEntry> {
    let tree = this.traverse(src, src.filter(x => x.parentId == 0));

    return tree;
  }

  private static traverse(allAppFileEntries: Array<AppFileEntry>, root: Array<AppFileEntry>): Array<AppFileEntry> {
    root.forEach(entry => {
      if (entry.isDirectory) {
        entry.children = allAppFileEntries
          .filter(x => x.parentId == entry.id)

        this.traverse(allAppFileEntries, entry.children);
      }
    })

    return root.sort(this.sortCompareFunction);
  }

  // Función para ordenar la colección de forma que los
  // directorios se posicionen al inicio de esta
  static sortCompareFunction = (x, y) =>
    Number(y.isDirectory) - Number(x.isDirectory);
}

