export class StringHelper {

    static getFileNameExtension(fileName : string) {

        let regexp = new RegExp('\.([A-Za-z0-9]+)$');
        let result = regexp.exec(fileName);
    
        if (result != null)
          return result[1];
    }
}