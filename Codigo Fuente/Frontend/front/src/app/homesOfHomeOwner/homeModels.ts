
//agregar nombre desp
export interface home {
  id: number;
  street: string;
  doorNumber: number;
  latitude: number;
  longitude: number;
}

export interface deviceUnit {
  name: string;
  hardwareId: string;
  isConnected: boolean;
  model: number;
  photo: string;
}

export class addMemberRequest{
  id: number;
  email: string;
  hasPermissionToListDevices: boolean;
  hasPermissionToAddDevice: boolean;
  recivesNotifications: boolean;
  constructor(
    id:number,
    email: string,
    hasPermissionToListDevices: boolean,
    hasPermissionToAddDevice: boolean,
    recivesNotifications: boolean
  ) {
    this.id = id;
    this.email = email;
    this.hasPermissionToListDevices = hasPermissionToListDevices;
    this.hasPermissionToAddDevice = hasPermissionToAddDevice;
    this.recivesNotifications = recivesNotifications;
  }
}

export class addDeviceRequest{
  id: number;
  deviceId : number;
  isConnected: boolean;
  constructor(
    id: number,
    deviceId : number,
    isConnected: boolean,
  ) {
    this.id = id;
    this.deviceId = deviceId;
    this.isConnected = isConnected;
  }
}

export class addDeviceToHomeRequest {
  deviceId : number;
  isConnected: boolean;
  constructor(
    deviceId : number,
    isConnected: boolean,
  ) {
    this.deviceId = deviceId;
    this.isConnected = isConnected;
  }
}

export class addDeviceToHomeListRequest {
  deviceUnits : addDeviceToHomeRequest[] = [];

  constructor(element?: addDeviceToHomeRequest) {
    if (element) {
      this.addElement(element);
    }
  }

  addElement(element: addDeviceToHomeRequest) {
    this.deviceUnits.push(element);
  }
}



export class addMemberToHomeRequest {
  userEmail: string;
  hasPermissionToListDevices: boolean;
  hasPermissionToAddDevice: boolean;
  recivesNotifications: boolean;

  constructor(
    userEmail: string,
    hasPermissionToListDevices: boolean,
    hasPermissionToAddDevice: boolean,
    recivesNotifications: boolean
  ) {
    this.userEmail = userEmail;
    this.hasPermissionToListDevices = hasPermissionToListDevices;
    this.hasPermissionToAddDevice = hasPermissionToAddDevice;
    this.recivesNotifications = recivesNotifications;
  }
}

export interface member {
  fullName: string;
  email: string;
  photo: string;
  hasPermissionToListDevices: boolean,
  hasPermissionToAddDevice: boolean,
  recivesNotifications: boolean
}




