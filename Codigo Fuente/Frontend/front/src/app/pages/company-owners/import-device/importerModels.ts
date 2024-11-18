export interface importer {
  implementationName: string;
  assemblyLocation: string;
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

