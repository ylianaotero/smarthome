export interface importer {
  implementationName: string;
  assemblyLocation: string;
}

export class ImporterPath {
  Path: string;

  constructor(dllPath: string) {
    this.Path = dllPath;
  }
}




export class ImportDevicesRequest {
  DllPath: string;
  FilePath: string;
  Type: string;

  constructor(dllPath: string, filePath: string, type: string) {
    this.DllPath = dllPath;
    this.FilePath = filePath;
    this.Type = type;
  }
}

