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

  constructor(name: string | null, model: string | null, description: string | null, functionalities: string[], photoUrls: string[], company: number | null) {
    this.name = name;
    this.model = model;
    this.description = description;
    this.functionalities = functionalities;
    this.photoUrls = photoUrls;
    this.company = company;
  }
}

export class PostSecurityCameraRequest {
  name: string | null;
  Model: string | null;
  Company: number | null;
  Description: string | null;
  Functionalities: string[];
  PhotoUrls: string[];
  LocationType: string | null;

  constructor(name: string | null, model: string | null, description: string | null, functionalities: string[], photoUrls: string[], locationType: string | null, company: number | null) {
    this.name = name;
    this.Model = model;
    this.Description = description;
    this.Functionalities = functionalities;
    this.PhotoUrls = photoUrls;
    this.LocationType = locationType;
    this.Company = company;
  }
}

export class PostSmartLampRequest {
  name: string | null;
  model: string | null;
  company: number | null;
  description: string | null;
  functionalities: string[];
  photoUrls: string[];

  constructor(name: string | null, model: string | null, description: string | null, functionalities: string[], photoUrls: string[], company: number | null) {
    this.name = name;
    this.model = model;
    this.description = description;
    this.functionalities = functionalities;
    this.photoUrls = photoUrls;
    this.company = company;
  }
}

export class PostMotionSensorRequest {
  name: string | null;
  model: string | null;
  company: number | null;
  description: string | null;
  functionalities: string[];
  photoUrls: string[];

  constructor(name: string | null, model: string | null, description: string | null, functionalities: string[], photoUrls: string[], company: number | null) {
    this.name = name;
    this.model = model;
    this.description = description;
    this.functionalities = functionalities;
    this.photoUrls = photoUrls;
    this.company = company;
  }
}
