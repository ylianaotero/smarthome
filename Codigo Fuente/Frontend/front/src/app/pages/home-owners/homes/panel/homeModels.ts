
export interface home {
  id: number;
  street: string;
  doorNumber: number;
  latitude: number;
  longitude: number;
}

export interface patchDeviceRequest {
  Name: string;
  HardwareId: string;
  HomeId: number;
}

export interface deviceUnit {
  name: string;
  hardwareId: string;
  isConnected: boolean;
  model: number;
  photo: string;
}

export class patchDeviceAlias {
  HardwareId: string;
  Name: string;

  constructor(HardwareId: string, Name: string) {
    this.HardwareId = HardwareId;
    this.Name = Name;
  }
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
  UserEmail: string = "";
  HasPermissionToListDevices: boolean = false;
  HasPermissionToAddADevice: boolean = false;
  ReceivesNotifications: boolean = false;

  constructor(
    userEmail: string,
    hasPermissionToListDevices: boolean,
    hasPermissionToAddDevice: boolean,
    receivesNotifications: boolean
  ) {
    this.UserEmail = userEmail;
    this.HasPermissionToListDevices = hasPermissionToListDevices;
    this.HasPermissionToAddADevice = hasPermissionToAddDevice;
    this.ReceivesNotifications = receivesNotifications;
  }
}

export class ChangeMemberNotificationsRequest {
  IdHome: number;
  MemberEmail: string;
  ReceivesNotifications: boolean;

  constructor(idHome: number, memberEmail: string, receivesNotifications: boolean) {
    this.IdHome = idHome;
    this.MemberEmail = memberEmail;
    this.ReceivesNotifications = receivesNotifications;
  }
}

export class ChangeMemberRequest {
  MemberEmail: string;
  ReceivesNotifications: boolean;

  constructor(memberEmail: string, receivesNotifications: boolean) {
    this.MemberEmail = memberEmail;
    this.ReceivesNotifications = receivesNotifications;
  }
}



export interface member {
  fullName: string;
  email: string;
  photo: string;
  hasPermissionToListDevices: boolean,
  hasPermissionToAddDevice: boolean,
  receivesNotifications: boolean
}




