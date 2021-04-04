import { StringHelper } from "./StringHelper";

export class MonacoHelper {

  static getLanguageByFileName(fileName: string) {
    const extension = StringHelper
      .getFileNameExtension(fileName);

    switch (extension) {
      case "js":
      case "mjs":
        return "javascript";

      case "ts":
        return "typescript";

      case "md":
        return "markdown";

      case "cs":
        return "csharp";

      case "kt":
      case "kts":
      case "ktm":
        return "kotlin";

      default:
        return extension;
    }
  }

}
