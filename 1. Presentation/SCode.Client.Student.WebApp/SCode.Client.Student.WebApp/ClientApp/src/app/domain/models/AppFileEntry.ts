export class AppFileEntry {
  id: number;
  parentId: number;

  name: string;
  isDirectory: boolean;

  content: string | null;

  isActive: boolean;
  children : Array<AppFileEntry>;
}
