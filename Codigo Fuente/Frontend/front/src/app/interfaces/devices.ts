export interface GetDeviceRequest {
    name: string | null;
    model: string | null;
    company: string | null;
    kind: string | null;
}

export interface GetDevicesResponse {
    devices: GetDeviceResponse[];
    totalCount: number;
}

export interface GetDeviceResponse {
    name: string;
    model: string;
    photoUrl: string | null;
    companyName: string | null;
}

export interface GetDeviceTypesResponse {
    deviceTypes: string[];
}

export class PostWindowSensorRequest {
  name: string | null;
  model: string | null;
  company: number | null;
  description: string | null;
  functionalities: string[];
  photoUrls: string[];

  constructor(name: string | null, model: string | null, description: string | null, functionalities: string[], photoUrls: string[]) {
    this.name = name;
    this.model = model;
    this.description = description;
    this.functionalities = functionalities;
    this.photoUrls = photoUrls;
  }
}

export class PostSecurityCameraRequest {
  name: string | null;
  model: string | null;
  company: number | null;
  description: string | null;
  functionalities: string[];
  photoUrls: string[];
  locationType: string | null;

  constructor(name: string | null, model: string | null, description: string | null, functionalities: string[], photoUrls: string[], locationType: string | null) {
    this.name = name;
    this.model = model;
    this.description = description;
    this.functionalities = functionalities;
    this.photoUrls = photoUrls;
    this.locationType = locationType;
  }
}

export class PostSmartLampRequest {
  name: string | null;
  model: string | null;
  company: number | null;
  description: string | null;
  functionalities: string[];
  photoUrls: string[];

  constructor(name: string | null, model: string | null, description: string | null, functionalities: string[], photoUrls: string[]) {
    this.name = name;
    this.model = model;
    this.description = description;
    this.functionalities = functionalities;
    this.photoUrls = photoUrls;
  }
}

export class PostMotionSensorRequest {
  name: string | null;
  model: string | null;
  company: number | null;
  description: string | null;
  functionalities: string[];
  photoUrls: string[];

  constructor(name: string | null, model: string | null, description: string | null, functionalities: string[], photoUrls: string[]) {
    this.name = name;
    this.model = model;
    this.description = description;
    this.functionalities = functionalities;
    this.photoUrls = photoUrls;
  }
}
